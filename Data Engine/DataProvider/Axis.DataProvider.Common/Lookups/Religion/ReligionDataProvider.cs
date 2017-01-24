using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReligionDataProvider : IReligionDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ReligionDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ReligionModel> GetReligions()
        {
            var repository = _unitOfWork.GetRepository<ReligionModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReligionDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
