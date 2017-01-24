using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public interface IScheduleTypeDataProvider
    {
        Response<ScheduleTypeModel> GetScheduleType();
    }
}
