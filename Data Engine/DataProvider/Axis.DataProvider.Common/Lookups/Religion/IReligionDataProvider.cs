using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReligionDataProvider
    {
        Response<ReligionModel> GetReligions();
    }
}