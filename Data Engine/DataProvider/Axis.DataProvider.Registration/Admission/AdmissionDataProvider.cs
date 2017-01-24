using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Admission Data Provider
    /// </summary>
    public class AdmissionDataProvider : IAdmissionDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmissionDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public AdmissionDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Admissions.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AdmissionModal> GetAdmission(long contactId)
        {
            var admissionRepository = unitOfWork.GetRepository<AdmissionModal>(SchemaName.Registration);
            SqlParameter contactIdParam = new SqlParameter("ContactID", contactId);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };
            return admissionRepository.ExecuteStoredProc("usp_GetContactAdmissions", procParams);
        }

        /// <summary>
        /// Add admission.
        /// </summary>
        /// <param name="admission">The admission.</param>
        /// <returns></returns>
        public Response<AdmissionModal> AddAdmission(AdmissionModal admission)
        {
            var admissionRepository = unitOfWork.GetRepository<AdmissionModal>(SchemaName.Registration);
            var procParams = BuildAdmissionSpParams(admission, false);
            return unitOfWork.EnsureInTransaction(
                    admissionRepository.ExecuteNQStoredProc,
                    "usp_AddContactAdmission",
                    procParams,
                    forceRollback: admission.ForceRollback.GetValueOrDefault(false),
                    idResult: true
                );
        }

        /// <summary>
        /// Updates admission
        /// </summary>
        /// <param name="admission">The admission.</param>
        /// <returns></returns>
        public Response<AdmissionModal> UpdateAdmission(AdmissionModal admission)
        {
            var procParams = BuildAdmissionSpParams(admission, true);
            var admissionRepository = unitOfWork.GetRepository<AdmissionModal>(SchemaName.Registration);
            return unitOfWork.EnsureInTransaction(
                   admissionRepository.ExecuteNQStoredProc,
                   "usp_UpdateContactAdmission",
                   procParams,
                   forceRollback: admission.ForceRollback.GetValueOrDefault(false),
                   idResult: false
               );
        }



        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Builds the additional demographics sp parameters.
        /// </summary>
        /// <param name="admission">The additional.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildAdmissionSpParams(AdmissionModal admission, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            if (isUpdate)
                spParameters.Add(new SqlParameter("ContactAdmissionID", admission.ContactAdmissionID));

            spParameters.Add(new SqlParameter("ContactID", admission.ContactID));
            spParameters.Add(new SqlParameter("OrganizationID", admission.ProgramUnitID));
            spParameters.Add(new SqlParameter("EffectiveDate", admission.EffectiveDate));
            spParameters.Add(new SqlParameter("UserID", admission.UserID));
            spParameters.Add(new SqlParameter("IsDocumentationComplete", admission.IsDocumentationComplete));
            spParameters.Add(new SqlParameter("Comments", admission.Comments));
            spParameters.Add(new SqlParameter("AdmissionReasonID", admission.AdmissionReasonID));
            spParameters.Add(new SqlParameter("IsCompanyActive", admission.IsCompanyActive));
            spParameters.Add(new SqlParameter("ModifiedOn", DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
