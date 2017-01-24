using Axis.Model.Common;
using Axis.Model.Common.Lookups.GroupType;

namespace Axis.DataProvider.Common.Lookups.GroupType
{
    public interface IGroupTypeDataProvider
    {
        Response<GroupTypeModel> GetGroupType();
    }
}
