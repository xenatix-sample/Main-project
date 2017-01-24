using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IGenderDataProvider
    {
        Response<GenderModel> GetGenders();
    }
}