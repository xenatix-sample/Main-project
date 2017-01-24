using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Models.Referrals;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Repository.Referral
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReferralAdditionalDetailRepository
    {
        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Task<Response<ReferralAdditionalDetailViewModel>> GetReferralAdditionalDetail(long contactID);

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Task<Response<ReferralDetailsViewModel>> GetReferralsDetails(long contactID);

        /// <summary>
        /// Adds the referral .
        /// </summary>
        /// <param name="referral">The referral .</param>
        /// <returns></returns>
        Response<ReferralAdditionalDetailViewModel> AddReferralAdditionalDetail(ReferralAdditionalDetailViewModel referral);

        /// <summary>
        /// Updates the referral .
        /// </summary>
        /// <param name="referral">The referral .</param>
        /// <returns></returns>
        Response<ReferralAdditionalDetailViewModel> UpdateReferralAdditionalDetail(ReferralAdditionalDetailViewModel referral);

        /// <summary>
        /// Deletes the referral details.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ReferralDetailsViewModel> DeleteReferralDetails(long contactID);
    }
}
