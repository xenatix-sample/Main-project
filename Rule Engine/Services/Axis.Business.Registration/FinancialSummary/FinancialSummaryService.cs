using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class FinancialSummaryService : IFinancialSummaryService
    {
        private readonly CommunicationManager communicationManager;

        private const string BaseRoute = "financialSummary/";

        public FinancialSummaryService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> GetFinancialSummaryById(long financialSummaryID)
        {
            string apiUrl = BaseRoute + "GetFinancialSummaryById";
            var request = new NameValueCollection();
            request.Add("FinancialSummaryID", financialSummaryID.ToString(CultureInfo.InvariantCulture));
            return communicationManager.Get<Response<FinancialSummaryModel>>(request, apiUrl);
        }

        /// <summary>
        /// Get all the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> GetFinancialSummaries(long contactID)
        {
            string apiUrl = BaseRoute + "GetFinancialSummaries";
            var request = new NameValueCollection();
            request.Add("contactID", contactID.ToString(CultureInfo.InvariantCulture));
            return communicationManager.Get<Response<FinancialSummaryModel>>(request, apiUrl);
        }

        /// <summary>
        /// Adds the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial assessment.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> AddFinancialSummary(FinancialSummaryModel financialSummary)
        {
            string apiUrl = BaseRoute + "AddFinancialSummary";
            return communicationManager.Post<FinancialSummaryModel, Response<FinancialSummaryModel>>(financialSummary, apiUrl);
        }

        /// <summary>
        /// Updates the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial assessment.</param>
        /// <returns></returns>
        public Response<FinancialSummaryModel> UpdateFinancialSummary(FinancialSummaryModel financialSummary)
        {
            string apiUrl = BaseRoute + "UpdateFinancialSummary";
            return communicationManager.Put<FinancialSummaryModel, Response<FinancialSummaryModel>>(financialSummary, apiUrl);
        }
    }
}