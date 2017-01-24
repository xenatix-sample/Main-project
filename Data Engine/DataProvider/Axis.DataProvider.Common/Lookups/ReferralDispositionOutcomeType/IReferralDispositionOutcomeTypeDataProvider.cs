using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReferralDispositionOutcomeTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the referral disposition outcome.
        /// </summary>
        /// <returns></returns>
        Response<ReferralDispositionOutcomeTypeModel> GetReferralDispositionOutcomeType();
    }
}
