using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EthnicityDataProvider : IEthnicityDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public EthnicityDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<EthnicityModel> GetEthnicities()
        {
            var repository = _unitOfWork.GetRepository<EthnicityModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetEthnicityDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
