using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LanguageDataProvider : ILanguageDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public LanguageDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<LanguageModel> GetLanguages()
        {
            var repository = _unitOfWork.GetRepository<LanguageModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetLanguageDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
