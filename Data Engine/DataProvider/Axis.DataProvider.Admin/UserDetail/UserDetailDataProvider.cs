using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.MessageTemplate;
using Axis.Helpers.Caching;
using Axis.Logging;
using Axis.Model.Account;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Model.Common.MessageTemplate;
using Axis.Model.Setting;
using Axis.NotificationService.Email;
using Axis.NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;

namespace Axis.DataProvider.Admin
{
    public class UserDetailDataProvider : IUserDetailDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService = null;
        private readonly IMessageTemplateDataProvider _messageTemplateDataProvider = null;
        private readonly ILogger _logger = null;
        #endregion Class Variables

        #region Constructors

        public UserDetailDataProvider(IUnitOfWork unitOfWork, IEmailService emailService, IMessageTemplateDataProvider messageTemplateDataProvider, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _messageTemplateDataProvider = messageTemplateDataProvider;
            _logger = logger;
        }

        #endregion Constructors

        #region Public Methods

        public Response<UserModel> GetUser(int userID)
        {
            var userDetailRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = userDetailRepository.ExecuteStoredProc("usp_GetUserByID", procParams);

            return result;
        }

        public Response<UserModel> AddUser(UserModel userDetail)
        {
            var userDetailRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter userNameParam = new SqlParameter("UserName", userDetail.UserName);
            SqlParameter firstNameParam = new SqlParameter("FirstName", userDetail.FirstName);
            SqlParameter lastNameParam = new SqlParameter("LastName", userDetail.LastName);
            SqlParameter middleNameParam = new SqlParameter("MiddleName", (object)userDetail.MiddleName ?? DBNull.Value);
            SqlParameter genderParam = new SqlParameter("GenderID", (object)userDetail.GenderID ?? DBNull.Value);
            SqlParameter passwordParam = new SqlParameter("Password", userDetail.Password);
            SqlParameter isActiveParam = new SqlParameter("IsActive", userDetail.IsActive ?? false);
            SqlParameter effectiveFromDateParam = userDetail.EffectiveFromDate != null ? new SqlParameter("EffectiveFromDate", userDetail.EffectiveFromDate) : new SqlParameter("EffectiveFromDate", DBNull.Value);
            SqlParameter effectiveToDateParam = userDetail.EffectiveToDate != null ? new SqlParameter("EffectiveToDate", userDetail.EffectiveToDate) : new SqlParameter("EffectiveToDate", DBNull.Value);
            SqlParameter primaryEmailParam = new SqlParameter("PrimaryEmail", userDetail.PrimaryEmail);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userNameParam, firstNameParam, lastNameParam, middleNameParam, genderParam, passwordParam, isActiveParam, effectiveFromDateParam, effectiveToDateParam, primaryEmailParam, modifiedOnParam };
            //var result = userDetailRepository.ExecuteNQStoredProc("usp_AddUser", procParams, true);
            var result = _unitOfWork.EnsureInTransaction(userDetailRepository.ExecuteNQStoredProc, "usp_AddUser", procParams,
                forceRollback: userDetail.ForceRollback.GetValueOrDefault(false), adonResult: true);

            return result;
        }

        public Response<UserModel> UpdateUser(UserModel userDetail)
        {
            var userDetailRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userDetail.UserID);
            SqlParameter userNameParam = new SqlParameter("UserName", userDetail.UserName);
            SqlParameter firstNameParam = new SqlParameter("FirstName", userDetail.FirstName);
            SqlParameter lastNameParam = new SqlParameter("LastName", userDetail.LastName);
            SqlParameter middleNameParam = new SqlParameter("MiddleName", (object)userDetail.MiddleName ?? DBNull.Value);
            SqlParameter genderParam = new SqlParameter("GenderID", (object)userDetail.GenderID ?? DBNull.Value);
            SqlParameter isActiveParam = new SqlParameter("IsActive", userDetail.IsActive ?? false);
            SqlParameter effectiveFromDateParam = userDetail.EffectiveFromDate != null ? new SqlParameter("EffectiveFromDate", userDetail.EffectiveFromDate) : new SqlParameter("EffectiveFromDate", DBNull.Value);
            SqlParameter effectiveToDateParam = userDetail.EffectiveToDate != null ? new SqlParameter("EffectiveToDate", userDetail.EffectiveToDate) : new SqlParameter("EffectiveToDate", DBNull.Value);
            SqlParameter resetLoginAttemptsParam = new SqlParameter("ResetLoginAttempts", false);
            SqlParameter primaryEmailParam = new SqlParameter("PrimaryEmail", userDetail.PrimaryEmail);
            SqlParameter emailIdParam = new SqlParameter("EmailID", userDetail.EmailID);
            SqlParameter isInternalParam = new SqlParameter("IsInternal", (object)userDetail.IsInternal ?? DBNull.Value);
            SqlParameter userGUIDParam = new SqlParameter("UserGUID", (object)userDetail.UserGUID ?? DBNull.Value);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, userNameParam, firstNameParam, lastNameParam, middleNameParam, genderParam, isActiveParam, effectiveFromDateParam, effectiveToDateParam, resetLoginAttemptsParam, primaryEmailParam, emailIdParam, isInternalParam, userGUIDParam, modifiedOnParam };
            //var result = userDetailRepository.ExecuteNQStoredProc("usp_UpdateUser", procParams);
            var result = _unitOfWork.EnsureInTransaction(userDetailRepository.ExecuteNQStoredProc, "usp_UpdateUser", procParams,
                forceRollback: userDetail.ForceRollback.GetValueOrDefault(false));

            return result;
        }

        public Response<UserModel> SendNewUserEmail(UserModel userDetail)
        {
            //Get email template for reset password
            var messageTemplate = _messageTemplateDataProvider.GetMessageTemplate((int)TemplateType.NewUserNotification);
            var emailMessage = PrepareNewUserEmailMessage(messageTemplate.DataItems.FirstOrDefault(), userDetail);
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

        public Response<CoSignaturesModel> GetCoSignatures(int userID)
        {
            var repo = _unitOfWork.GetRepository<CoSignaturesBaseModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var res = repo.ExecuteStoredProc("usp_GetCoSignatures", procParams);
            return FillCoSignatures(res);
        }

        private Response<CoSignaturesModel> FillCoSignatures(Response<CoSignaturesBaseModel> res)
        {
            Response<CoSignaturesModel> result = new Response<CoSignaturesModel>();
            result.ResultCode = res.ResultCode;
            result.ResultMessage = res.ResultMessage;
            result.ID = res.ID;
            result.AdditionalResult = res.AdditionalResult;
            result.DataItems = new List<CoSignaturesModel>();
            CoSignaturesModel model = new CoSignaturesModel();
            model.CoSignatures = res.DataItems;
            result.DataItems.Add(model);
            return result;
        }

        public Response<CoSignaturesModel> AddCoSignatures(CoSignaturesModel signature)
        {
            var repo = _unitOfWork.GetRepository<CoSignaturesModel>(SchemaName.Core);
            SqlParameter XMLParam = new SqlParameter("CoSignaturesXML", GenerateRequestXml(signature));
            XMLParam.DbType = System.Data.DbType.Xml;
            var modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { XMLParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<CoSignaturesModel>>(repo.ExecuteNQStoredProc, "usp_AddCoSignatures", procParams, adonResult: false);
            result.AdditionalResult = (result.ResultCode == 0) ? "All Co-Signatures added successfully." : "Error adding Co-Signatures.";
            return result;
        }

        public Response<CoSignaturesModel> UpdateCoSignatures(Model.Admin.CoSignaturesModel signature)
        {
            var repo = _unitOfWork.GetRepository<CoSignaturesModel>(SchemaName.Core);
            SqlParameter XMLParam = new SqlParameter("CoSignaturesXML", GenerateRequestXml(signature, true));
            XMLParam.DbType = System.Data.DbType.Xml;
            var modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { XMLParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<CoSignaturesModel>>(repo.ExecuteNQStoredProc, "usp_UpdateCoSignatures", procParams, adonResult: false);
            result.AdditionalResult = (result.ResultCode == 0) ? "All Co-Signatures updated successfully." : "Error updating Co-Signatures.";
            return result;
        }

        public Response<CoSignaturesModel> DeleteCoSignatures(long id, DateTime modifiedOn)
        {
            var repository = _unitOfWork.GetRepository<CoSignaturesModel>(SchemaName.Core);
            SqlParameter param = new SqlParameter("CoSignatureID", id);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { param, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<CoSignaturesModel>>(repository.ExecuteNQStoredProc, "usp_DeleteCoSignatures", procParams);
            return result;
        }

        public Response<UserIdentifierDetailsModel> GetUserIdentifierDetails(int userID)
        {
            var repo = _unitOfWork.GetRepository<UserIdentifierDetailsBaseModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = repo.ExecuteStoredProc("usp_GetUserIdentifierDetails", procParams);
            return FillUserIdentifiers(result);
        }

        private Response<UserIdentifierDetailsModel> FillUserIdentifiers(Response<UserIdentifierDetailsBaseModel> res)
        {
            Response<UserIdentifierDetailsModel> result = new Response<UserIdentifierDetailsModel>();
            result.ResultCode = res.ResultCode;
            result.ResultMessage = res.ResultMessage;
            result.ID = res.ID;
            result.AdditionalResult = res.AdditionalResult;
            result.DataItems = new List<UserIdentifierDetailsModel>();
            UserIdentifierDetailsModel model = new UserIdentifierDetailsModel();
            model.UserDetails = res.DataItems;
            result.DataItems.Add(model);
            return result;
        }

        public Response<UserIdentifierDetailsModel> AddUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            var repo = _unitOfWork.GetRepository<UserIdentifierDetailsModel>(SchemaName.Core);
            SqlParameter XMLParam = new SqlParameter("UserIdentifierXML", GenerateRequestXml(useridentifier));
            XMLParam.DbType = System.Data.DbType.Xml;
            var modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { XMLParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<UserIdentifierDetailsModel>>(repo.ExecuteNQStoredProc, "usp_AddUserIdentifierDetails", procParams, adonResult: false);
            result.AdditionalResult = (result.ResultCode == 0) ? "All Employee Information added successfully." : "Error adding Employee Information.";
            return result;
        }

        public Response<UserIdentifierDetailsModel> UpdateUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            var repo = _unitOfWork.GetRepository<UserIdentifierDetailsModel>(SchemaName.Core);
            SqlParameter XMLParam = new SqlParameter("UserIdentifierXML", GenerateRequestXml(useridentifier, true));
            XMLParam.DbType = System.Data.DbType.Xml;
            var modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { XMLParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<UserIdentifierDetailsModel>>(repo.ExecuteNQStoredProc, "usp_UpdateUserIdentiferDetails", procParams, adonResult: false);
            result.AdditionalResult = (result.ResultCode == 0) ? "All Employee Information updated successfully." : "Error updating Employee Information.";
            return result;
        }

        public Response<UserIdentifierDetailsModel> DeleteUserIdentifierDetails(long id, DateTime modifiedOn)
        {
            var repository = _unitOfWork.GetRepository<UserIdentifierDetailsModel>(SchemaName.Core);
            SqlParameter param = new SqlParameter("UserIdentifierDetailsID", id);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { param, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<UserIdentifierDetailsModel>>(repository.ExecuteNQStoredProc, "usp_DeleteUserIdentifierDetails", procParams);
            return result;
        }

        public Response<UserAdditionalDetailsModel> GetUserAdditionalDetails(int userID)
        {
            var repo = _unitOfWork.GetRepository<UserAdditionalDetailsBaseModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = repo.ExecuteStoredProc("usp_GetUserAdditionalDetails", procParams);
            return FillUserAdditionalDetails(result);
        }

        private Response<UserAdditionalDetailsModel> FillUserAdditionalDetails(Response<UserAdditionalDetailsBaseModel> res)
        {
            Response<UserAdditionalDetailsModel> result = new Response<UserAdditionalDetailsModel>();
            result.ResultCode = res.ResultCode;
            result.ResultMessage = res.ResultMessage;
            result.ID = res.ID;
            result.AdditionalResult = res.AdditionalResult;
            result.DataItems = new List<UserAdditionalDetailsModel>();
            UserAdditionalDetailsModel model = new UserAdditionalDetailsModel();
            model.UserDetails = res.DataItems;
            result.DataItems.Add(model);
            return result;
        }

        public Response<UserAdditionalDetailsModel> AddUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            var repo = _unitOfWork.GetRepository<UserAdditionalDetailsModel>(SchemaName.Core);
            SqlParameter XMLParam = new SqlParameter("UserAdditionalDetailsXML", GenerateRequestXml(details));
            XMLParam.DbType = System.Data.DbType.Xml;
            var modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { XMLParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<UserAdditionalDetailsModel>>(repo.ExecuteNQStoredProc, "usp_AddUserAdditionalDetails", procParams, adonResult: false);
            result.AdditionalResult = (result.ResultCode == 0) ? "All Contract Employee items added successfully." : "Error adding Contract Emoloyee items.";
            return result;
        }

        public Response<UserAdditionalDetailsModel> UpdateUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            var repo = _unitOfWork.GetRepository<UserAdditionalDetailsModel>(SchemaName.Core);
            SqlParameter XMLParam = new SqlParameter("UserAdditionalDetailsXML", GenerateRequestXml(details, true));
            XMLParam.DbType = System.Data.DbType.Xml;
            var modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { XMLParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<UserAdditionalDetailsModel>>(repo.ExecuteNQStoredProc, "usp_UpdateUserAdditionalDetails", procParams, adonResult: false);
            result.AdditionalResult = (result.ResultCode == 0) ? "All Contract Employee items updated successfully." : "Error updating Contract Emoloyee items.";
            return result;
        }

        public Response<UserAdditionalDetailsModel> DeleteUserAdditionalDetails(long id, DateTime modifiedOn)
        {
            var repository = _unitOfWork.GetRepository<UserAdditionalDetailsModel>(SchemaName.Core);
            SqlParameter param = new SqlParameter("UserAdditionalDetailsID", id);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { param, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<UserAdditionalDetailsModel>>(repository.ExecuteNQStoredProc, "usp_DeleteUserAdditionalDetails", procParams);
            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private EmailMessageModel PrepareNewUserEmailMessage(MessageTemplateModel messageTemplate, UserModel userDetail)
        {
            var emailMessage = new EmailMessageModel();
            var retrievedSettings = GetFromEmailSettings();
            var webServerUrl = retrievedSettings.FirstOrDefault(f => f.SettingsID == (int)Setting.WebServerUrl).Value;

            emailMessage.Subject = messageTemplate.EmailSubject;
            emailMessage.Body = String.Format(messageTemplate.MessageBody, userDetail.FirstName + " " + userDetail.LastName, userDetail.UserName, userDetail.Password, webServerUrl);
            emailMessage.IsBodyHtml = messageTemplate.IsHtmlBody;

            emailMessage.To.Add(new EmailAddressModel() { EmailAddress = userDetail.PrimaryEmail, DisplayName = userDetail.PrimaryEmail });

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

        private string GenerateRequestXml(CoSignaturesModel signature, bool isupdate = false)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("CoSignatures");

                    //WriteToXML(XWriter, divisionProgram.userID, divisionProgram.MappingID, divisionProgram.IsActive);

                    if (signature.CoSignatures != null)
                    {
                        foreach (CoSignaturesBaseModel sigbase in signature.CoSignatures)
                        {
                            WriteToXML(XWriter, sigbase, isupdate);
                        }
                    }
                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        private string GenerateRequestXml(UserIdentifierDetailsModel details, bool isupdate = false)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("UserIdentifiers");
                    //WriteToXML(XWriter, divisionProgram.userID, divisionProgram.MappingID, divisionProgram.IsActive);
                    if (details.UserDetails != null)
                    {
                        foreach (UserIdentifierDetailsBaseModel basemodel in details.UserDetails)
                        {
                            WriteToXML(XWriter, basemodel, isupdate);
                        }
                    }
                    XWriter.WriteEndElement();
                }
                return sWriter.ToString();
            }
        }

        private string GenerateRequestXml(UserAdditionalDetailsModel details, bool isupdate = false)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("UserAdditionalDetails");
                    //WriteToXML(XWriter, divisionProgram.userID, divisionProgram.MappingID, divisionProgram.IsActive);
                    if (details.UserDetails != null)
                    {
                        foreach (UserAdditionalDetailsBaseModel basemodel in details.UserDetails)
                        {
                            WriteToXML(XWriter, basemodel, isupdate);
                        }
                    }
                    XWriter.WriteEndElement();
                }
                return sWriter.ToString();
            }
        }

        public void WriteToXML(XmlWriter XWriter, CoSignaturesBaseModel sigbase, bool isupdate = false)
        {
            XWriter.WriteStartElement("CoSignatures");
            if (isupdate)
                XWriter.WriteElementString("CoSignatureID", sigbase.CoSignatureID.ToString());
            XWriter.WriteElementString("UserID", sigbase.UserID.ToString());
            XWriter.WriteElementString("CoSigneeID", sigbase.CoSigneeID.ToString());
            XWriter.WriteElementString("DocumentTypeGroupID", sigbase.DocumentTypeGroupID.ToString());
            if (sigbase.EffectiveDate.HasValue)
                XWriter.WriteElementString("EffectiveDate", sigbase.EffectiveDate.Value.ToString());
            if (sigbase.ExpirationDate.HasValue)
                XWriter.WriteElementString("ExpirationDate", sigbase.ExpirationDate.Value.ToString());
            XWriter.WriteElementString("IsActive", true.ToString());
            //XWriter.WriteElementString("ModifiedBy", sigbase.UserID.ToString());
            //XWriter.WriteElementString("ModifiedOn", DateTime.Now.ToString());
            XWriter.WriteEndElement();
        }

        public void WriteToXML(XmlWriter XWriter, UserAdditionalDetailsBaseModel details, bool isupdate = false)
        {
            XWriter.WriteStartElement("UserAdditionalDetails");
            if (isupdate)
                XWriter.WriteElementString("UserAdditionalDetailID", details.UserAdditionalDetailID.ToString());
            XWriter.WriteElementString("UserID", details.UserID.ToString());
            XWriter.WriteElementString("ContractingEntity", details.ContractingEntity);
            XWriter.WriteElementString("IDNumber", details.IDNumber);
            if (details.EffectiveDate.HasValue)
                XWriter.WriteElementString("EffectiveDate", details.EffectiveDate.Value.ToString());
            if (details.ExpirationDate.HasValue)
                XWriter.WriteElementString("ExpirationDate", details.ExpirationDate.Value.ToString());
            XWriter.WriteElementString("IsActive", true.ToString());
            //XWriter.WriteElementString("ModifiedBy", details.UserID.ToString());
            //XWriter.WriteElementString("ModifiedOn", DateTime.Now.ToString());
            XWriter.WriteEndElement();
        }

        public void WriteToXML(XmlWriter XWriter, UserIdentifierDetailsBaseModel details, bool isupdate = false)
        {
            XWriter.WriteStartElement("UserIdentifiers");
            if (isupdate)
                XWriter.WriteElementString("UserIdentifierDetailsID", details.UserIdentifierDetailsID.ToString());
            XWriter.WriteElementString("UserID", details.UserID.ToString());
            XWriter.WriteElementString("UserIdentifierTypeID", details.UserIdentifierTypeID.ToString());
            XWriter.WriteElementString("IDNumber", details.IDNumber);
            if (details.EffectiveDate.HasValue)
                XWriter.WriteElementString("EffectiveDate", details.EffectiveDate.Value.ToString());
            if (details.ExpirationDate.HasValue)
                XWriter.WriteElementString("ExpirationDate", details.ExpirationDate.Value.ToString());
            XWriter.WriteElementString("IsActive", true.ToString());
            //XWriter.WriteElementString("ModifiedBy", details.UserID.ToString());
            //XWriter.WriteElementString("ModifiedOn", DateTime.Now.ToString());
            XWriter.WriteEndElement();
        }

        #endregion Private Methods
    }
}
