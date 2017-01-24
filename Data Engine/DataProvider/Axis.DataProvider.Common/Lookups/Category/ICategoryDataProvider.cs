using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICategoryDataProvider
    {
        Response<FinanceCategoryModel> GetCategories();
        Response<FinanceCategoryTypeModel> GetCategoriesType();
    }
}