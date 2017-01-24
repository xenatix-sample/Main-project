using Axis.Constant;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.RuleEngine.Admin.UserDirectReports;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using System;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class UserDirectReportsController : BaseApiController
    {
        /// <summary>
        /// The _user direct reports rule engine
        /// </summary>
        private readonly IUserDirectReportsRuleEngine _userDirectReportsRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDirectReportsController"/> class.
        /// </summary>
        /// <param name="userDirectReportsRuleEngine">The user direct reports rule engine.</param>
        public UserDirectReportsController(IUserDirectReportsRuleEngine userDirectReportsRuleEngine)
        {
            _userDirectReportsRuleEngine = userDirectReportsRuleEngine;
        }

        #region Site Admin Public Methods
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports, Permission = Permission.Read)]
        [HttpGet]
        public Response<UserDirectReportsModel> GetUsers(int userID)
        {
            return _userDirectReportsRuleEngine.GetUsers(userID);
        }

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports, Permission = Permission.Read)]
        [HttpGet]
        public Response<UserDirectReportsModel> GetUsersByCriteria(string strSearch)
        {
            return _userDirectReportsRuleEngine.GetUsersByCriteria(strSearch);
        }

        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports, Permission = Permission.Create)]
        [HttpPost]
        public Response<UserDirectReportsModel> Add(UserDirectReportsModel userDetail)
        {
            return _userDirectReportsRuleEngine.Add(userDetail);
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports, Permission = Permission.Delete)]
        [HttpDelete]
        public Response<UserDirectReportsModel> Remove(long id, DateTime modifiedOn)
        {
            return _userDirectReportsRuleEngine.Remove(id, modifiedOn);
        }
        #endregion Site Admin Public Methods

        #region My Profile Public Methods
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<UserDirectReportsModel> GetMyProfile(int userID)
        {
            return _userDirectReportsRuleEngine.GetUsers(userID);
        }

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<UserDirectReportsModel> GetMyProfileByCriteria(string strSearch)
        {
            return _userDirectReportsRuleEngine.GetUsersByCriteria(strSearch);
        }

        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<UserDirectReportsModel> AddMyProfile(UserDirectReportsModel userDetail)
        {
            return _userDirectReportsRuleEngine.Add(userDetail);
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<UserDirectReportsModel> RemoveMyProfile(long id, DateTime modifiedOn)
        {
            return _userDirectReportsRuleEngine.Remove(id, modifiedOn);
        }
        #endregion My Profile Public Methods
    }
}