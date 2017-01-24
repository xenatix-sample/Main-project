using System;
using Axis.Plugins.Registration.Repository.Referrals;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.ApiControllers.Referrals
{
    /// <summary>
    /// This class is use to perform action for referral search screen
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class ReferralSearchController : BaseApiController
    {
        /// <summary>
        /// The _referral search repository
        /// </summary>
        private readonly IReferralSearchRepository _referralSearchRepository = null;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSearchController"/> class.
        /// </summary>
        /// <param name="referralSearchRepository">The referral search repository.</param>
        public ReferralSearchController(IReferralSearchRepository referralSearchRepository)
        {
            _referralSearchRepository = referralSearchRepository;
        }

        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ReferralSearchViewModel>> GetReferrals(string searchStr, int searchType, long userID)
        {
            searchStr = searchStr != null ? searchStr : string.Empty;
            var result = await _referralSearchRepository.GetReferralsAsync(searchStr, searchType, userID);
            return result;
        }

        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ReferralSearchViewModel> DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _referralSearchRepository.DeleteReferral(id, reasonForDelete, modifiedOn);
        }
    }
}
