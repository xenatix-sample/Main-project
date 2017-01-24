using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    public class BenefitsAssistanceController : BaseApiController
    {
        private readonly IBenefitsAssistanceRepository benefitsAssistanceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BenefitsAssistanceController"/> class.
        /// </summary>
        /// <param name="benefitsAssistanceRepository">The benefits assistance repository.</param>
        public BenefitsAssistanceController(IBenefitsAssistanceRepository benefitsAssistanceRepository)
        {
            this.benefitsAssistanceRepository = benefitsAssistanceRepository;
        }

        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<BenefitsAssistanceViewModel> GetBenefitsAssistance(long benefitsAssistanceID)
        {
            return benefitsAssistanceRepository.GetBenefitsAssistance(benefitsAssistanceID);
        }

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<BenefitsAssistanceViewModel> GetBenefitsAssistanceByContactID(long contactID)
        {
            return benefitsAssistanceRepository.GetBenefitsAssistanceByContactID(contactID);
        }

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistance">The benefits assistance.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<BenefitsAssistanceViewModel> AddBenefitsAssistance(BenefitsAssistanceViewModel benefitsAssistance)
        {
            return benefitsAssistanceRepository.AddBenefitsAssistance(benefitsAssistance);
        }

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistance">The benefits assistance.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<BenefitsAssistanceViewModel> UpdateBenefitsAssistance(BenefitsAssistanceViewModel benefitsAssistance)
        {
            return benefitsAssistanceRepository.UpdateBenefitsAssistance(benefitsAssistance);
        }

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<BenefitsAssistanceViewModel> DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn)
        {
            return benefitsAssistanceRepository.DeleteBenefitsAssistance(benefitsAssistanceID, modifiedOn);
        }
    }
}
