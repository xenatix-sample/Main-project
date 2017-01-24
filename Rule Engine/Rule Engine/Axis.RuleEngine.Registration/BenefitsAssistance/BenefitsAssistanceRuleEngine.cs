using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;

namespace Axis.RuleEngine.Registration
{
    public class BenefitsAssistanceRuleEngine : IBenefitsAssistanceRuleEngine
    {
        private readonly IBenefitsAssistanceService benefitsAssistanceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BenefitsAssistanceRuleEngine"/> class.
        /// </summary>
        /// <param name="benefitsAssistanceService">The benefits assistance service.</param>
        public BenefitsAssistanceRuleEngine(IBenefitsAssistanceService benefitsAssistanceService)
        {
            this.benefitsAssistanceService = benefitsAssistanceService;
        }

        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> GetBenefitsAssistance(long benefitsAssistanceID)
        {
            return benefitsAssistanceService.GetBenefitsAssistance(benefitsAssistanceID);
        }

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> GetBenefitsAssistanceByContactID(long contactID)
        {
            return benefitsAssistanceService.GetBenefitsAssistanceByContactID(contactID);
        }

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> AddBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            return benefitsAssistanceService.AddBenefitsAssistance(benefitsAssistanceModel);
        }

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> UpdateBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            return benefitsAssistanceService.UpdateBenefitsAssistance(benefitsAssistanceModel);
        }

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn)
        {
            return benefitsAssistanceService.DeleteBenefitsAssistance(benefitsAssistanceID, modifiedOn);
        }
    }
}
