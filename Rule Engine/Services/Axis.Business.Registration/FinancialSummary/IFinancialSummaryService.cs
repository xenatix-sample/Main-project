using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Service.Registration
{
    public interface IFinancialSummaryService
    {
        /// <summary>
        /// Gets the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> GetFinancialSummaryById(long financialSummaryID);

        /// <summary>
        /// Get all the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> GetFinancialSummaries(long contactID);

        /// <summary>
        /// Adds the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> AddFinancialSummary(FinancialSummaryModel financialSummary);

        /// <summary>
        /// Updates the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> UpdateFinancialSummary(FinancialSummaryModel financialSummary);
    }
}
