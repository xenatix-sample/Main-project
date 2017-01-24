using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Common;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Data provider for Patient Profile
    /// </summary>
    public class PatientProfileDataProvider : IPatientProfileDataProvider
    {
        #region initializations

        IUnitOfWork unitOfWork = null;

        public PatientProfileDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// To get Patient Profile for contact id
        /// </summary>
        /// <param name="contactID">client contact id</param> 
        /// <returns>Patient Profile details</returns>
        public Response<PatientProfileModel> GetPatientProfile(long contactID)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var patientProfileRepository = unitOfWork.GetRepository<PatientProfileModel>(SchemaName.Registration);
            var patientProfileResults = patientProfileRepository.ExecuteStoredProc("usp_GetPatientProfile", procsParameters);
            return patientProfileResults;
        }

        #endregion  exposed functionality

    }
}
