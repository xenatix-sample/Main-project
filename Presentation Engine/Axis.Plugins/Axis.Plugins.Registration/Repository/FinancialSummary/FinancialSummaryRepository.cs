using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using System.Collections.Specialized;


namespace Axis.Plugins.Registration.Repository.FinancialAssessment
{
    public class FinancialSummaryRepository : IFinancialSummaryRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "financialSummary/";

        public FinancialSummaryRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public FinancialSummaryRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
       
        public Response<FinancialSummaryModel> GetFinancialSummaryById(long financialSummaryID)
        {
            string apiUrl = baseRoute + "GetFinancialSummaryById";
            var param = new NameValueCollection();
            param.Add("financialSummaryID", financialSummaryID.ToString());
            return communicationManager.Get<Response<FinancialSummaryModel>>(param, apiUrl);
        }

        /// <summary>
        /// Get all the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
     
        public Response<FinancialSummaryModel> GetFinancialSummaries(long contactID)
        {
            string apiUrl = baseRoute + "GetFinancialSummaries";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            return communicationManager.Get<Response<FinancialSummaryModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
       
        public Response<FinancialSummaryModel> AddFinancialSummary(FinancialSummaryModel financialSummary)
        {
            string apiUrl = baseRoute + "AddFinancialSummary";
            return communicationManager.Post<FinancialSummaryModel, Response<FinancialSummaryModel>>(financialSummary, apiUrl);
        }

        /// <summary>
        /// Updates the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial summary.</param>
        /// <returns></returns>
        
        public Response<FinancialSummaryModel> UpdateFinancialSummary(FinancialSummaryModel financialSummary)
        {
            string apiUrl = baseRoute + "UpdateFinancialSummary";
            return communicationManager.Put<FinancialSummaryModel, Response<FinancialSummaryModel>>(financialSummary, apiUrl);
        }
    }
}