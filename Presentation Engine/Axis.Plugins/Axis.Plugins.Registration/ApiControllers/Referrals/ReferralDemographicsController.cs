using System;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Requestor;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals
{
    public class ReferralDemographicsController : BaseApiController
    {
        /// <summary>
        /// The referral followup repository
        /// </summary>
        private readonly IReferralDemographicsRepository referralDemographicsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsController" /> class.
        /// </summary>
        /// <param name="referralDemographicsRepository">The referral demographics repository.</param>
        public ReferralDemographicsController(IReferralDemographicsRepository referralDemographicsRepository)
        {
            this.referralDemographicsRepository = referralDemographicsRepository;
        }


        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralDemographicsViewModel> GetReferralDemographics(long referralID)
        {
            return referralDemographicsRepository.GetReferralDemographics(referralID);
        }

        /// <summary>
        /// Adds the referral Demographics detail.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralDemographicsViewModel> AddReferralDemographics(ReferralDemographicsViewModel referralDemographics)
        {
            return referralDemographicsRepository.AddReferralDemographics(referralDemographics);
        }

        /// <summary>
        /// Updates the referral demographics.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralDemographicsViewModel> UpdateReferralDemographics(ReferralDemographicsViewModel referralDemographics)
        {
            return referralDemographicsRepository.UpdateReferralDemographics(referralDemographics);
        }

        /// <summary>
        /// Deletes the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ReferralDemographicsViewModel> DeleteReferralDemographics(long referralID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return referralDemographicsRepository.DeleteReferralDemographics(referralID, modifiedOn);
        }
    }
}