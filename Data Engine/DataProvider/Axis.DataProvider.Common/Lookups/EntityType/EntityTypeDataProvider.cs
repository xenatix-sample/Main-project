using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EntityTypeDataProvider : IEntityTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public EntityTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<EntityTypeModel> GetEntityType()
        {
            var repository = _unitOfWork.GetRepository<EntityTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetEntityType");

            return results;
        }

        #endregion exposed functionality

    }

}
