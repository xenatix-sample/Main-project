using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IRelationshipTypeDataProvider
    {
        Response<RelationshipTypeModel> GetRelationshipTypeDetails();     
    }
}
