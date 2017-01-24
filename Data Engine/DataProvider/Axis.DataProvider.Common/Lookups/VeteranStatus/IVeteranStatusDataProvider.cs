using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IVeteranStatusDataProvider
    {
        Response<VeteranStatusModel> GetVeteranStatuses();
    }
}