using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ProgramClientIdentifierDataProvider : IProgramClientIdentifierDataProvider
    {
         #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ProgramClientIdentifierDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ProgramClientIdentifierModel> GetProgramClientIdentifiers()
        {
            var repository = _unitOfWork.GetRepository<ProgramClientIdentifierModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetProgramClientIdentifiers");

            return results;
        }

        #endregion exposed functionality
    }

    
}
