using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class UserIdentifierTypeDataProvider : IUserIdentifierTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public UserIdentifierTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<UserIdentifierTypeModel> GetUserIdentifierType()
        {
            var repository = _unitOfWork.GetRepository<UserIdentifierTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetUserIdentifierType");

            return results;
        }

        #endregion exposed functionality
    }
}
