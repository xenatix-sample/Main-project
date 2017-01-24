using Axis.Model.Common;
using Axis.Model.Common.Lookups.FamilyRelationship;

namespace Axis.DataProvider.Common.Lookups.FamilyRelationship
{
    public interface IFamilyRelationshipDataProvider
    {
        Response<FamilyRelationshipModel> GetFamilyRelationships();
    }
}
