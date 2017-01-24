using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IDOBStatusDataProvider
    {
        Response<DOBStatusModel> GetDOBStatuses();
    }
}