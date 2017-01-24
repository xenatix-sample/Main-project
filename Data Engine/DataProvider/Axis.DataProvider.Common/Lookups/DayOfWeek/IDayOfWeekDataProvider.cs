using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public interface IDayOfWeekDataProvider
    {
        Response<DayOfWeekModel> GetDayOfWeek();
    }
}
