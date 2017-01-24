using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referrals.ClientInformation
{
    public class ReferralClientDemographicsDataProvider : IReferralClientDemographicsDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        public ReferralClientDemographicsDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Response<ContactDemographicsModel> GetClientDemographics(long ContactID)
        {
            var referralRepository = unitOfWork.GetRepository<ContactDemographicsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", ContactID) };
            return referralRepository.ExecuteStoredProc("usp_GetContactDemographics", procParams);
        }

        /// <summary>
        /// Adds the Referral client demographics.
        /// </summary>
        /// <param name="referral">The referral demographics .</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> AddClientDemographics(ContactDemographicsModel referralDemographics)
        {
            var referralRepository = unitOfWork.GetRepository<ContactDemographicsModel>(SchemaName.Registration);
            var procParams = BuildParams(referralDemographics);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddContactDemographics",
                    procParams, idResult: true,
                    forceRollback: referralDemographics.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Updates the Referral client demographics.
        /// </summary>
        /// <param name="referral">The referral demographics .</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> UpdateClientDemographics(ContactDemographicsModel referralDemographics)
        {
            var referralRepository = unitOfWork.GetRepository<ContactDemographicsModel>(SchemaName.Registration);
            var procParams = BuildParams(referralDemographics);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateContactDemographics",
                    procParams, idResult: false,
                    forceRollback: referralDemographics.ForceRollback.GetValueOrDefault(false)
                );
        }

        #region Private Methods

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="model">The referral.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ContactDemographicsModel contact)
        {
            var spParameters = new List<SqlParameter>
            {
                new SqlParameter("ContactID", (object)contact.ContactID ?? DBNull.Value),
                new SqlParameter("ContactTypeID", (object)contact.ContactTypeID ?? DBNull.Value),
                new SqlParameter("ClientTypeID", (object)contact.ClientTypeID ?? DBNull.Value),
                new SqlParameter("FirstName", (object)contact.FirstName ?? DBNull.Value),
                new SqlParameter("Middle", (object)contact.Middle ?? DBNull.Value),
                new SqlParameter("LastName", (object)contact.LastName ?? DBNull.Value),
                new SqlParameter("SuffixID", (object)contact.SuffixID ?? DBNull.Value),
                new SqlParameter("GenderID", (object)contact.GenderID ?? DBNull.Value),
                new SqlParameter("PreferredGenderID", (object)contact.PreferredGenderID ?? DBNull.Value),
                new SqlParameter("TitleID", (object)contact.TitleID ?? DBNull.Value),
                new SqlParameter("SequesteredByID", (object)contact.SequesteredByID ?? DBNull.Value),
                new SqlParameter("DOB", (object)contact.DOB ?? DBNull.Value),
                new SqlParameter("DOBStatusID", (object)contact.DOBStatusID ?? DBNull.Value),
                new SqlParameter("SSN", (object)contact.SSN ?? DBNull.Value),
                new SqlParameter("SSNStatusID", (object)contact.SSNStatusID ?? DBNull.Value),
                new SqlParameter("DriverLicense", (object)contact.DriverLicense ?? DBNull.Value),
                new SqlParameter("DriverLicenseStateID", (object)contact.DriverLicenseStateID ?? DBNull.Value),
                new SqlParameter("IsPregnant", (object)contact.IsPregnant ?? DBNull.Value),
                new SqlParameter("PreferredName", (object)contact.PreferredName ?? DBNull.Value),
                new SqlParameter("DeceasedDate", (object)contact.DeceasedDate ?? DBNull.Value),
                new SqlParameter("PreferredContactMethodID", (object)contact.ContactMethodID ?? DBNull.Value),
                new SqlParameter("ReferralSourceID", (object)contact.ReferralSourceID ?? DBNull.Value),
                new SqlParameter("GestationalAge", (object)contact.GestationalAge ?? DBNull.Value),
                new SqlParameter("ModifiedOn", contact.ModifiedOn ?? DateTime.Now)
            };

            return spParameters;
        }

        #endregion Private Methods
    }
}
