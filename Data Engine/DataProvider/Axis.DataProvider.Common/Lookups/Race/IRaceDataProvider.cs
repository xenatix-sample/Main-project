using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IRaceDataProvider
    {
        Response<RaceModel> GetRaces();
    }
}