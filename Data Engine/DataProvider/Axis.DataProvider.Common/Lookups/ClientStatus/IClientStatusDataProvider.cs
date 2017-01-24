using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IClientStatusDataProvider
    {
        Response<ClientStatusModel> GetClientStatus();
    }
}
