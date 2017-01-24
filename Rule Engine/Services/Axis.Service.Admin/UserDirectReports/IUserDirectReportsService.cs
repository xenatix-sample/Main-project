using Axis.Model.Admin;
using Axis.Model.Common;
using System;

namespace Axis.Service.Admin.UserDirectReports
{
    public interface IUserDirectReportsService
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        Response<UserDirectReportsModel> GetUsers(int userID);

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        Response<UserDirectReportsModel> GetUsersByCriteria(string strSearch);

        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        Response<UserDirectReportsModel> Add(UserDirectReportsModel userDetail);

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<UserDirectReportsModel> Remove(long id, DateTime modifiedOn);

    }
}
