using System;
using Axis.Data.Repository;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Model.Common;
using Axis.Helpers;
using Axis.Model.Email;
using System.Xml.Linq;
using System.Linq;
using Axis.Data.Repository.Schema;

namespace Axis.DataProvider.Common
{
    public class EmailDataProvider : IEmailDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public EmailDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<EmailModel> GetEmail(long emailID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("EmailID", emailID) };
            var repository = _unitOfWork.GetRepository<EmailModel>();
            var results = repository.ExecuteStoredProc("usp_GetEmail", spParameters);

            return results;
        }

        public Response<EmailModel> AddEmail(EmailModel email)
        {
            var repository = _unitOfWork.GetRepository<EmailModel>();
            var requestXMLValueParam = new SqlParameter("EmailsXML", EmailToXML(new List<EmailModel> { email }));
            var spParameters = new List<SqlParameter>() { requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<EmailModel>>(repository.ExecuteNQStoredProc, "usp_AddEmail", spParameters, forceRollback: email.ForceRollback.GetValueOrDefault(false));
        }

        public string EmailToXML(List<EmailModel> emails)
        {
            var xEle = new XElement("RequestXMLValue",
                from email in emails
                select new XElement("Email",
                               new XElement("EmailID", email.EmailID),
                               new XElement("Email", email.Email),
                               new XElement("ModifiedOn", email.ModifiedOn ?? DateTime.Now)
                           ));

            return xEle.ToString();
        }

        public Response<EmailModel> UpdateEmail(EmailModel emails)
        {
            var xmlParams = emails.ToXml2();
            var requestXMLValueParam = new SqlParameter("RequestXMLValue", xmlParams);
            var spParameters = new List<SqlParameter>() { requestXMLValueParam };
            var repository = _unitOfWork.GetRepository<EmailModel>(SchemaName.Registration);
            return _unitOfWork.EnsureInTransaction<Response<EmailModel>>(repository.ExecuteNQStoredProc, "usp_UpdateEmail", spParameters, forceRollback: emails.ForceRollback.GetValueOrDefault(false));
        }

        #region UserEmail

        public Response<UserEmailModel> GetUserEmails(int userID)
        {
            var userEmailRepository = _unitOfWork.GetRepository<UserEmailModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var userEmailResult = userEmailRepository.ExecuteStoredProc("usp_GetUserEmails", procParams);

            return userEmailResult;
        }

        public Response<UserEmailModel> UpdateUserEmails(int userID, List<UserEmailModel> userEmails)
        {
            var userEmailRepository = _unitOfWork.GetRepository<UserEmailModel>();
            //We will need to add the ability to assign multiple emails to a user

            SqlParameter requestXmlValueParam = new SqlParameter("EmailXml", EmailToXml(userEmails));
            List<SqlParameter> procParams = new List<SqlParameter>() { requestXmlValueParam };
            var userEmailResult = _unitOfWork.EnsureInTransaction<Response<UserEmailModel>>(userEmailRepository.ExecuteNQStoredProc, "usp_UpdateUserEmails", procParams, forceRollback: userEmails.Any(x => x.ForceRollback.GetValueOrDefault(false)));

            return userEmailResult;        
        }

        private string EmailToXml(List<UserEmailModel> userEmails)
        {
            var emailXml = new XElement("RequestXMLValue",
                from email in userEmails
                select new XElement("Email",
                               new XElement("UserEmailID", email.UserEmailID),
                               new XElement("EmailID", email.EmailID),
                               new XElement("Email", email.Email),
                               new XElement("EmailPermissionID", email.EmailPermissionID),
                               new XElement("IsPrimary", email.IsPrimary),
                               new XElement("ModifiedOn", email.ModifiedOn ?? DateTime.Now)
                           ));

            return emailXml.ToString();
        }

        #endregion

        #endregion exposed functionality
    }

}
