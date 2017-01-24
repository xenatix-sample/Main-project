using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IEthnicityDataProvider
    {
        Response<EthnicityModel> GetEthnicities();
    }
}