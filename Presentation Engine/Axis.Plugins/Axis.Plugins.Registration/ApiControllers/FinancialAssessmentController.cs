using System;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository.FinancialAssessment;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Controller for Financial Assessment screen
    /// </summary>
    public class FinancialAssessmentController : BaseApiController
    {
        private readonly IFinancialAssessmentRepository financialAssessmentRepository;

        public FinancialAssessmentController(IFinancialAssessmentRepository financialAssessmentRepository)
        {
            this.financialAssessmentRepository = financialAssessmentRepository;
        }

        /// <summary>
        /// To get the financial assessment of contact id 
        /// </summary>
        /// <param name="contactID">contact ID</param>
        /// <param name="financialAssessmentID">financialAssessment ID</param>
        /// <returns>json data of financial assesment model</returns>
        [HttpGet]
        public async Task<Response<FinancialAssessmentViewModel>> GetFinancialAssessment(long contactID, long financialAssessmentID)
        {
            var result = await financialAssessmentRepository.GetFinancialAssessmentAsync(contactID, financialAssessmentID);
            return result;
        }

        /// <summary>
        /// To get the financial assessment details of contact id 
        /// </summary>
        /// <param name="contactID">contact ID</param>
        /// <param name="financialAssessmentID">financialAssessment ID</param>
        /// <returns>json data of financial assesment model</returns>
        [HttpGet]
        public async Task<Response<FinancialAssessmentDetailsModel>> GetFinancialAssessmentDetails(long contactID, long financialAssessmentID)
        {
            var result = await financialAssessmentRepository.GetFinancialAssessmentDetailsAsync(contactID, financialAssessmentID);
            return result;
        }

        /// <summary>
        /// To add the financial assessment for contact id 
        /// </summary>
        /// <returns>json data of financial assesment model</returns>
        [HttpPost]
        public Response<FinancialAssessmentViewModel> AddFinancialAssessment(FinancialAssessmentViewModel financialAssessment)
        {
            return financialAssessmentRepository.AddFinancialAssessment(financialAssessment);
        }

        /// <summary>
        /// To update the financial assessment for contact id 
        /// </summary>
        /// <returns>json data of financial assesment model</returns>
        [HttpPut]
        public Response<FinancialAssessmentViewModel> UpdateFinancialAssessment(FinancialAssessmentViewModel financialAssessment)
        {
            return financialAssessmentRepository.UpdateFinancialAssessment(financialAssessment);
        }

        /// <summary>
        /// To add the financial assessment details for contact id 
        /// </summary>
        /// <returns>json data of financial assesment model</returns>
        [HttpPost]
        public Response<FinancialAssessmentDetailsModel> AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            return financialAssessmentRepository.AddFinancialAssessmentDetails(financialAssessmentDetail);
        }

        /// <summary>
        /// To update the financial assessment details for contact id 
        /// </summary>
        /// <returns>json data of financial assesment model</returns>
        [HttpPut]
        public Response<FinancialAssessmentDetailsModel> UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetail)
        {
            return financialAssessmentRepository.UpdateFinancialAssessmentDetails(financialAssessmentDetail);
        }

        /// <summary>
        /// Delete Method to soft delete the  Financial Assessment Detail data
        /// </summary>
        /// <param name="financialAssessmentDetailID">financialAssessmentDetailID</param>
        /// <returns>JsonResult</returns>
        [HttpDelete]
        public Response<bool> DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return financialAssessmentRepository.DeleteFinancialAssessmentDetail(financialAssessmentDetailID, modifiedOn);
        }
    }
}
