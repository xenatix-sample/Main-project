using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IBehavioralCategoryDataProvider
    {
        Response<BehavioralCategoryModel> GetBehavioralCategories();
    }
}
