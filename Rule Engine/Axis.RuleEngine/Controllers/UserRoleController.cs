using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Service.Controllers
{
    public class UserRoleController: BaseApiController
    {
        #region Class Variables

        private readonly IUserRoleRuleEngine _userRoleRuleEngine = null;

        #endregion

        #region Constructors

        public UserRoleController(IUserRoleRuleEngine userRoleRuleEngine)
        {
            _userRoleRuleEngine = userRoleRuleEngine;
        }

        #endregion

        #region Public Methods
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserRoles, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUserRoles(int userID)
        {
            Response<UserRoleModel> responseObject = _userRoleRuleEngine.GetUserRoles(userID);
            return new HttpResult<Response<UserRoleModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserRoles, Permissions = new String[]{Permission.Create,Permission.Update})]
        [HttpPost]
        public IHttpActionResult SaveUserRoles(UserModel user)
        {
            Response<UserModel> responseObject = _userRoleRuleEngine.SaveUserRoles(user);
            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        #endregion
    }
}