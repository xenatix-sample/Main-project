using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class ReferralSearchController : BaseApiController
    {
        /// <summary>
        /// The referral search data provider
        /// </summary>
        private readonly IReferralSearchDataProvider _referralSearchDataProvider;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="referralSearchDataProvider"></param>
        public ReferralSearchController(IReferralSearchDataProvider referralSearchDataProvider)
        {
            this._referralSearchDataProvider = referralSearchDataProvider;
        }

        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferrals(string searchStr, int searchType, long userID)
        {
            return new HttpResult<Response<ReferralSearchModel>>(_referralSearchDataProvider.GetReferrals(searchStr, searchType, userID), Request);
        }

        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reasonForDelete"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralSearchModel>>(_referralSearchDataProvider.DeleteReferral(id, reasonForDelete, modifiedOn), Request);
        }
    }
}
