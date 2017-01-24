using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Model.Clinical;

namespace Axis.DataProvider.Clinical.Vital
{
    public class VitalDataProvider : IVitalDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public VitalDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<VitalModel> GetContactVitals(long ContactId)
        {
            var vitalRepository = unitOfWork.GetRepository<VitalModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("ContactID", ContactId) };
            var contactVital = vitalRepository.ExecuteStoredProc("usp_GetVitalByContactID", procParams);
            return contactVital;
        }

        /// <summary>
        /// Get BPMethods
        /// </summary>
        /// <returns></returns>
        public Response<BPMethodModel> GetBPMethods() {

            var vitalRepository = unitOfWork.GetRepository<BPMethodModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter>() {};
            var bpMethods = vitalRepository.ExecuteStoredProc("usp_GetBPMethod", procParams);
            return bpMethods;
        
        }
        
        /// <summary>
        /// Adds the Vital.
        /// </summary>
        /// <param name="vital">The vital model.</param>
        /// <returns></returns>
        public Response<VitalModel> AddVital(VitalModel vital)
        {
            var vitalParameters = BuildVitalSpParams(vital);
            var vitalRepository = unitOfWork.GetRepository<VitalModel>(SchemaName.Clinical);
            return vitalRepository.ExecuteNQStoredProc("usp_AddVital", vitalParameters, idResult: true);
        }

        /// <summary>
        /// Updates the Vital.
        /// </summary>
        /// <param name="vital">The vital model.</param>
        /// <returns></returns>
        public Response<VitalModel> UpdateVital(VitalModel vital)
        {
            var vitalParameters = BuildVitalSpParams(vital);
            var vitalRepository = unitOfWork.GetRepository<VitalModel>(SchemaName.Clinical);
            return vitalRepository.ExecuteNQStoredProc("usp_UpdateVital", vitalParameters);
        }

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<VitalModel> DeleteVital(long id, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("VitalId", id), new SqlParameter("ModifiedOn", modifiedOn) };
            var vitalRepository = unitOfWork.GetRepository<VitalModel>(SchemaName.Clinical);
            Response<VitalModel> spResults = new Response<VitalModel>();
            spResults = vitalRepository.ExecuteNQStoredProc("usp_DeleteVital", procsParameters);
            return spResults;
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Builds the vital sp parameters.
        /// </summary>
        /// <param name="vital">The Vital.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildVitalSpParams(VitalModel vital)
        {
            var spParameters = new List<SqlParameter>();

            if (vital.VitalID > 0) // Update, not Add
                spParameters.Add(new SqlParameter("VitalID", vital.VitalID));

            spParameters.AddRange(new List<SqlParameter>
            {
                new SqlParameter("ContactID", vital.ContactID),
                new SqlParameter("EncounterID", (object) vital.EncounterID ?? DBNull.Value),
                new SqlParameter("HeightFeet", (object) vital.HeightFeet?? DBNull.Value),
                new SqlParameter("HeightInches", (object) vital.HeightInches?? DBNull.Value),
                new SqlParameter("WeightLbs", (object) vital.WeightLbs?? DBNull.Value),
                new SqlParameter("WeightOz", (object) vital.WeightOz?? DBNull.Value),
                new SqlParameter("BMI", (object) vital.BMI?? DBNull.Value),
                new SqlParameter("LMP", (object) vital.LMP ?? DBNull.Value),
                new SqlParameter("TakenTime",(object) vital.TakenTime ??  DBNull.Value),
                new SqlParameter("TakenBy", (object) vital.TakenBy ?? DBNull.Value),
                new SqlParameter("BPMethodID", (object) vital.BPMethodID ?? DBNull.Value),
                new SqlParameter("Systolic", (object) vital.Systolic ?? DBNull.Value),
                new SqlParameter("Diastolic", (object) vital.Diastolic ?? DBNull.Value),
                new SqlParameter("OxygenSaturation", (object) vital.OxygenSaturation ?? DBNull.Value),
                new SqlParameter("RespiratoryRate", (object) vital.RespiratoryRate ?? DBNull.Value),
                new SqlParameter("Pulse", (object) vital.Pulse ?? DBNull.Value),
                new SqlParameter("Temperature", (object) vital.Temperature ?? DBNull.Value),
                new SqlParameter("Glucose", (object) vital.Glucose ?? DBNull.Value),
                new SqlParameter("WaistCircumference", (object) vital.WaistCircumference ?? DBNull.Value),
                new SqlParameter("ModifiedOn", vital.ModifiedOn ?? DateTime.Now)
            });
            return spParameters;
        }

        #endregion Helpers
    }
}
