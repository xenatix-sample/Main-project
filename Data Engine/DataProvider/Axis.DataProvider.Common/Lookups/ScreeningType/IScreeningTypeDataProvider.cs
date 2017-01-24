using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IScreeningTypeDataProvider
    {
        Response<ScreeningTypeModel> GetScreeningTypes();
    }
}