using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.FamilyRelationship;

namespace Axis.DataProvider.Common.Lookups.FamilyRelationship
{
    public class FamilyRelationshipDataProvider : IFamilyRelationshipDataProvider
    {
         #region initializations

        readonly IUnitOfWork unitOfWork;

        public FamilyRelationshipDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<FamilyRelationshipModel> GetFamilyRelationships()
        {
            var repository = unitOfWork.GetRepository<FamilyRelationshipModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetMedicalHistoryRelationshipTypes");

            return results;
        }

        #endregion exposed functionality
    }
}
