using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Repository.Referral;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class ReferralAdditionalDetailController : BaseApiController
    {
        /// <summary>
        /// The referral followup repository
        /// </summary>
        private readonly IReferralAdditionalDetailRepository _referralRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAdditionalDetailController" /> class.
        /// </summary>
        /// <param name="referralRepository">The referral  repository.</param>
        public ReferralAdditionalDetailController(IReferralAdditionalDetailRepository referralRepository)
        {
            this._referralRepository = referralRepository;
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ReferralAdditionalDetailViewModel>> GetReferralAdditionalDetail(long contactID)
        {
            var model = await _referralRepository.GetReferralAdditionalDetail(contactID);
            return model;
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ReferralDetailsViewModel>> GetReferralsDetails(long contactID)
        {
            var model = await _referralRepository.GetReferralsDetails(contactID);
            return model;
        }

        /// <summary>
        /// Adds the referral  detail.
        /// </summary>
        /// <param name="referral">The referral .</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralAdditionalDetailViewModel> AddReferralAdditionalDetail(ReferralAdditionalDetailViewModel referral)
        {
            return _referralRepository.AddReferralAdditionalDetail(referral);
        }

        /// <summary>
        /// Updates the referral .
        /// </summary>
        /// <param name="referral">The referral .</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralAdditionalDetailViewModel> UpdateReferralAdditionalDetail(ReferralAdditionalDetailViewModel referral)
        {
            return _referralRepository.UpdateReferralAdditionalDetail(referral);
        }

        /// <summary>
        /// Deletes the referral details.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ReferralDetailsViewModel> DeleteReferralDetails(long contactID)
        {
            return _referralRepository.DeleteReferralDetails(contactID);
        }
    }
}
