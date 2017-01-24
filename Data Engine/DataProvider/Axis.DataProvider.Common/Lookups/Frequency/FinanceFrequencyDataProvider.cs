using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
   /// <summary>
    /// Data provider class for Finance Frequency data
   /// </summary>
    public class FinanceFrequencyDataProvider : IFinanceFrequencyDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public FinanceFrequencyDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<FinanceFrequencyModel> GetFrequencies()
        {
            var repository = _unitOfWork.GetRepository<FinanceFrequencyModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetFinanceFrequencyDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
