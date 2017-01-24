using System;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Helpers.Caching;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Model.Setting;
using Axis.NotificationService.Email;
using Axis.NotificationService.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using Axis.DataProvider.MessageTemplate;
using Axis.Model.Common.MessageTemplate;
using Axis.Logging;

namespace Axis.DataProvider.Admin
{
    public class AdminDataProvider : IAdminDataProvider
    {
        #region Class Variables

        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// The email service
        /// </summary>
        private readonly IEmailService _emailService = null;
        
        /// <summary>
        /// The email template data provider
        /// </summary>
        private readonly IMessageTemplateDataProvider _messageTemplateDataProvider = null;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger=null;

        #endregion Class Variables

        #region Constructors

        public AdminDataProvider(IUnitOfWork unitOfWork, IEmailService emailService, IMessageTemplateDataProvider messageTemplateDataProvider, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            _emailService = emailService;
            _messageTemplateDataProvider = messageTemplateDataProvider;
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public Response<UserModel> GetUsers(string userSch)
        {
            var userRepository = unitOfWork.GetRepository<UserModel>();

            SqlParameter userSchParam = new SqlParameter("UserSch", string.IsNullOrEmpty(userSch) ? string.Empty : userSch);
            List<SqlParameter> procParams = new List<SqlParameter>() { userSchParam };
            var result = userRepository.ExecuteStoredProc("usp_GetUsers", procParams);

            return result;
        }

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public Response<UserModel> GetUserByEmail(string email)
        {
            var userRepository = unitOfWork.GetRepository<UserModel>();

            SqlParameter emailParam = new SqlParameter("Email", string.IsNullOrEmpty(email) ? string.Empty : email);
            List<SqlParameter> procParams = new List<SqlParameter>() { emailParam };

            var result = userRepository.ExecuteStoredProc("usp_GetUserByEmail", procParams);

            return result;
        }

        public Response<UserModel> AddUser(UserModel user)
        {
            var userRepository = unitOfWork.GetRepository<UserModel>();
            SqlParameter userNameParam = new SqlParameter("UserName", user.UserName);
            SqlParameter firstNameParam = new SqlParameter("FirstName", user.FirstName);
            SqlParameter lastNameParam = new SqlParameter("LastName", user.LastName);
            SqlParameter passwordParam = new SqlParameter("Password", user.Password);
            SqlParameter isActiveParam = new SqlParameter("IsActive", user.IsActive ?? false);
            SqlParameter effectiveToDateParam = user.EffectiveToDate != null ? new SqlParameter("EffectiveToDate", user.EffectiveToDate) : new SqlParameter("EffectiveToDate", DBNull.Value);
            SqlParameter rolesParam = new SqlParameter("RolesXMLValue", GenerateRoleXML(user.Roles));
            SqlParameter credentialsParam = new SqlParameter("CredentialsXMLValue", GenerateUserCredentialsXML(user.Credentials));
            rolesParam.DbType = System.Data.DbType.Xml;
            credentialsParam.DbType = System.Data.DbType.Xml;
            SqlParameter primaryEmailParam = new SqlParameter("PrimaryEmail", user.PrimaryEmail);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", user.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userNameParam, firstNameParam, lastNameParam, passwordParam, isActiveParam, effectiveToDateParam, rolesParam, credentialsParam, primaryEmailParam, modifiedOnParam };
            return unitOfWork.EnsureInTransaction<Response<UserModel>>(userRepository.ExecuteNQStoredProc, "usp_AddUser", procParams, forceRollback: user.ForceRollback.GetValueOrDefault(false), adonResult: true);
        }

        public Response<UserModel> UpdateUser(UserModel user)
        {
            var userRepository = unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", user.UserID);
            SqlParameter userNameParam = new SqlParameter("UserName", user.UserName);
            SqlParameter firstNameParam = new SqlParameter("FirstName", user.FirstName);
            SqlParameter lastNameParam = new SqlParameter("LastName", user.LastName);
            SqlParameter isActiveParam = new SqlParameter("IsActive", user.IsActive ?? false);
            SqlParameter effectiveToDateParam = user.EffectiveToDate != null ? new SqlParameter("EffectiveToDate", user.EffectiveToDate) : new SqlParameter("EffectiveToDate", DBNull.Value);
            SqlParameter resetLoginAttemptsParam = new SqlParameter("ResetLoginAttempts", false);
            SqlParameter rolesParam = new SqlParameter("RolesXMLValue", GenerateRoleXML(user.Roles));
            SqlParameter credentialsParam = new SqlParameter("CredentialsXMLValue", GenerateUserCredentialsXML(user.Credentials));
            rolesParam.DbType = System.Data.DbType.Xml;
            credentialsParam.DbType = System.Data.DbType.Xml;
            SqlParameter primaryEmailParam = new SqlParameter("PrimaryEmail", user.PrimaryEmail);
            SqlParameter emailIdParam = new SqlParameter("EmailID", user.EmailID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", user.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, userNameParam, firstNameParam, lastNameParam,
                                                                       isActiveParam, effectiveToDateParam, resetLoginAttemptsParam, rolesParam, credentialsParam, primaryEmailParam, emailIdParam, modifiedOnParam };

            return unitOfWork.EnsureInTransaction<Response<UserModel>>(userRepository.ExecuteNQStoredProc, "usp_UpdateUser", procParams, forceRollback: user.ForceRollback.GetValueOrDefault(false));
        }

        public Response<UserModel> RemoveUser(int userID, DateTime modifiedOn)
        {
            var userRepository = unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, modifiedOnParam };
            return unitOfWork.EnsureInTransaction<Response<UserModel>>(userRepository.ExecuteNQStoredProc, "usp_DeleteUser", procParams);
        }

        public Response<UserModel> ActivateUser(UserModel user)
        {
            var userRepository = unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", user.UserID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", user.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, modifiedOnParam };
            return unitOfWork.EnsureInTransaction<Response<UserModel>>(userRepository.ExecuteNQStoredProc, "usp_UpdateUserStatus", procParams, forceRollback: user.ForceRollback.GetValueOrDefault(false));
        }

        public Response<UserModel> UnlockUser(UserModel user)
        {
            var userRepository = unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", user.UserID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", user.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, modifiedOnParam };
            return unitOfWork.EnsureInTransaction<Response<UserModel>>(userRepository.ExecuteNQStoredProc, "usp_UpdateUserUnlock", procParams, forceRollback: user.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Sends an email to the new user
        /// </summary>
        /// <param name="user">The new user model</param>
        /// <returns></returns>
        public Response<UserModel> SendNewUserEmail(UserModel user)
        {
            //Get email template for reset password
            var messageTemplate = _messageTemplateDataProvider.GetMessageTemplate((int)TemplateType.NewUserNotification);
            var emailMessage = PrepareNewUserEmailMessage(messageTemplate.DataItems.FirstOrDefault(), user);
            var response = new Response<UserModel>();

            try
            {
                string error = string.Empty;
                if (_emailService.Send(emailMessage, out error) == false)
                {
                    _logger.Error(new Exception(error));
                    response.ResultCode = 1;
                    response.ResultMessage = error;
                }
                return response;
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
                response.ResultCode = 1;
                response.ResultMessage = exc.Message;
            }

            return response;
        }

        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            var userRepository = unitOfWork.GetRepository<UserRoleModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };

            return userRepository.ExecuteStoredProc("usp_GetUserRoles", procParams);
        }

        public Response<UserCredentialModel> GetUserCredentials(int userID)
        {
            var userRepository = unitOfWork.GetRepository<UserCredentialModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };

            return userRepository.ExecuteStoredProc("usp_GetUserCredentials", procParams);
        }

        public Response<UserModel> GetUsersByCredentialId(long credentialID)
        {
            var userRepository = unitOfWork.GetRepository<UserModel>();

            SqlParameter userIDParam = new SqlParameter("CredentialID", credentialID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };

            return userRepository.ExecuteStoredProc("usp_GetCredentialsUser", procParams);
        }

        public Response<UserCredentialModel> AddUserCredential(UserCredentialModel userCredential)
        {
            var userRepository = unitOfWork.GetRepository<UserCredentialModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userCredential.UserID);
            SqlParameter credentialIDParam = new SqlParameter("CredentialID", userCredential.CredentialID);
            SqlParameter credentialCodeParam = userCredential.LicenseNbr != null ? new SqlParameter("LicenseNbr", userCredential.LicenseNbr) : new SqlParameter("LicenseNbr", DBNull.Value);
            SqlParameter effectiveDateParam = userCredential.LicenseIssueDate != null ? new SqlParameter("LicenseIssueDate", userCredential.LicenseIssueDate) : new SqlParameter("LicenseIssueDate", DBNull.Value);
            SqlParameter expirationDateParam = userCredential.LicenseExpirationDate != null ? new SqlParameter("LicenseExpirationDate", userCredential.LicenseExpirationDate) : new SqlParameter("LicenseExpirationDate", DBNull.Value);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", userCredential.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, credentialIDParam, credentialCodeParam, effectiveDateParam, expirationDateParam, modifiedOnParam };

            return userRepository.ExecuteNQStoredProc("usp_AddUserCredential", procParams);
        }

        public Response<UserCredentialModel> UpdateUserCredential(UserCredentialModel userCredential)
        {
            var userRepository = unitOfWork.GetRepository<UserCredentialModel>();
            SqlParameter userCredentialIDParam = new SqlParameter("UserCredentialID", userCredential.UserCredentialID);
            SqlParameter userIDParam = new SqlParameter("UserID", userCredential.UserID);
            SqlParameter credentialIDParam = new SqlParameter("CredentialID", userCredential.CredentialID);
            SqlParameter credentialCodeParam = userCredential.LicenseNbr != null ? new SqlParameter("LicenseNbr", userCredential.LicenseNbr) : new SqlParameter("LicenseNbr", DBNull.Value);
            SqlParameter effectiveDateParam = userCredential.LicenseIssueDate != null ? new SqlParameter("LicenseIssueDate", userCredential.LicenseIssueDate) : new SqlParameter("LicenseIssueDate", DBNull.Value);
            SqlParameter expirationDateParam = userCredential.LicenseExpirationDate != null ? new SqlParameter("LicenseExpirationDate", userCredential.LicenseExpirationDate) : new SqlParameter("LicenseExpirationDate", DBNull.Value);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", userCredential.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userCredentialIDParam, userIDParam, credentialIDParam, credentialCodeParam, 
                effectiveDateParam, expirationDateParam , modifiedOnParam};

            return userRepository.ExecuteNQStoredProc("usp_UpdateUserCredential", procParams);
        }

        public Response<UserCredentialModel> DeleteUserCredential(int userCredentialID, DateTime modifiedOn)
        {
            var userRepository = unitOfWork.GetRepository<UserCredentialModel>();
            SqlParameter userCredentialIDParam = new SqlParameter("UserCredentialID", userCredentialID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { userCredentialIDParam, modifiedOnParam };
            return userRepository.ExecuteNQStoredProc("usp_DeleteUserCredential", procParams);
        }

        #endregion Methods

        #region Private Methods

        private string GenerateRoleXML(List<RoleModel> roles)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("RolesXMLValue");

                    if (roles != null)
                    {
                        foreach (RoleModel role in roles)
                        {
                            XWriter.WriteElementString("RoleID", role.RoleID.ToString());
                        }
                    }

                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        private string GenerateUserCredentialsXML(List<UserCredentialModel> userCredentials)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (var XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("CredentialsXMLValue");

                    if (userCredentials != null)
                    {
                        foreach (var credential in userCredentials)
                        {
                            XWriter.WriteStartElement("Credential");

                            XWriter.WriteElementString("CredentialID", credential.CredentialID.ToString());
                            XWriter.WriteElementString("LicenseNbr", credential.LicenseNbr);
                            XWriter.WriteElementString("EffectiveDate", credential.LicenseIssueDate.ToString());
                            XWriter.WriteElementString("ExpirationDate", credential.LicenseExpirationDate.ToString());

                            XWriter.WriteEndElement();
                        }
                    }

                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        private EmailMessageModel PrepareNewUserEmailMessage(MessageTemplateModel messageTemplate, UserModel user)
        {
            var emailMessage = new EmailMessageModel();
            var retrievedSettings = GetFromEmailSettings();
            var webServerUrl = retrievedSettings.FirstOrDefault(f => f.SettingsID == (int)Setting.WebServerUrl).Value;

            emailMessage.Subject = messageTemplate.EmailSubject;
            emailMessage.Body = String.Format(messageTemplate.MessageBody, user.FirstName + " " + user.LastName, user.UserName, user.Password, webServerUrl);
            emailMessage.IsBodyHtml = messageTemplate.IsHtmlBody;

            emailMessage.To.Add(new EmailAddressModel() { EmailAddress = user.PrimaryEmail, DisplayName = user.PrimaryEmail });

            emailMessage.From.EmailAddress = retrievedSettings.FirstOrDefault(f => f.SettingsID == (int)Setting.FromEmail).Value;
            emailMessage.From.DisplayName = retrievedSettings.FirstOrDefault(d => d.SettingsID == (int)Setting.FromDisplayName).Value;

            return emailMessage;
        }

        private List<SettingModel> GetFromEmailSettings()
        {
            List<Setting> neededSettings = new List<Setting>() { Setting.FromDisplayName, Setting.FromEmail, Setting.WebServerUrl };
            SettingsCacheManager cacheMgr = new SettingsCacheManager();
            var retrievedSettings = cacheMgr.GetAppSettingsByEnums(neededSettings);

            return retrievedSettings;
        }

        public Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            var resetPasswordRepository = unitOfWork.GetRepository<ResetPasswordModel>();

            if (resetPassword.ResetIdentifier == Guid.Empty && resetPassword.IsDigitalPassword == true)
            {
                SqlParameter useridParam = new SqlParameter("UserID", resetPassword.UserID);
                SqlParameter securityQuestionIDParam = new SqlParameter("SecurityQuestionID", resetPassword.SecurityQuestionID);
                SqlParameter securityAnswerParam = new SqlParameter("SecurityAnswer", resetPassword.SecurityAnswer);

                List<SqlParameter> procParams = new List<SqlParameter>() { useridParam, securityQuestionIDParam, securityAnswerParam, };

                var securityDetailsInfo = resetPasswordRepository.ExecuteNQStoredProc("usp_VerifySecurityDetailsMyProfile", procParams, false, true);
                return securityDetailsInfo;
            }
            else
            {

                SqlParameter securityQuestionIDParam = new SqlParameter("SecurityQuestionID", resetPassword.SecurityQuestionID);
                SqlParameter securityAnswerParam = new SqlParameter("SecurityAnswer", resetPassword.SecurityAnswer);
                SqlParameter resetLinkParam = new SqlParameter("ResetIdentifier", resetPassword.ResetIdentifier);
                SqlParameter requestorIPAddressParam = new SqlParameter("RequestorIPAddress", resetPassword.RequestorIPAddress);

                List<SqlParameter> procParams = new List<SqlParameter>() { securityQuestionIDParam, securityAnswerParam, resetLinkParam, requestorIPAddressParam };

                var securityDetailsInfo = resetPasswordRepository.ExecuteStoredProc("usp_VerifySecurityDetails", procParams);
                return securityDetailsInfo;
            }
        }

        #endregion Private Methods
    }
}
