using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Data.Repository;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class UsersDataProvider : IUsersDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public UsersDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<UsersModel> GetUsers()
        {
            var repository = _unitOfWork.GetRepository<UsersModel>();
            var results = repository.ExecuteStoredProc("usp_GetUsersLookup");

            return results;
        }

        #endregion exposed functionality
    }
}