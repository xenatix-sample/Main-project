using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IReferralStatusDataProvider
    {
        /// <summary>
        /// Gets the referral statuses.
        /// </summary>
        /// <returns></returns>
        Response<ReferralStatusModel> GetReferralStatuses();
    }
}