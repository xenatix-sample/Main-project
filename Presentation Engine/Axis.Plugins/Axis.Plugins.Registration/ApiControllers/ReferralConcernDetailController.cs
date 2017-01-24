using System;
using System.Web.Http;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository.Referral;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers
{
    public class ReferralConcernDetailController : BaseApiController
    {
        /// <summary>
        /// The referral followup repository
        /// </summary>
        private readonly IReferralConcernDetailRepository _referralConcernDetailRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailController" /> class.
        /// </summary>
        /// <param name="referralConcernDetailRepository">The referral ConcernDetail repository.</param>
        public ReferralConcernDetailController(IReferralConcernDetailRepository referralConcernDetailRepository)
        {
            this._referralConcernDetailRepository = referralConcernDetailRepository;
        }

        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralAdditionalDetailID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralConcernDetailViewModel> GetReferralConcernDetail(long referralAdditionalDetailID)
        {
            return _referralConcernDetailRepository.GetReferralConcernDetail(referralAdditionalDetailID);
        }

        /// <summary>
        /// Adds the referral ConcernDetail detail.
        /// </summary>
        /// <param name="referralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralConcernDetailViewModel> AddReferralConcernDetail(ReferralConcernDetailViewModel referralConcernDetail)
        {
            return _referralConcernDetailRepository.AddReferralConcernDetail(referralConcernDetail);
        }

        /// <summary>
        /// Updates the referral ConcernDetail.
        /// </summary>
        /// <param name="referralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralConcernDetailViewModel> UpdateReferralConcernDetail(ReferralConcernDetailViewModel referralConcernDetail)
        {
            return _referralConcernDetailRepository.UpdateReferralConcernDetail(referralConcernDetail);
        }

        /// <summary>
        /// Deletes the referral ConcernDetail.
        /// </summary>
        /// <param name="referralConcernDetailID">The referral identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ReferralConcernDetailViewModel> DeleteReferralConcernDetail(long referralConcernDetailID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _referralConcernDetailRepository.DeleteReferralConcernDetail(referralConcernDetailID, modifiedOn);
        }
    }
}