using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public interface IMonthDataProvider
    {
        Response<MonthModel> GetMonth();
    }
}
