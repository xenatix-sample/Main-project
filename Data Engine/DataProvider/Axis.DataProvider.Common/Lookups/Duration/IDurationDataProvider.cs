using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IDurationDataProvider
    {
        Response<DurationModel> GetQuarterDurations();
    }
}
