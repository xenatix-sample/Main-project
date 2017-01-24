using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ISmokingStatusDataProvider
    {
        Response<SmokingStatusModel> GetSmokingStatuses();
    }
}