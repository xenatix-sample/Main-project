using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReferralResourceTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the referral resource.
        /// </summary>
        /// <returns></returns>
        Response<ReferralResourceTypeModel> GetReferralResourceType();
    }
}