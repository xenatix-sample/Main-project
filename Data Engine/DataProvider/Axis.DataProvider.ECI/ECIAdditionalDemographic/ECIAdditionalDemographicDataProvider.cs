using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.ECI
{
    public class ECIAdditionalDemographicDataProvider : IECIAdditionalDemographicDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Logging
        /// </summary>
        private ILogger logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ECIAdditionalDemographicDataProvider(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<ECIAdditionalDemographicsModel> GetAdditionalDemographic(long contactId)
        {
            var additionalDemographicsRepository = unitOfWork.GetRepository<ECIAdditionalDemographicsModel>(SchemaName.ECI);

            SqlParameter contactIdParam = new SqlParameter("ContactID", contactId);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };

            var additionalDemography = additionalDemographicsRepository.ExecuteStoredProc("usp_GetAdditionalDemographics", procParams);

            return additionalDemography;
        }

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        public Response<ECIAdditionalDemographicsModel> AddAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {            
            var contactDemographicsRepository = unitOfWork.GetRepository<ECIAdditionalDemographicsModel>(SchemaName.ECI);
            var contactMRNRepository = unitOfWork.GetRepository<ECIAdditionalDemographicsModel>(SchemaName.Registration);
            var defaultErrorMessage = "Error while saving the additional demographics";
            var response = new Response<ECIAdditionalDemographicsModel>() { ResultCode = -1, ResultMessage = defaultErrorMessage };

            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var additionalDemographicsParameters = BuildAdditionalDemographicsSpParams(additional, false);
                    var addDemographicsResponse = contactDemographicsRepository.ExecuteNQStoredProc("usp_AddAdditionalDemographics", additionalDemographicsParameters);

                    if (addDemographicsResponse.ResultCode != 0)
                        goto end;

                    SqlParameter contactIDParam = new SqlParameter("ContactID", additional.ContactID);
                    SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", additional.ModifiedOn ?? DateTime.Now);
                    var mrnResponse = contactMRNRepository.ExecuteNQStoredProc("usp_GenerateMRN", new List<SqlParameter>() { contactIDParam, modifiedOnParam });

                    if (mrnResponse.ResultCode != 0)
                        goto end;

                    response = addDemographicsResponse;
                    if (!additional.ForceRollback.GetValueOrDefault(false))
                        unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = defaultErrorMessage;
                }
            }

            end:
            return response;
        }

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        public Response<ECIAdditionalDemographicsModel> UpdateAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            var additionalDemographicsParameters = BuildAdditionalDemographicsSpParams(additional,true);
            var contactDemographicsRepository = unitOfWork.GetRepository<ECIAdditionalDemographicsModel>(SchemaName.ECI);
            return contactDemographicsRepository.ExecuteNQStoredProc("usp_UpdateAdditionalDemographics", additionalDemographicsParameters);
        }

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ECIAdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            throw new NotImplementedException();
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Builds the additional demographics sp parameters.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildAdditionalDemographicsSpParams(ECIAdditionalDemographicsModel additional, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            if (isUpdate)
                spParameters.Add(new SqlParameter("AdditionalDemographicID", additional.AdditionalDemographicID));

            spParameters.Add(new SqlParameter("ContactID", additional.ContactID));
            spParameters.Add(new SqlParameter("ReferralDispositionStatusID", (object)additional.ReferralDispositionStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SchoolDistrictID", (object)additional.SchoolDistrictID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherRace",(object)additional.OtherRace ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EthnicityID", additional.EthnicityID));
            spParameters.Add(new SqlParameter("OtherEthnicity",(object)additional.OtherEthnicity ?? DBNull.Value));
            spParameters.Add(new SqlParameter("PrimaryLanguageID", (object)additional.LanguageID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherPreferredLanguage", (object)additional.OtherPreferredLanguage ?? DBNull.Value));
            spParameters.Add(new SqlParameter("InterpreterRequired", (object)additional.InterpreterRequired ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsCPSInvolved", (object)additional.IsCPSInvolved ));
            spParameters.Add(new SqlParameter("IsChildHospitalized", (object)additional.IsChildHospitalized ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ExpectedHospitalDischargeDate", (object)additional.ExpectedHospitalDischargeDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("BirthWeightLbs", (object)additional.BirthWeightLbs ?? DBNull.Value));
            spParameters.Add(new SqlParameter("BirthWeightOz", (object)additional.BirthWeightOz ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsTransfer", (object)additional.IsTransfer ?? DBNull.Value));
            spParameters.Add(new SqlParameter("TransferFrom", (object)additional.TransferFrom ?? DBNull.Value));
            spParameters.Add(new SqlParameter("TransferDate", (object)additional.TransferDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsOutOfServiceArea", (object)additional.IsOutOfServiceArea ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReportingUnitID", (object)additional.ReportingUnitID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ServiceCoordinatorID", (object)additional.ServiceCoordinatorID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ServiceCoordinatorPhoneID", (object)additional.ServiceCoordinatorPhoneID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", additional.ModifiedOn ?? DateTime.Now));
            return spParameters;
        }

        #endregion Helpers
    }
}
