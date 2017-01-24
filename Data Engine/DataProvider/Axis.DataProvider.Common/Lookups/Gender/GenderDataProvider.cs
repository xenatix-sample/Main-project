using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GenderDataProvider : IGenderDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public GenderDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<GenderModel> GetGenders()
        {
            var repository = _unitOfWork.GetRepository<GenderModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetGenderDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
