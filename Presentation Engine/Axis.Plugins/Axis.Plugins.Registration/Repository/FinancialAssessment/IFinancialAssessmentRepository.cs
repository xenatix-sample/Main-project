using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.Repository.FinancialAssessment
{
    /// <summary>
    /// Interface for Financial Assessment Repository to call web api methods.
    /// </summary>
    public interface IFinancialAssessmentRepository
    {
        Response<FinancialAssessmentViewModel> GetFinancialAssessment(long contactID, long financialAssessmentID);
        Task<Response<FinancialAssessmentViewModel>> GetFinancialAssessmentAsync(long contactID, long financialAssessmentID);
        Task<Response<FinancialAssessmentDetailsModel>> GetFinancialAssessmentDetailsAsync(long contactID, long financialAssessmentID);
        Response<FinancialAssessmentViewModel> AddFinancialAssessment(FinancialAssessmentViewModel financialAssessment);
        Response<FinancialAssessmentViewModel> UpdateFinancialAssessment(FinancialAssessmentViewModel financialAssessment);
        Response<FinancialAssessmentDetailsModel> AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetails);
        Response<FinancialAssessmentDetailsModel> UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetails);
        Response<bool> DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn);
    }
}