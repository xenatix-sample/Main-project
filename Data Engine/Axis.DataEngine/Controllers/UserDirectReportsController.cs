using Axis.DataEngine.Helpers.Controllers;
using Axis.DataProvider.Admin.UserDirectReports;
using Axis.Model.Admin;
using Axis.Model.Common;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class UserDirectReportsController : BaseApiController
    {
        /// <summary>
        /// The _user direct reports data provider
        /// </summary>
        private IUserDirectReportsDataProvider _userDirectReportsDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDirectReportsController"/> class.
        /// </summary>
        /// <param name="userDirectReportsDataProvider">The user direct reports data provider.</param>
        public UserDirectReportsController(IUserDirectReportsDataProvider userDirectReportsDataProvider) 
        {
            _userDirectReportsDataProvider = userDirectReportsDataProvider;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<UserDirectReportsModel> GetUsers(int userID)
        {
            return _userDirectReportsDataProvider.GetUsers(userID);
        }

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<UserDirectReportsModel> GetUsersByCriteria(string strSearch)
        {
            return _userDirectReportsDataProvider.GetUsersByCriteria(strSearch);
        }

        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<UserDirectReportsModel> Add(UserDirectReportsModel userDetail)
        {
            return _userDirectReportsDataProvider.Add(userDetail);
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<UserDirectReportsModel> Remove(long id, DateTime modifiedOn)
        {
            return _userDirectReportsDataProvider.Remove(id, modifiedOn);
        }
    }
}