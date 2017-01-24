using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// interface for Financial Summary Rule Engine
    /// </summary>
    public interface IFinancialSummaryRuleEngine
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
