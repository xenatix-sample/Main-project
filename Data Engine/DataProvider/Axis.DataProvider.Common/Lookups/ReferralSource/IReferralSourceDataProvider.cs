using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReferralSourceDataProvider
    {
        Response<ReferralSourceModel> GetReferralSources();
    }
}