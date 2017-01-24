using System;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Registration.Common;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Contact email data provider
    /// </summary>
    public class ContactEmailDataProvider : IContactEmailDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactEmailDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactEmailDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">Contact Type</param>
        /// <returns></returns>
        public Response<ContactEmailModel> GetEmails(long contactID, int? contactTypeID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };

            if (contactTypeID.HasValue)
                spParameters.Add(new SqlParameter("ContactTypeID", contactTypeID));
            else
                spParameters.Add(new SqlParameter("ContactTypeID", ContactType.Contact));

            var repository = _unitOfWork.GetRepository<ContactEmailModel>();
            var results = repository.ExecuteStoredProc("usp_GetContactEmails", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the emails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="emails">The emails.</param>
        /// <returns></returns>
        public Response<ContactEmailModel> AddEmails(long contactID, List<ContactEmailModel> emails)
        {
            var repository = _unitOfWork.GetRepository<ContactEmailModel>(SchemaName.Registration);

            var ContactID = new SqlParameter("ContactID", contactID);
            var requestXMLValueParam = new SqlParameter("EmailsXML", EmailToXML(emails));
            var spParameters = new List<SqlParameter>() { ContactID, requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<ContactEmailModel>>(repository.ExecuteNQStoredProc, "usp_AddContactEmails", spParameters, forceRollback: emails.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }

        /// <summary>
        /// Emails to XML.
        /// </summary>
        /// <param name="emails">The emails.</param>
        /// <returns></returns>
        public string EmailToXML(List<ContactEmailModel> emails)
        {
            var emailXml = new XElement("RequestXMLValue",
                from email in emails
                select new XElement("Email",
                               new XElement("EmailID", email.EmailID),
                               new XElement("Email", email.Email),
                               new XElement("EmailPermissionID", email.EmailPermissionID),
                               new XElement("IsPrimary", email.IsPrimary),
                               new XElement("EffectiveDate", email.EffectiveDate),
                               new XElement("ExpirationDate", email.ExpirationDate),
                               new XElement("ContactEmailID", email.ContactEmailID),
                               new XElement("IsActive",email.IsActive),
                               new XElement("ModifiedOn", email.ModifiedOn ?? DateTime.Now)
                           ));

            return emailXml.ToString();
        }

        /// <summary>
        /// Updates the emails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="emails">The emails.</param>
        /// <returns></returns>
        public Response<ContactEmailModel> UpdateEmails(long contactID, List<ContactEmailModel> emails)
        {
            var emailResults = new Response<ContactEmailModel>();
            emailResults.ResultCode = 0;

            // Prepare email collection to add/update
            var emailsToAdd = emails.Where(email => email.EmailID <= 0).ToList();
            
            var emailsToUpdate = emails.Where(email => email.EmailID != 0).ToList();

            if (emailsToAdd.Count() > 0)
            {
                emailResults = AddEmails(contactID, emailsToAdd);
                if (emailResults.ResultCode != 0)
                {
                    return emailResults;
                }
            }
            if (emailsToUpdate.Count() > 0)
            {
                var repository = _unitOfWork.GetRepository<ContactEmailModel>(SchemaName.Registration);
                var requestXMLValueParam = new SqlParameter("EmailXML", EmailToXML(emails));
                var spParameters = new List<SqlParameter>() { requestXMLValueParam };
                emailResults = _unitOfWork.EnsureInTransaction<Response<ContactEmailModel>>(repository.ExecuteNQStoredProc, "usp_UpdateContactEmails", spParameters, forceRollback: emails.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            return emailResults;
        }

        /// <summary>
        /// Deletes the email.
        /// </summary>
        /// <param name="contactEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactEmailModel> DeleteEmail(long contactEmailID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ContactEmailID", contactEmailID), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactBenefitRepository = _unitOfWork.GetRepository<ContactEmailModel>(SchemaName.Registration);
            Response<ContactEmailModel> spResults = new Response<ContactEmailModel>();
            spResults = contactBenefitRepository.ExecuteNQStoredProc("usp_DeleteContactEmail", procsParameters);
            return spResults;
        }


        #endregion exposed functionality
    }
}
