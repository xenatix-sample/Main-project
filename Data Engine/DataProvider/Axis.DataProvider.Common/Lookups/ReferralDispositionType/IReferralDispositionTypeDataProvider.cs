using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReferralDispositionTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the referral.
        /// </summary>
        /// <returns></returns>
        Response<ReferralDispositionTypeModel> GetReferralDispositionType();
    }
}
