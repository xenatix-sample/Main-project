using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReferralConcernTypeDataProvider
    {
        Response<ReferralConcernTypeModel> GetReferralConcerns();
        Response<ReferralConcernTypeModel> GetReferralProblems();
    }
}
