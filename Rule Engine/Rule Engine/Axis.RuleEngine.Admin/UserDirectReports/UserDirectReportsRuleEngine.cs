using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Service.Admin.UserDirectReports;
using System;

namespace Axis.RuleEngine.Admin.UserDirectReports
{
    public class UserDirectReportsRuleEngine : IUserDirectReportsRuleEngine
    {
        /// <summary>
        /// The _user direct reports service
        /// </summary>
        private readonly IUserDirectReportsService _userDirectReportsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDirectReportsRuleEngine"/> class.
        /// </summary>
        /// <param name="userDirectReportsService">The user direct reports service.</param>
        public UserDirectReportsRuleEngine(IUserDirectReportsService userDirectReportsService)
        {
            _userDirectReportsService = userDirectReportsService;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> GetUsers(int userID)
        {
            return _userDirectReportsService.GetUsers(userID);
        }

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> GetUsersByCriteria(string strSearch)
        {
            return _userDirectReportsService.GetUsersByCriteria(strSearch);
        }

        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> Add(UserDirectReportsModel userDetail)
        {
            return _userDirectReportsService.Add(userDetail);
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> Remove(long id, DateTime modifiedOn)
        {
            return _userDirectReportsService.Remove(id, modifiedOn);
        }

    }
}
