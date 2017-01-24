using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace Axis.Plugins.Registration.Repository.FinancialAssessment
{
    /// <summary>
    /// Repository for Financial Assessment to call web api methods.
    /// </summary>
    public class FinancialAssessmentRepository : IFinancialAssessmentRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "financialAssessment/";

        public FinancialAssessmentRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public FinancialAssessmentRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To call the get financial assessment details web api method
        /// </summary>
        /// <param name="contactID">contactID</param>
        /// <param name="financialAssessmentID">financialAssessment ID</param>
        /// <returns>Financial Assessment View Model type</returns>
        public Response<FinancialAssessmentViewModel> GetFinancialAssessment(long contactID, long financialAssessmentID)
        {
            return GetFinancialAssessmentAsync(contactID, financialAssessmentID).Result;
        }

        /// <summary>
        /// To call the get financial assessment details web api method
        /// </summary>
        /// <param name="contactID">contactID</param>
        /// <param name="financialAssessmentID">financialAssessment ID</param>
        /// <returns>Financial Assessment View Model type</returns>
        public async Task<Response<FinancialAssessmentViewModel>> GetFinancialAssessmentAsync(long contactID, long financialAssessmentID)
        {
            string apiUrl = baseRoute + "GetFinancialAssessment";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            param.Add("financialAssessmentID", financialAssessmentID.ToString());
            var response = await communicationManager.GetAsync<Response<FinancialAssessmentModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// To call the get financial assessment details web api method
        /// </summary>
        /// <param name="contactID">contactID</param>
        /// <param name="financialAssessmentID">financialAssessment ID</param>
        /// <returns>Financial Assessment View Model type</returns>
        public async Task<Response<FinancialAssessmentDetailsModel>> GetFinancialAssessmentDetailsAsync(long contactID, long financialAssessmentID)
        {
            string apiUrl = baseRoute + "GetFinancialAssessment";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            param.Add("financialAssessmentID", financialAssessmentID.ToString());
            var response = await communicationManager.GetAsync<Response<FinancialAssessmentModel>>(param, apiUrl);
            var newResponse = response.CloneResponse<FinancialAssessmentDetailsModel>();
            if (response.DataItems.Count == 1)
                newResponse.DataItems = response.DataItems.First().FinancialAssessmentDetails;
            return newResponse;
        }

        /// <summary>
        /// To call the Add financial assessment web api method
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>Financial Assessment View Model type</returns>
        public Response<FinancialAssessmentViewModel> AddFinancialAssessment(FinancialAssessmentViewModel financialAssessment)
        {
            string apiUrl = baseRoute + "AddFinancialAssessment";
            var response = communicationManager.Post<FinancialAssessmentModel, Response<FinancialAssessmentModel>>(financialAssessment.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// To call the Add financial assessment details web api method
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>Financial Assessment View Model type</returns>
        public Response<FinancialAssessmentDetailsModel> AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            string apiUrl = baseRoute + "AddFinancialAssessmentDetails";
            var response = communicationManager.Post<FinancialAssessmentDetailsModel, Response<FinancialAssessmentDetailsModel>>(financialAssessmentDetail, apiUrl);
            return response;
        }

        /// <summary>
        /// To call the update  financial assessment web api method
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>Financial Assessment View Model type</returns>
        public Response<FinancialAssessmentViewModel> UpdateFinancialAssessment(FinancialAssessmentViewModel financialAssessment)
        {
            string apiUrl = baseRoute + "UpdateFinancialAssessment";
            var response = communicationManager.Post<FinancialAssessmentModel, Response<FinancialAssessmentModel>>(financialAssessment.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// To call the update  financial assessment details web api method
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>Financial Assessment View Model type</returns>
        public Response<FinancialAssessmentDetailsModel> UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            string apiUrl = baseRoute + "UpdateFinancialAssessmentDetails";
            var response = communicationManager.Post<FinancialAssessmentDetailsModel, Response<FinancialAssessmentDetailsModel>>(financialAssessmentDetail, apiUrl);
            return response;
        }

        /// <summary>
        /// Delete Financial Assessment Detail
        /// </summary>
        /// <param name="financialAssessmentDetailID">Financial AssessmentDetail ID</param>
        public Response<bool> DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute + "DeleteFinancialAssessmentDetail";
            var param = new NameValueCollection
            {
                {"financialAssessmentDetailID", financialAssessmentDetailID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<bool>>(param, apiUrl);
        }
    }
}