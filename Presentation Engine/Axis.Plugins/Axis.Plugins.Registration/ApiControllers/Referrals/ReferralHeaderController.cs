using System;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Requestor;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals
{
    public class ReferralHeaderController : BaseApiController
    {
        /// <summary>
        /// The referral followup repository
        /// </summary>
        private readonly IReferralHeaderRepository referralHeaderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralHeaderController" /> class.
        /// </summary>
        /// <param name="referralHeaderRepository">The referral header repository.</param>
        public ReferralHeaderController(IReferralHeaderRepository referralHeaderRepository)
        {
            this.referralHeaderRepository = referralHeaderRepository;
        }

        /// <summary>
        /// Referral Header view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralHeaderViewModel> GetReferralHeader(long referralHeaderID)
        {
            return referralHeaderRepository.GetReferralHeader(referralHeaderID);
            
        }

        /// <summary>
        /// Adds the referral Header detail.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralHeaderViewModel> AddReferralHeader(ReferralHeaderViewModel referralHeader)
        {
            return referralHeaderRepository.AddReferralHeader(referralHeader);
        }

        /// <summary>
        /// Updates the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralHeaderViewModel> UpdateReferralHeader(ReferralHeaderViewModel referralHeader)
        {
            return referralHeaderRepository.UpdateReferralHeader(referralHeader);
        }

        /// <summary>
        /// Deletes the referral header.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ReferralHeaderViewModel> DeleteReferralHeader(long referralID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return referralHeaderRepository.DeleteReferralHeader(referralID, modifiedOn);
        }
    }
}