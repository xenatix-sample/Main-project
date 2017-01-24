using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Repository.FinancialAssessment;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Controller for Financial Summary screen
    /// </summary>
    public class FinancialSummaryController : BaseApiController
    {
        private readonly IFinancialSummaryRepository financialSummaryRepository;

        public FinancialSummaryController(IFinancialSummaryRepository financialSummaryRepository)
        {
            this.financialSummaryRepository = financialSummaryRepository;
        }

        /// <summary>
        /// Gets the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<FinancialSummaryModel> GetFinancialSummaryById(long financialSummaryID)
        {
            return financialSummaryRepository.GetFinancialSummaryById(financialSummaryID);
        }

        /// <summary>
        /// Get all the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<FinancialSummaryModel> GetFinancialSummaries(long contactID)
        {
            return financialSummaryRepository.GetFinancialSummaries(contactID);
        }

        /// <summary>
        /// Adds the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<FinancialSummaryModel> AddFinancialSummary(FinancialSummaryModel financialSummary)
        {
            return financialSummaryRepository.AddFinancialSummary(financialSummary);
        }

        /// <summary>
        /// Updates the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<FinancialSummaryModel> UpdateFinancialSummary(FinancialSummaryModel financialSummary)
        {
            return financialSummaryRepository.UpdateFinancialSummary(financialSummary);
        }
    }
}