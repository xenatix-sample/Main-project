using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReferralTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the referral.
        /// </summary>
        /// <returns></returns>
        Response<ReferralTypeModel> GetReferralType();
    }
}