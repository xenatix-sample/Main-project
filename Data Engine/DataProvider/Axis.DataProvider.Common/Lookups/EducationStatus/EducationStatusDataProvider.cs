using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EducationStatusDataProvider : IEducationStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public EducationStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<EducationStatusModel> GetEducationStatuses()
        {
            var repository = _unitOfWork.GetRepository<EducationStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetEducationStatusDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
