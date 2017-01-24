using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IClientTypeDataProvider
    {
        Response<ClientTypeModel> GetClientTypes();
    }
}