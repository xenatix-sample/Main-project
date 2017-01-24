using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class PresentingProblemTypeDataProvider : IPresentingProblemTypeDataProvider
    {
         #region initializations

        readonly IUnitOfWork _unitOfWork;

        public PresentingProblemTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<PresentingProblemTypeModel> GetPresentingProblemTypes()
        {
            var repository = _unitOfWork.GetRepository<PresentingProblemTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetPresentingProblemTypeDetails");

            return results;
        }

        #endregion exposed functionality
    }
}
