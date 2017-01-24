using Axis.Constant;
using Axis.Data.Repository;
using Axis.DataProvider.Admin;
using Axis.DataProvider.Common;
using Axis.DataProvider.MessageTemplate;
using Axis.Helpers.Caching;
using Axis.Logging;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;
using Axis.Model.Common.MessageTemplate;
using Axis.Model.Setting;
using Axis.NotificationService.Email;
using Axis.NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Axis.DataProvider.Account
{
    /// <summary>
    ///
    /// </summary>
    public class ForgotPasswordDataProvider : IForgotPasswordDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork = null;

        /// <summary>
        /// The email service
        /// </summary>
        private readonly IEmailService emailService = null;

        /// <summary>
        /// The email template data provider
        /// </summary>
        private readonly IMessageTemplateDataProvider messageTemplateDataProvider = null;

        /// <summary>
        /// The admin data provider
        /// </summary>
        private readonly IAdminDataProvider adminDataProvider = null;

        /// <summary>
        /// The security question data provider
        /// </summary>
        private readonly ISecurityQuestionDataProvider securityQuestionDataProvider;

        // TO DO - Email address & display name will be retrieved from database's setting table
        /// <summary>
        /// From email
        /// </summary>
        private readonly string fromEmail = string.Empty;

        /// <summary>
        /// From display name
        /// </summary>
        private readonly string fromDisplayName = string.Empty;

        /// <summary>
        /// Logging
        /// </summary>
        ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="messageTemplateDataProvider">The email template data provider.</param>
        /// <param name="adminDataProvider">The admin data provider.</param>
        /// <param name="securityQuestionDataProvider">The security question data provider.</param>
        public ForgotPasswordDataProvider(
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IMessageTemplateDataProvider messageTemplateDataProvider,
            IAdminDataProvider adminDataProvider,
            ISecurityQuestionDataProvider securityQuestionDataProvider,
            ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.messageTemplateDataProvider = messageTemplateDataProvider;
            this.adminDataProvider = adminDataProvider;
            this.securityQuestionDataProvider = securityQuestionDataProvider;
            this.fromEmail = ConfigurationManager.AppSettings["FromEmail"];
            this.fromDisplayName = ConfigurationManager.AppSettings["FromDisplayName"];
            this.logger = logger;
        }

        /// <summary>
        /// Sends the reset link.usp_ValidateServerIPAddress
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="requestorIPAddress">The requestor ip address.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> SendResetLink(string email, string requestorIPAddress)
        {
            //Get email template for reset password
            var messageTemplate = messageTemplateDataProvider.GetMessageTemplate((int)TemplateType.ResetPasswordInitiation);
            var response = new Response<ResetPasswordModel>();

            var users = adminDataProvider.GetUserByEmail(email);
            if (users != null && users.DataItems != null && users.DataItems.Count > 0)
            {
                if (users.DataItems[0].IsActive == false)
                {
                    response.ResultCode = 1;
                    response.ResultMessage = "Unable to reset password because user is inactive";
                }
                else
                    if (users.DataItems[0].ADFlag == false) // ADFlag cannot be null
                {
                    var resetIdentifier = Guid.NewGuid();
                    var user = users.DataItems.FirstOrDefault();
                    if (user != null)
                    {
                        var createLinkResponse = CreateResetPasswordLink(user.UserID, requestorIPAddress, resetIdentifier);
                        if (createLinkResponse.ResultCode == 0)
                        {
                            var emailMessage = PrepareResetPasswordEmailMessage(user, messageTemplate.DataItems.FirstOrDefault(), email, resetIdentifier);
                            try
                            {
                                string error = string.Empty;
                                if (emailService.Send(emailMessage, out error) == false)
                                {
                                    logger.Error(new Exception(error));
                                    response.ResultCode = 1;
                                    response.ResultMessage = error;
                                }
                                return response;
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex);
                                response.ResultCode = 1;
                                response.ResultMessage = ex.Message;
                            }
                        }
                        else
                        {
                            response.ResultCode = 1;
                            response.ResultMessage = "Unable to create reset link.";
                        }
                    }
                }
                else
                {
                    response.DataItems = new List<ResetPasswordModel>{
                        new ResetPasswordModel {ADFlag = users.DataItems[0].ADFlag, ADUserPasswordResetMessage = users.DataItems[0].ADUserPasswordResetMessage}
                    };
                }
            }
            else
            {
                response.ResultCode = 1;
                response.ResultMessage = "Email is not defined in the axis system.";
            }
            return response;
        }

        /// <summary>
        /// Verifies the reset identifier.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        public Response<ResetPasswordModel> VerifyResetIdentifier(ResetPasswordModel resetPassword)
        {
            var resetPasswordRepository = unitOfWork.GetRepository<ResetPasswordModel>();

            SqlParameter resetLinkParam = new SqlParameter("ResetIdentifier", resetPassword.ResetIdentifier);
            SqlParameter requestorIPAddressParam = new SqlParameter("RequestorIPAddress", resetPassword.RequestorIPAddress);

            List<SqlParameter> procParams = new List<SqlParameter>() { resetLinkParam, requestorIPAddressParam };

            var resetIdentifierInfo = resetPasswordRepository.ExecuteStoredProc("usp_VerifyResetIdentifier", procParams);
            return resetIdentifierInfo;
        }

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            return adminDataProvider.VerifySecurityDetails(resetPassword);
        }

        /// <summary>
        /// Verifies the otp.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ResetPasswordModel> VerifyOTP(ResetPasswordModel resetPassword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ResetPasswordModel> ResetPassword(ResetPasswordModel resetPassword)
        {
            var resetPasswordRepository = unitOfWork.GetRepository<ResetPasswordModel>();

            SqlParameter newPasswordParam = new SqlParameter("NewPassword", resetPassword.NewPassword);
            SqlParameter resetLinkParam = new SqlParameter("ResetIdentifier", resetPassword.ResetIdentifier);
            SqlParameter resetuseridParam = new SqlParameter("ResetUserId", resetPassword.UserID);
            SqlParameter requestorIPAddressParam = new SqlParameter("RequestorIPAddress", resetPassword.RequestorIPAddress);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", resetPassword.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { newPasswordParam, resetLinkParam, resetuseridParam, requestorIPAddressParam, modifiedOnParam };

            var resetPasswordInfo = resetPasswordRepository.ExecuteStoredProc("usp_ResetPassword", procParams);

            if (resetPasswordInfo.ResultCode == 0)
            {
                //Get message template for reset password confirmation
                var messageTemplate = messageTemplateDataProvider.GetMessageTemplate((int)TemplateType.ResetPasswordConfirmation);
                var response = new Response<ResetPasswordModel>();

                var users = adminDataProvider.GetUserByEmail(resetPassword.Email);
                if (users != null && users.DataItems != null && users.DataItems.Count > 0)
                {
                    var emailMessage = PrepareConfirmedResetPasswordEmailMessage(users.DataItems.FirstOrDefault(), messageTemplate.DataItems.FirstOrDefault(), resetPassword.Email);
                    try
                    {
                        string error = string.Empty;
                        if (emailService.Send(emailMessage, out error) == false)
                        {
                            logger.Error(new Exception(error));
                            response.ResultCode = 1;
                            response.ResultMessage = error;
                        }
                        return response;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        response.ResultCode = 1;
                        response.ResultMessage = ex.Message;
                    }
                }
                else if (resetPassword.UserID > 0)
                {
                    return response;
                }
                else
                {
                    response.ResultCode = 1;
                    response.ResultMessage = "Email does not exists";
                }
                return response;
            }
            return resetPasswordInfo;
        }

        /// <summary>
        /// Creates the reset password link.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="requestorIPAddress">The requestor ip address.</param>
        /// <param name="resetIdentifier">The reset identifier.</param>
        /// <returns></returns>
        private Response<ResetPasswordModel> CreateResetPasswordLink(int userID, string requestorIPAddress, Guid resetIdentifier)
        {
            var resetPasswordRepository = unitOfWork.GetRepository<ResetPasswordModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            SqlParameter resetLinkParam = new SqlParameter("ResetIdentifier", resetIdentifier);
            SqlParameter requestorIPAddressParam = new SqlParameter("RequestorIPAddress", requestorIPAddress ?? (object)DBNull.Value);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, resetLinkParam, requestorIPAddressParam, modifiedOnParam };

            var resetPasswordInfo = resetPasswordRepository.ExecuteNQStoredProc("usp_AddResetPasswordLink", procParams);
            return resetPasswordInfo;
        }

        /// <summary>
        /// Prepares the reset password email message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="MessageTemplateModel">The email template model.</param>
        /// <param name="email">The email.</param>
        /// <param name="resetIdentifier">The reset identifier.</param>
        /// <returns></returns>
        private EmailMessageModel PrepareResetPasswordEmailMessage(UserModel user, MessageTemplateModel MessageTemplateModel, string email, Guid resetIdentifier)
        {
            var emailMessage = new EmailMessageModel();
            var retrievedSettings = GetFromEmailSettings();

            var webAppUrl = retrievedSettings.FirstOrDefault(f => f.SettingsID == (int)Setting.WebServerUrl).Value;

            emailMessage.Subject = MessageTemplateModel.EmailSubject;
            emailMessage.Body = string.Format(MessageTemplateModel.MessageBody, user.FirstName, webAppUrl, resetIdentifier);
            emailMessage.IsBodyHtml = MessageTemplateModel.IsHtmlBody;

            emailMessage.To.Add(new EmailAddressModel() { EmailAddress = email, DisplayName = string.Format("{0} {1}", user.FirstName, user.LastName) });

            emailMessage.From.EmailAddress = retrievedSettings.FirstOrDefault(f => f.SettingsID == (int)Setting.FromEmail).Value;
            emailMessage.From.DisplayName = retrievedSettings.FirstOrDefault(d => d.SettingsID == (int)Setting.FromDisplayName).Value;
            return emailMessage;
        }

        /// <summary>
        /// Prepares the confirmed reset password email message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="MessageTemplateModel">The email template model.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        private EmailMessageModel PrepareConfirmedResetPasswordEmailMessage(UserModel user, MessageTemplateModel MessageTemplateModel, string email)
        {
            var emailMessage = new EmailMessageModel();

            emailMessage.Subject = MessageTemplateModel.EmailSubject;
            emailMessage.Body = string.Format(MessageTemplateModel.MessageBody, user.FirstName);
            emailMessage.IsBodyHtml = MessageTemplateModel.IsHtmlBody;

            emailMessage.To.Add(new EmailAddressModel() { EmailAddress = email, DisplayName = string.Format("{0} {1}", user.FirstName, user.LastName) });

            var retrievedSettings = GetFromEmailSettings();
            emailMessage.From.EmailAddress = retrievedSettings.FirstOrDefault(f => f.SettingsID == (int)Setting.FromEmail).Value;
            emailMessage.From.DisplayName = retrievedSettings.FirstOrDefault(d => d.SettingsID == (int)Setting.FromDisplayName).Value;
            return emailMessage;
        }

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        public Response<SecurityQuestionModel> GetSecurityQuestions()
        {
            var securityQuestionResult = securityQuestionDataProvider.GetSecurityQuestions();

            return securityQuestionResult;
        }

        #region Private Methods

        private List<SettingModel> GetFromEmailSettings()
        {
            List<Setting> neededSettings = new List<Setting>() { Setting.FromDisplayName, Setting.FromEmail, Setting.WebServerUrl };
            SettingsCacheManager cacheMgr = new SettingsCacheManager();
            var retrievedSettings = cacheMgr.GetAppSettingsByEnums(neededSettings);

            return retrievedSettings;
        }

        #endregion
    }
}
