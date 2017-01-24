using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for Financial Assessment screen
    /// </summary>
    public class FinancialAssessmentController : BaseApiController
    {
         readonly IFinancialAssessmentDataProvider financialAssessmentDataProvider;

         public FinancialAssessmentController(IFinancialAssessmentDataProvider financialAssessmentDataProvider)
        {
            this.financialAssessmentDataProvider = financialAssessmentDataProvider;
        }

         /// <summary>
         /// To get the financial assessment details of contact id 
         /// </summary>
         /// <param name="contactID">contact id for which financial assessment data will fetch</param> 
         /// <param name="financialAssessmentID">financialAssessmentID</param>
         /// <returns>IHttpActionResult of financial assesment model type</returns>
         [HttpGet]
         public IHttpActionResult GetFinancialAssessment(long contactID, long financialAssessmentID)
         {
             return new HttpResult<Response<FinancialAssessmentModel>>(financialAssessmentDataProvider.GetFinancialAssessment(contactID,financialAssessmentID), Request);
         }

         /// <summary>
         /// To add the financial assessment for contact id 
         /// </summary>
         /// <param name="financialAssessment">model of FinancialAssessment </param>
         /// <returns>IHttpActionResult of financial assesment model type</returns>
        [HttpPost]
         public IHttpActionResult  AddFinancialAssessment(FinancialAssessmentModel financialAssessment)
         {
             return new HttpResult<Response<FinancialAssessmentModel>>(financialAssessmentDataProvider.AddFinancialAssessment(financialAssessment), Request);
         }

        /// <summary>
        /// To update the financial assessment for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>IHttpActionResult of financial assesment model type</returns>
        
        [HttpPost]
         public IHttpActionResult UpdateFinancialAssessment(FinancialAssessmentModel financialAssessment)
         {
             return new HttpResult<Response<FinancialAssessmentModel>>(financialAssessmentDataProvider.UpdateFinancialAssessment(financialAssessment), Request);
         }

        /// <summary>
        /// To add the financial assessment details for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment Details</param>
        /// <returns>IHttpActionResult of financial assesment model type</returns>
        [HttpPost]
        public IHttpActionResult AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            return new HttpResult<Response<FinancialAssessmentDetailsModel>>(financialAssessmentDataProvider.AddFinancialAssessmentDetails(financialAssessmentDetail), Request);
        }

        /// <summary>
        /// To update the financial assessment details for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment Details </param>
        /// <returns>IHttpActionResult of financial assesment model type</returns>

        [HttpPost]
        public IHttpActionResult UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            return new HttpResult<Response<FinancialAssessmentDetailsModel>>(financialAssessmentDataProvider.UpdateFinancialAssessmentDetails(financialAssessmentDetail), Request);
        }

        /// <summary>
        /// To delete the financial assessment details 
        /// </summary>
        /// <param name="financialAssessmentDetailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn)
        {
            return new HttpResult<Response<bool>>(financialAssessmentDataProvider.DeleteFinancialAssessmentDetail(financialAssessmentDetailID, modifiedOn), Request);
        }

    }
}
