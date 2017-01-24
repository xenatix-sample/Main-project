using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public interface IWeekOfMonthDataProvider
    {
        Response<WeekOfMonthModel> GetWeekOfMonth();
    }
}
