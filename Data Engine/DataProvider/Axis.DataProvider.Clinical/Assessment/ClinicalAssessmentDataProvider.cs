using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Clinical.Assessment
{
    public class ClinicalAssessmentDataProvider :IClinicalAssessmentDataProvider
    {

        private readonly IUnitOfWork _unitOfWork;


        /// <summary>
        /// Initializes a new instance of the <see cref="ClinicalAssessmentDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ClinicalAssessmentDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Get list of assessments for contact
        /// </summary>
        /// <param name="contactID">The contact Id.</param>
        /// <returns></returns>
        /// 
        public Response<ClinicalAssessmentModel> GetClinicalAssessments(long clinicalAssessmentID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("clinicalAssessmentID", clinicalAssessmentID) };
            var repository = _unitOfWork.GetRepository<ClinicalAssessmentModel>(SchemaName.Clinical);
            return repository.ExecuteStoredProc("usp_GetClinicalAssessments", spParameters);
        }


        public Response<ClinicalAssessmentModel> GetClinicalAssessmentsByContact(long contactID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var repository = _unitOfWork.GetRepository<ClinicalAssessmentModel>(SchemaName.Clinical);
            return repository.ExecuteStoredProc("usp_GetClinicalAssessmentsByContact", spParameters);
        }
        
        /// <summary>
        /// Add assessment for contact
        /// </summary>
        /// <param name="assessment">The assessment</param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> AddAssessment(ClinicalAssessmentModel assessment)
        {
            var spParameters = BuildSpParams(assessment);
            var repository = _unitOfWork.GetRepository<ClinicalAssessmentModel>(SchemaName.Clinical);
            return _unitOfWork.EnsureInTransaction<Response<ClinicalAssessmentModel>>(repository.ExecuteNQStoredProc, "usp_AddClinicalAssessments", spParameters, forceRollback: assessment.ForceRollback.GetValueOrDefault(false), idResult: true);
        }

        /// <summary>
        /// Update Assessment
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> UpdateAssessment(ClinicalAssessmentModel assessment)
        {
            var spParameters = BuildSpParams(assessment);
            var repository = _unitOfWork.GetRepository<ClinicalAssessmentModel>(SchemaName.Clinical);
            return _unitOfWork.EnsureInTransaction<Response<ClinicalAssessmentModel>>(repository.ExecuteNQStoredProc, "usp_UpdateClinicalAssessments", spParameters, forceRollback: assessment.ForceRollback.GetValueOrDefault(false));
        }


        /// <summary>
        /// Delete Assessment
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> DeleteAssessment(long Id, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ClinicalAssessmentID", Id), new SqlParameter("ModifiedOn", modifiedOn) };
            var assessmentRepository = _unitOfWork.GetRepository<ClinicalAssessmentModel>(SchemaName.Clinical);
            Response<ClinicalAssessmentModel> spResults = assessmentRepository.ExecuteNQStoredProc("usp_DeleteClinicalAssessments", procsParameters);
            return spResults;
        }


        /// <summary>
        /// Build stored procedure parameters
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        private List<SqlParameter> BuildSpParams(ClinicalAssessmentModel assessment)
        {
            var spParameters = new List<SqlParameter>();
            if (assessment.ClinicalAssessmentID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("ClinicalAssessmentID", assessment.ClinicalAssessmentID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactID", assessment.ContactID),
                new SqlParameter("AssessmentDate", (object) assessment.TakenTime ?? string.Empty),
                new SqlParameter("UserID", assessment.TakenBy),
                new SqlParameter("AssessmentID", assessment.AssessmentID),
                new SqlParameter("ResponseID",(object) assessment.ResponseID??DBNull.Value),
                new SqlParameter("AssessmentStatusID", (object) assessment.EncounterID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", assessment.ModifiedOn ?? DateTime.Now)
            });

            return spParameters;
        }


    }
}
