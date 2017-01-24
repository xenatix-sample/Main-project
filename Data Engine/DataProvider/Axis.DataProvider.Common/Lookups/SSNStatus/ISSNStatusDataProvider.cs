using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ISSNStatusDataProvider
    {
        Response<SSNStatusModel> GetSSNStatuses();
    }
}