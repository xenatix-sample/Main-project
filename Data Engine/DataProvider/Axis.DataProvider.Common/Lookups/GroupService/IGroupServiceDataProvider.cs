using Axis.Model.Common;
using Axis.Model.Common.Lookups.GroupService;

namespace Axis.DataProvider.Common.Lookups.GroupService
{
    public interface IGroupServiceDataProvider
    {
        Response<GroupServiceModel> GetGroupService();
    }
}
