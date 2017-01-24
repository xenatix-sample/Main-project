using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.Repository.FinancialAssessment
{    
    public interface IFinancialSummaryRepository
    {
        /// <summary>
        /// Gets the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<FinancialSummaryModel> GetFinancialSummaryById(long financialSummaryID);

        /// <summary>
        /// Gets the financial summary.
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
    }
}