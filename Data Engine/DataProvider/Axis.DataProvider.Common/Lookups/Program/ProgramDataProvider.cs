using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ProgramDataProvider : IProgramDataProvider
    {
         #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ProgramDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ProgramModel> GetProgram()
        {
            var repository = _unitOfWork.GetRepository<ProgramModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetProgram");

            return results;
        }

        #endregion exposed functionality
    }
}
