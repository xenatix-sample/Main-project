using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReferralPriorityDataProvider
    {
        Response<ReferralPriorityModel> GetReferralPriorities();
    }
}
