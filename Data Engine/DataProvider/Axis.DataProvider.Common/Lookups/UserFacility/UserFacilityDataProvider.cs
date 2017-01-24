using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Data.Repository;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class UserFacilityDataProvider : IUserFacilityDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public UserFacilityDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<UserFacilityModel> GetUserFacility()
        {
            var repository = _unitOfWork.GetRepository<UserFacilityModel>();
            var FacilityIDParam = new SqlParameter("FacilityID", 1);
            var procParams = new List<SqlParameter>() { FacilityIDParam };
            var results = repository.ExecuteStoredProc("usp_GetUserFacility", procParams);

            return results;
        }

        #endregion exposed functionality
    }
}