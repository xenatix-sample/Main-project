﻿using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// interface for Financial Assessment Rule Engine
    /// </summary>
    public interface IFinancialAssessmentRuleEngine
    {
        Response<FinancialAssessmentModel> GetFinancialAssessment(long contactID, long financialAssessmentID);
        Response<FinancialAssessmentModel> AddFinancialAssessment(FinancialAssessmentModel financialAssessment);
        Response<FinancialAssessmentModel> UpdateFinancialAssessment(FinancialAssessmentModel financialAssessment);
        Response<FinancialAssessmentDetailsModel> AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail);
        Response<FinancialAssessmentDetailsModel> UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail);
        Response<bool> DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn);
    }
}