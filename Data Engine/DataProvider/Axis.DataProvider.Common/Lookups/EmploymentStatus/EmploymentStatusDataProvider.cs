using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EmploymentStatusDataProvider : IEmploymentStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public EmploymentStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<EmploymentStatusModel> GetEmploymentStatuses()
        {
            var repository = _unitOfWork.GetRepository<EmploymentStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetEmploymentStatusDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
