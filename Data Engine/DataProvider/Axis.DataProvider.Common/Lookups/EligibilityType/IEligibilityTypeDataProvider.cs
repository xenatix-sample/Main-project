using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IEligibilityTypeDataProvider
    {
        Response<EligibilityTypeModel> GetEligibilityTypes();
    }
}
