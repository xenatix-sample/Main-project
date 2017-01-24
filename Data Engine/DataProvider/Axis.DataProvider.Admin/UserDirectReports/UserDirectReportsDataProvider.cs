using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Admin;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Admin.UserDirectReports
{
    public class UserDirectReportsDataProvider : IUserDirectReportsDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDirectReportsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserDirectReportsDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> GetUsers(int userID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("UserID", userID) };
            var repository = _unitOfWork.GetRepository<UserDirectReportsModel>(SchemaName.Core);
            return repository.ExecuteStoredProc("usp_GetUserDirectReports", spParameters);
        }

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> GetUsersByCriteria(string strSearch)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("UserSch", strSearch) };
            var repository = _unitOfWork.GetRepository<UserDirectReportsModel>(SchemaName.Core);
            return repository.ExecuteStoredProc("usp_GetUsers", spParameters);
        }

        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> Add(UserDirectReportsModel userDetail)
        {
            var spParameters = BuildSpParams(userDetail);
            var repository = _unitOfWork.GetRepository<UserDirectReportsModel>(SchemaName.Core);
            return _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_AddUserDirectReport", 
                    spParameters, forceRollback: userDetail.ForceRollback.GetValueOrDefault(false), idResult: true);
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> Remove(long id, DateTime modifiedOn)
        {
            var repository = _unitOfWork.GetRepository<UserDirectReportsModel>(SchemaName.Core);
            var spParameters = new List<SqlParameter> { new SqlParameter("MappingID", id), new SqlParameter("ModifiedOn", modifiedOn) };
            return repository.ExecuteNQStoredProc("usp_DeleteUserDirectReport", spParameters);
        }

        private List<SqlParameter> BuildSpParams(UserDirectReportsModel userDetail)
        {
            //There will only be add and no edit. Mapping can only be deleted
            var spParameters = new List<SqlParameter> { 
                                                        new SqlParameter("UserID", userDetail.UserID), 
                                                        new SqlParameter("ParentID", userDetail.ParentID),
                                                        new SqlParameter("ModifiedOn", userDetail.ModifiedOn)
                                                    };
            return spParameters;
        }

    }
}
