using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReferralOriginDataProvider
    {
        Response<ReferralOriginModel> GetReferralOrigin();
    }
}