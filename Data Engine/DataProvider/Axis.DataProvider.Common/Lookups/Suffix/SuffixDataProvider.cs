using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SuffixDataProvider : ISuffixDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public SuffixDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<SuffixModel> GetSuffixes()
        {
            var repository = _unitOfWork.GetRepository<SuffixModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetSuffixDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
