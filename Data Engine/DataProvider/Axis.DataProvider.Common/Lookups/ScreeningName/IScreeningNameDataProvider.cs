using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IScreeningNameDataProvider
    {
        Response<ScreeningNameModel> GetScreeningName();
    }
}