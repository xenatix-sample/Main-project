using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using System;
using System.Threading.Tasks;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public interface IUserDirectReportsRepository
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        Response<UserDirectReportsViewModel> GetUsers(int userID, bool isMyProfile);

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        Task<Response<UserDirectReportsViewModel>> GetUsersAsync(int userID, bool isMyProfile);

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        Response<UserDirectReportsViewModel> GetUsersByCriteria(string strSearch, bool isMyProfile);
        
        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        Response<UserDirectReportsViewModel> Add(UserDirectReportsViewModel userDetail, bool isMyProfile);

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<UserDirectReportsViewModel> Remove(long id, DateTime modifiedOn, bool isMyProfile);

    }
}
