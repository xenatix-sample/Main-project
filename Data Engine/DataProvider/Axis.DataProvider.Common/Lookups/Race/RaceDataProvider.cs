using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RaceDataProvider : IRaceDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public RaceDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<RaceModel> GetRaces()
        {
            var repository = _unitOfWork.GetRepository<RaceModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetRaceDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
