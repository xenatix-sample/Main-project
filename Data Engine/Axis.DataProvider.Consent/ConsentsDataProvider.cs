using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Consents;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Consents
{
    public class ConsentsDataProvider : IConsentsDataProvider
    {
        #region Private Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork _unitOfWork = null;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ConsentsDataProvider(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion Constructors

        /// <summary>
        /// Get consent list
        /// </summary>
        /// <param name="contactID">Contact id</param>
        /// <returns></returns>
        public Response<ConsentsModel> GetConsents(long contactID)
        {
            var consentRepository = _unitOfWork.GetRepository<ConsentsModel>(SchemaName.Registration);
            SqlParameter consentIdParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { consentIdParam };
            return consentRepository.ExecuteStoredProc("usp_GetContactConsents", procParams);
        }

        /// <summary>
        /// Adds the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Response<ConsentsModel> AddConsent(ConsentsModel consent)
        {
            var consentRepository = _unitOfWork.GetRepository<ConsentsModel>(SchemaName.Registration);
            var procParams = BuildSpParams(consent);

            return _unitOfWork.EnsureInTransaction(consentRepository.ExecuteNQStoredProc, "usp_AddContactConsent", procParams, idResult: true,
                forceRollback: consent.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Updates the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Response<ConsentsModel> UpdateConsent(ConsentsModel consent)
        {
            var benefitsAssistanceRepository = _unitOfWork.GetRepository<ConsentsModel>(SchemaName.Registration);
            var procParams = BuildSpParams(consent);

            return _unitOfWork.EnsureInTransaction(benefitsAssistanceRepository.ExecuteNQStoredProc, "usp_UpdateContactConsent", procParams,
                forceRollback: consent.ForceRollback.GetValueOrDefault(false));
        }

        private List<SqlParameter> BuildSpParams(ConsentsModel consent)
        {
            var spParameters = new List<SqlParameter>();
            if (consent.ContactConsentID > 0)
                spParameters.Add(new SqlParameter("ContactConsentID", consent.ContactConsentID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactID", (object)consent.ContactID ?? DBNull.Value),
                new SqlParameter("AssessmentID", (object)consent.AssessmentID ?? DBNull.Value),
                new SqlParameter("AssessmentSectionID", (object)consent.AssessmentSectionID ?? DBNull.Value),
                new SqlParameter("ResponseID", (object)consent.ResponseID ?? DBNull.Value),
                new SqlParameter("DateSigned", (object)consent.DateSigned ?? DBNull.Value),
                new SqlParameter("EffectiveDate", (object)consent.EffectiveDate ?? DBNull.Value),
                new SqlParameter("ExpirationDate", (object)consent.ExpirationDate ?? DBNull.Value),
                new SqlParameter("ExpirationReasonID", (object)consent.ExpirationReasonID ?? DBNull.Value),
                new SqlParameter("ExpiredResponseID", (object)consent.ExpiredResponseID ?? DBNull.Value),
                new SqlParameter("ExpiredBy", (object)consent.ExpiredBy ?? DBNull.Value),
                new SqlParameter("SignatureStatusID", (object)consent.SignatureStatusID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object)consent.ModifiedOn ?? DBNull.Value)
            });
            //var spParameters = new List<SqlParameter> {
            //    new SqlParameter("ContactID", (object)consent.ContactID ?? DBNull.Value),
            //    new SqlParameter("AssessmentID", (object)consent.AssessmentID ?? DBNull.Value),
            //    new SqlParameter("AssessmentSectionID", (object)consent.AssessmentSectionID ?? DBNull.Value),
            //    new SqlParameter("ResponseID", (object)consent.ResponseID ?? DBNull.Value),
            //    new SqlParameter("DateSigned", (object)consent.DateSigned ?? DBNull.Value),
            //    new SqlParameter("EffectiveDate", (object)consent.EffectiveDate ?? DBNull.Value),
            //    new SqlParameter("SignatureStatusID", (object)consent.SignatureStatusID ?? DBNull.Value),
            //    new SqlParameter("ModifiedOn", (object)consent.ModifiedOn ?? DBNull.Value)
            //};
            return spParameters;
        }

        //private List<SqlParameter> BuildUpdateSpParams(ConsentsModel consent)
        //{
        //    var spParameters = new List<SqlParameter> {
        //        new SqlParameter("ContactConsentID", consent.ContactConsentID),
        //        new SqlParameter("DateSigned", (object)consent.DateSigned ?? DBNull.Value),
        //        new SqlParameter("ExpirationDate", (object)consent.ExpirationDate ?? DBNull.Value),
        //        new SqlParameter("ExpirationReasonID", (object)consent.ExpirationReasonID ?? DBNull.Value),
        //        new SqlParameter("ExpiredResponseID", (object)consent.ExpiredResponseID ?? DBNull.Value),
        //        new SqlParameter("ExpiredBy", (object)consent.ExpiredBy ?? DBNull.Value),
        //        new SqlParameter("SignatureStatusID", (object)consent.SignatureStatusID ?? DBNull.Value),
        //        new SqlParameter("ModifiedOn", (object)consent.ModifiedOn ?? DBNull.Value)
        //    };
        //    return spParameters;
        //}

    }
}
