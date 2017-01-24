using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Service.Registration
{
    public interface IFinancialAssessmentService
    {
        Response<FinancialAssessmentModel> GetFinancialAssessment(long contactID, long financialAssessmentID);
        Response<FinancialAssessmentModel> AddFinancialAssessment(FinancialAssessmentModel financialAssessment);
        Response<FinancialAssessmentModel> UpdateFinancialAssessment(FinancialAssessmentModel financialAssessment);
        Response<FinancialAssessmentDetailsModel> AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail);
        Response<FinancialAssessmentDetailsModel> UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail);
        Response<bool> DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn);
    }
}
