using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// Rule engine class for calling service method of financial Assessment Service
    /// </summary>
    public class FinancialSummaryRuleEngine : IFinancialSummaryRuleEngine
    {
        private readonly IFinancialSummaryService financialSummaryService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="financialSummaryService">The financial summary service.</param>
        public FinancialSummaryRuleEngine(IFinancialSummaryService financialSummaryService)
        {
            this.financialSummaryService = financialSummaryService;
        }

        /// <summary>
        /// Gets the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> GetFinancialSummaryById(long financialSummaryID)
        {
            return financialSummaryService.GetFinancialSummaryById(financialSummaryID);
        }

        /// <summary>
        /// Get all the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> GetFinancialSummaries(long contactID)
        {
            return financialSummaryService.GetFinancialSummaries(contactID);
        }

        /// <summary>
        /// Adds the financial summary.
        /// </summary>
        /// <param name="financialAssessment">The financial assessment.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> AddFinancialSummary(FinancialSummaryModel financialAssessment)
        {
            return financialSummaryService.AddFinancialSummary(financialAssessment);
        }

        /// <summary>
        /// Updates the financial summary.
        /// </summary>
        /// <param name="financialAssessment">The financial assessment.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> UpdateFinancialSummary(FinancialSummaryModel financialAssessment)
        {
            return financialSummaryService.UpdateFinancialSummary(financialAssessment);
        }
    }
}