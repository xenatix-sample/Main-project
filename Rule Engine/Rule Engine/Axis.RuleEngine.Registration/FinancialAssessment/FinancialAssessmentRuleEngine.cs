using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// Rule engine class for calling service method of financial Assessment Service
    /// </summary>
    public class FinancialAssessmentRuleEngine : IFinancialAssessmentRuleEngine
    {
        private readonly IFinancialAssessmentService financialAssessmentService;

        public FinancialAssessmentRuleEngine(IFinancialAssessmentService financialAssessmentService)
        {
            this.financialAssessmentService = financialAssessmentService;
        }

        public FinancialAssessmentRuleEngine()
        {            
        }

        /// <summary>
        /// To get the financial assessment details of contact id 
        /// </summary>
        /// <param name="contactID">contact id for which financial assessment data will fetch</param> 
        /// <param name="financialAssessmentID">financialAssessmentID</param>
        /// <returns>Response type financial assesment model </returns>
        public Response<FinancialAssessmentModel> GetFinancialAssessment(long contactID, long financialAssessmentID)
        {
            return financialAssessmentService.GetFinancialAssessment(contactID,financialAssessmentID);
        }

        /// <summary>
        /// To add the financial assessment for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>Response type financial assesment model </returns>
        public Response<FinancialAssessmentModel> AddFinancialAssessment(FinancialAssessmentModel financialAssessment)
        {
            return financialAssessmentService.AddFinancialAssessment(financialAssessment);
        }

        /// <summary>
        /// To update the financial assessment for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>Response type financial assesment model </returns>
        public Response<FinancialAssessmentModel> UpdateFinancialAssessment(FinancialAssessmentModel financialAssessment)
        {
            return financialAssessmentService.UpdateFinancialAssessment(financialAssessment);
        }

        /// <summary>
        /// To add the financial assessment details for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>Response type financial assesment model </returns>
        public Response<FinancialAssessmentDetailsModel> AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetails)
        {
            return financialAssessmentService.AddFinancialAssessmentDetails(financialAssessmentDetails);
        }

        /// <summary>
        /// To update the financial assessment details for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>Response type financial assesment model </returns>
        public Response<FinancialAssessmentDetailsModel> UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetails)
        {
            return financialAssessmentService.UpdateFinancialAssessmentDetails(financialAssessmentDetails);
        }

        /// <summary>
        /// To Delete Financial Assessment Detail
        /// </summary>
        /// <param name="financialAssessmentDetailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<bool> DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn)
        {
            return financialAssessmentService.DeleteFinancialAssessmentDetail(financialAssessmentDetailID, modifiedOn);
        }

    }
}
