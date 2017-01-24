using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Interface for Financial Summary Data provider
    /// </summary>
    public interface IFinancialSummaryDataProvider
    {
        /// <summary>
        /// Gets the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> GetFinancialSummaryById(long financialSummaryID);

        /// <summary>
        /// Get all the financial summaries.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> GetFinancialSummaries(long contactID);

        /// <summary>
        /// Adds the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial assessment report.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> AddFinancialSummary(FinancialSummaryModel financialSummary);

        /// <summary>
        /// Updates the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial assessment report.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> UpdateFinancialSummary(FinancialSummaryModel financialSummary);

        /// <summary>
        /// Gets the financial summary confirmation statement.
        /// </summary>
        /// <param name="financialSummaryID">The financial summary identifier.</param>
        /// <returns></returns>
        Response<FinancialSummaryConfirmationStatementModel> GetFinancialSummaryConfirmationStatement(long financialSummaryID);

        /// <summary>
        /// Adds the financial summary confirmation statement.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
        Response<FinancialSummaryConfirmationStatementModel> AddFinancialSummaryConfirmationStatement(FinancialSummaryConfirmationStatementModel financialSummaryConfirmationStatement);

        /// <summary>
        /// Updates the financial summary confirmation statement.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
        Response<FinancialSummaryConfirmationStatementModel> UpdateFinancialSummaryConfirmationStatement(FinancialSummaryConfirmationStatementModel financialSummaryConfirmationStatement);
    }
}