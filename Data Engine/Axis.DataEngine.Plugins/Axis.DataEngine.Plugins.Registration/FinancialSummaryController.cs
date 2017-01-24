using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class FinancialSummaryController : BaseApiController
    {
        private readonly IFinancialSummaryDataProvider financialAssessmentDataProvider;

        public FinancialSummaryController(IFinancialSummaryDataProvider financialAssessmentDataProvider)
        {
            this.financialAssessmentDataProvider = financialAssessmentDataProvider;
        }

        /// <summary>
        /// Gets the financial summary
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFinancialSummaryById(long financialSummaryID)
        {
            return new HttpResult<Response<FinancialSummaryModel>>(financialAssessmentDataProvider.GetFinancialSummaryById(financialSummaryID), Request);
        }

        /// <summary>
        /// Get all the financial summary
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFinancialSummaries(long contactID)
        {
            return new HttpResult<Response<FinancialSummaryModel>>(financialAssessmentDataProvider.GetFinancialSummaries(contactID), Request);
        }

        /// <summary>
        /// Adds the financial summary
        /// </summary>
        /// <param name="financialSummary">The financial assessment report.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddFinancialSummary(FinancialSummaryModel financialSummary)
        {
            return new HttpResult<Response<FinancialSummaryModel>>(financialAssessmentDataProvider.AddFinancialSummary(financialSummary), Request);
        }

        /// <summary>
        /// Updates the financial summary
        /// </summary>
        /// <param name="financialSummary">The financial assessment report.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateFinancialSummary(FinancialSummaryModel financialSummary)
        {
            return new HttpResult<Response<FinancialSummaryModel>>(financialAssessmentDataProvider.UpdateFinancialSummary(financialSummary), Request);
        }
    }
}