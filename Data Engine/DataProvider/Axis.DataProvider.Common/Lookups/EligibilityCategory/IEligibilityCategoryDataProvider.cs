using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IEligibilityCategoryDataProvider
    {
        Response<EligibilityCategoryModel> GetEligibilityCategories();
    }
}
