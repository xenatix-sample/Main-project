using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Financial Assessment Service class to call the web api methods
    /// </summary>
    public class FinancialAssessmentService : IFinancialAssessmentService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "financialAssessment/";

        public FinancialAssessmentService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// To get the financial assessment details (income or expenses) for contact id
        /// </summary>
        /// <param name="contactID">client contact id</param>
        /// <param name="financialAssessmentID">financialAssessmentID</param>
        /// <returns>Response type Financial assessment</returns>
        public Response<FinancialAssessmentModel> GetFinancialAssessment(long contactID, long financialAssessmentID)
        {
            string apiUrl = BaseRoute + "GetFinancialAssessment";
            var requestXMLValueNvc = new NameValueCollection();
            requestXMLValueNvc.Add("contactID", contactID.ToString(CultureInfo.InvariantCulture));
            requestXMLValueNvc.Add("financialAssessmentID", financialAssessmentID.ToString(CultureInfo.InvariantCulture));
            return communicationManager.Get<Response<FinancialAssessmentModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// To Add the financial assessment for contact id of FinancialAssessmentModel
        /// </summary>
        /// <param name="financialAssessment">New financial Assessment model</param>
        /// <returns>Response type Financial assessment</returns>
        public Response<FinancialAssessmentModel> AddFinancialAssessment(FinancialAssessmentModel financialAssessment)
        {
            string apiUrl = BaseRoute + "AddFinancialAssessment";
            return communicationManager.Post<FinancialAssessmentModel, Response<FinancialAssessmentModel>>(financialAssessment, apiUrl);
        }

        /// <summary>
        /// To update the financial assessment for contact id  of FinancialAssessmentModel
        /// </summary>
        /// <param name="financialAssessment">Updated financial Assessment model</param>
        /// <returns>Response type Financial assessment</returns>
        public Response<FinancialAssessmentModel> UpdateFinancialAssessment(FinancialAssessmentModel financialAssessment)
        {
            string apiUrl = BaseRoute + "UpdateFinancialAssessment";
            return communicationManager.Post<FinancialAssessmentModel, Response<FinancialAssessmentModel>>(financialAssessment, apiUrl);
        }

        /// <summary>
        /// To Add the financial assessment details (income or expenses) for contact id of FinancialAssessmentModel
        /// </summary>
        /// <param name="financialAssessmentDetail">New financial Assessment detail model</param>
        /// <returns>Response type Financial assessment detail</returns>
        public Response<FinancialAssessmentDetailsModel> AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            string apiUrl = BaseRoute + "AddFinancialAssessmentDetails";
            return communicationManager.Post<FinancialAssessmentDetailsModel, Response<FinancialAssessmentDetailsModel>>(financialAssessmentDetail, apiUrl);
        }

        /// <summary>
        /// To update the financial assessment details (income or expenses) for contact id  of FinancialAssessmentModel
        /// </summary>
        /// <param name="financialAssessmentDetail">Updated financial Assessment detail model</param>
        /// <returns>Response type Financial assessment detail</returns>
        public Response<FinancialAssessmentDetailsModel> UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            string apiUrl = BaseRoute + "UpdateFinancialAssessmentDetails";
            return communicationManager.Post<FinancialAssessmentDetailsModel, Response<FinancialAssessmentDetailsModel>>(financialAssessmentDetail, apiUrl);
        }


        /// <summary>
        /// Deletes the  Financial Assessment Detail.
        /// </summary>
        /// <param name="financialAssessmentDetailID">financialAssessmentDetailID</param>
        /// <returns></returns>
        public Response<bool> DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "DeleteFinancialAssessmentDetail";
            var requestXMLValueNvc = new NameValueCollection
            {
                {"financialAssessmentDetailID", financialAssessmentDetailID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<bool>>(requestXMLValueNvc, apiUrl);
            
            
        }

    }
}
