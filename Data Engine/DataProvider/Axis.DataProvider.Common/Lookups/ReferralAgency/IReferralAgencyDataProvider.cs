using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReferralAgencyDataProvider
    {
        Response<ReferralAgencyModel> GetReferralAgency();
    }
}
