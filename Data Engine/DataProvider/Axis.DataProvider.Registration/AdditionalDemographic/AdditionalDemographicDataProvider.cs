using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class AdditionalDemographicDataProvider : IAdditionalDemographicDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        private ILogger _logger = null;

        private IRegistrationDataProvider _registrationDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public AdditionalDemographicDataProvider(IUnitOfWork unitOfWork, IRegistrationDataProvider registrationDataProvider, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            _registrationDataProvider = registrationDataProvider;
            _logger = logger;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AdditionalDemographicsModel> GetAdditionalDemographic(long contactId)
        {
            var additionalDemographicsRepository = unitOfWork.GetRepository<AdditionalDemographicsModel>(SchemaName.Registration);

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
        public Response<AdditionalDemographicsModel> AddAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            var contactDemographicsRepository = unitOfWork.GetRepository<AdditionalDemographicsModel>(SchemaName.Registration);
            var defaultErrorMessage = "Error while saving the additional demographics";
            var response = new Response<AdditionalDemographicsModel>() { ResultCode = -1, ResultMessage = defaultErrorMessage };

            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var generateMRN = additional.GenerateMRN.HasValue ? Convert.ToBoolean(additional.GenerateMRN) : true;
                    var additionalDemographicsParameters = BuildAdditionalDemographicsSpParams(additional, false);
                    var addDemographicsResponse = contactDemographicsRepository.ExecuteNQStoredProc("usp_AddAdditionalDemographics", additionalDemographicsParameters);

                    if (addDemographicsResponse.ResultCode != 0)
                        goto end;

                    //Save the MRN
                    var mrnResponse = SaveMRN(additional, contactDemographicsRepository, generateMRN);

                    if (mrnResponse.ResultCode != 0)
                        goto end;

                    response = addDemographicsResponse;
                    if (!additional.ForceRollback.GetValueOrDefault(false))
                        unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = defaultErrorMessage;
                }
            }

        end:
            return response;
        }

        private Response<AdditionalDemographicsModel> SaveMRN(AdditionalDemographicsModel additional, IRepository<AdditionalDemographicsModel> contactDemographicsRepository, bool generateMRN)
        {
            var hasMRN = false;
            var mrnResponse = new Response<AdditionalDemographicsModel>() { ResultCode = 0, ResultMessage = "Executed Successfully" };
            var contactMRNRepository = unitOfWork.GetRepository<ContactDemographicsModel>(SchemaName.Registration);
            SqlParameter mrnContactIDParam = new SqlParameter("ContactID", additional.ContactID);
            var procParams = new List<SqlParameter>() { mrnContactIDParam };
            var contactMRNResponse = contactMRNRepository.ExecuteStoredProc("usp_GetContactDemographics", procParams);
            if (contactMRNResponse.DataItems.Count > 0)
            {
                if (contactMRNResponse.DataItems[0].MRN != null)
                {
                    hasMRN = true;
                }
            }

            if (!hasMRN && generateMRN)
            {
                SqlParameter contactIDParam = new SqlParameter("ContactID", additional.ContactID);
                SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", additional.ModifiedOn ?? DateTime.Now);
                mrnResponse = contactDemographicsRepository.ExecuteNQStoredProc("usp_GenerateMRN", new List<SqlParameter>() { contactIDParam, modifiedOnParam });
            }

            return mrnResponse;
        }

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        public Response<AdditionalDemographicsModel> UpdateAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            var generateMRN = additional.GenerateMRN.HasValue ? Convert.ToBoolean(additional.GenerateMRN) : true;
            var defaultErrorMessage = "Error while updating the additional demographics";
            var response = new Response<AdditionalDemographicsModel>() { ResultCode = -1, ResultMessage = defaultErrorMessage };

            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var additionalDemographicsParameters = BuildAdditionalDemographicsSpParams(additional, true);
                    var contactDemographicsRepository = unitOfWork.GetRepository<AdditionalDemographicsModel>(SchemaName.Registration);
                    var updateDemographicsResponse = contactDemographicsRepository.ExecuteNQStoredProc("usp_UpdateAdditionalDemographics", additionalDemographicsParameters);

                    if (updateDemographicsResponse.ResultCode == 0)
                    {
                        //Save the MRN
                        var mrnResponse = SaveMRN(additional, contactDemographicsRepository, generateMRN);

                        if (mrnResponse.ResultCode != 0)
                        {
                            response = mrnResponse;
                        }
                        else
                        {
                            response = updateDemographicsResponse;
                            if (!additional.ForceRollback.GetValueOrDefault(false))
                                unitOfWork.TransactionScopeComplete(transactionScope);   
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = defaultErrorMessage;
                }
            }

            return response;
        }

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<AdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn)
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
        private List<SqlParameter> BuildAdditionalDemographicsSpParams(AdditionalDemographicsModel additional, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            if (isUpdate)
                spParameters.Add(new SqlParameter("AdditionalDemographicID", additional.AdditionalDemographicID));

            spParameters.Add(new SqlParameter("ContactID", additional.ContactID));
            spParameters.Add(new SqlParameter("AdvancedDirective", (object)additional.AdvancedDirective ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AdvancedDirectiveTypeID", (object)additional.AdvancedDirectiveTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SmokingStatusID", (object)additional.SmokingStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherRace", (object)additional.OtherRace ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherEthnicity", (object)additional.OtherEthnicity ?? DBNull.Value));
            spParameters.Add(new SqlParameter("LookingForWork", (object)additional.LookingForWork ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SchoolDistrictID", (object)additional.SchoolDistrictID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EthnicityID", (object)additional.EthnicityID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("PrimaryLanguageID", (object)additional.PrimaryLanguageID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SecondaryLanguageID", (object)additional.SecondaryLanguageID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("LegalStatusID", (object)additional.LegalStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("LivingArrangementID", (object)additional.LivingArrangementID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("CitizenshipID", (object)additional.CitizenshipID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("MaritalStatusID", (object)additional.MaritalStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EmploymentStatusID", (object)additional.EmploymentStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("PlaceOfEmployment", (object)additional.PlaceOfEmployment ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EmploymentBeginDate", (object)additional.EmploymentBeginDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EmploymentEndDate", (object)additional.EmploymentEndDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReligionID", (object)additional.ReligionID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("VeteranStatusID", (object)additional.VeteranStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EducationStatusID", (object)additional.EducationStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SchoolAttended", (object)additional.SchoolAttended ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SchoolBeginDate", (object)additional.SchoolBeginDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SchoolEndDate", (object)additional.SchoolEndDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("LivingWill", (object)additional.LivingWill ?? DBNull.Value));
            spParameters.Add(new SqlParameter("PowerOfAttorney", (object)additional.PowerOfAttorney ?? DBNull.Value));
            spParameters.Add(new SqlParameter("InterpreterRequired", (object)additional.InterpreterRequired ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherLegalstatus ", (object)additional.OtherLegalstatus ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherPreferredLanguage ", (object)additional.OtherPreferredLanguage ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherSecondaryLanguage ", (object)additional.OtherSecondaryLanguage ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherCitizenship ", (object)additional.OtherCitizenship ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherEducation ", (object)additional.OtherEducation ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherLivingArrangement ", (object)additional.OtherLivingArrangement ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherVeteranStatus ", (object)additional.OtherVeteranStatus ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherEmploymentStatus", (object)additional.OtherEmploymentStatus ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherReligion ", (object)additional.OtherReligion ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", additional.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
