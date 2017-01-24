using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IScreeningResultDataProvider
    {
        Response<ScreeningResultModel> GetScreeningResults();
    }
}