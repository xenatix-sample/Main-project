using System;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.Registration.Referrals.Common
{
   public class ReferralEmailDataProvider :IReferralEmailDataProvider
    {
       #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralEmailDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

       #region exposed functionality

        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralEmailModel> GetEmails(long referralID, int? contactTypeID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ReferralID", referralID) };

            //if (contactTypeID.HasValue)
            //    spParameters.Add(new SqlParameter("ContactTypeID", contactTypeID));
            //else
            //    spParameters.Add(new SqlParameter("ContactTypeID", ContactType.Contact));

            var repository = _unitOfWork.GetRepository<ReferralEmailModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetReferralEmails", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="emails">The emails.</param>
        /// <returns></returns>
        private Response<ReferralEmailModel> AddEmails(long referralID, List<ReferralEmailModel> emails)
        {
            var repository = _unitOfWork.GetRepository<ReferralEmailModel>(SchemaName.Registration);

            var ReferralID = new SqlParameter("ReferralID", referralID);
            var requestXMLValueParam = new SqlParameter("EmailsXML", EmailToXML(emails));
            var spParameters = new List<SqlParameter>() { ReferralID, requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<ReferralEmailModel>>(repository.ExecuteNQStoredProc, "usp_AddReferralEmails", spParameters, forceRollback: emails.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }

        /// <summary>
        /// Emails to XML.
        /// </summary>
        /// <param name="emails">The emails.</param>
        /// <returns></returns>
        public string EmailToXML(List<ReferralEmailModel> emails)
        {
            var emailXml = new XElement("RequestXMLValue",
                from email in emails
                select new XElement("Email",
                               new XElement("EmailID", email.EmailID),
                               new XElement("Email", email.Email),
                               new XElement("EmailPermissionID", email.EmailPermissionID),
                               new XElement("IsPrimary", email.IsPrimary),
                               new XElement("ReferralEmailID", email.ReferralEmailID),
                               new XElement("IsActive",email.IsActive),
                               new XElement("ModifiedOn", email.ModifiedOn ?? DateTime.Now)
                           ));

            return emailXml.ToString();
        }

        /// <summary>
        /// Adds and update emails.
        /// </summary>
        /// <param name="emails">The emails.</param>
        /// <returns></returns>
        public Response<ReferralEmailModel> AddUpdateEmails(List<ReferralEmailModel> emails)
        {
            long referralID = 0;
            if (emails.Count > 0)
                referralID = emails[0].ReferralID;

            var emailResults = new Response<ReferralEmailModel>();
            emailResults.ResultCode = 0;

            // Prepare email collection to add/update
            var emailsToAdd = emails.Where(email => email.EmailID <= 0).ToList();
            
            var emailsToUpdate = emails.Where(email => email.EmailID != 0).ToList();

            if (emailsToAdd.Count() > 0)
            {
                emailResults = AddEmails(referralID, emailsToAdd);
                if (emailResults.ResultCode != 0)
                {
                    return emailResults;
                }
            }
            if (emailsToUpdate.Count() > 0)
            {
                var repository = _unitOfWork.GetRepository<ReferralEmailModel>(SchemaName.Registration);
                var requestXMLValueParam = new SqlParameter("EmailXML", EmailToXML(emails));
                var spParameters = new List<SqlParameter>() { requestXMLValueParam };
                emailResults = _unitOfWork.EnsureInTransaction<Response<ReferralEmailModel>>(repository.ExecuteNQStoredProc, "usp_UpdateReferralEmails", spParameters, forceRollback: emails.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            return emailResults;
        }

        /// <summary>
        /// Deletes the email.
        /// </summary>
        /// <param name="referralEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralEmailModel> DeleteEmail(long referralEmailID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ReferralEmailID", referralEmailID), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactBenefitRepository = _unitOfWork.GetRepository<ReferralEmailModel>(SchemaName.Registration);
            Response<ReferralEmailModel> spResults = new Response<ReferralEmailModel>();
            spResults = contactBenefitRepository.ExecuteNQStoredProc("usp_DeleteReferralEmail", procsParameters);
            return spResults;
        }

        #endregion exposed functionality
    }
}
