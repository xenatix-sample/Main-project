using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IEntityTypeDataProvider
    {
        Response<EntityTypeModel> GetEntityType();
    }
}