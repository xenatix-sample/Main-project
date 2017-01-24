using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PhonePermissionDataProvider : IPhonePermissionDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public PhonePermissionDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<PhonePermissionModel> GetPhonePermissions()
        {
            var repository = _unitOfWork.GetRepository<PhonePermissionModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetPhonePermissionDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
