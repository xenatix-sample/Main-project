using System.Web.Http;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;
using System;

namespace Axis.RuleEngine.Service.Controllers
{
    public class UserCredentialController: BaseApiController
    {
        #region Class Variables

        private readonly IUserCredentialRuleEngine _userCredentialRuleEngine = null;

        #endregion

        #region Constructors

        public UserCredentialController(IUserCredentialRuleEngine userCredentialRuleEngine)
        {
            _userCredentialRuleEngine = userCredentialRuleEngine;
        }

        #endregion

        #region Site Admin Public Methods

        [HttpGet]
        public IHttpActionResult GetUserCredentials(int userID)
        {
            Response<UserCredentialModel> responseObject = _userCredentialRuleEngine.GetUserCredentials(userID);
            return new HttpResult<Response<UserCredentialModel>>(responseObject, Request);
        }

        [HttpGet]
        public IHttpActionResult GetUserCredentialsWithServiceID(int userID, long moduleComponentID)
        {
            Response<UserCredentialModel> responseObject = _userCredentialRuleEngine.GetUserCredentialsWithServiceID(userID, moduleComponentID);
            return new HttpResult<Response<UserCredentialModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Credentials, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult SaveUserCredentials(UserModel user)
        {
            Response<UserModel> responseObject = _userCredentialRuleEngine.SaveUserCredentials(user);
            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        #endregion Site Admin Public Methods

        #region My Profile Public Methods
        [HttpGet]
        public IHttpActionResult GetMyProfileCredentials(int userID)
        {
            Response<UserCredentialModel> responseObject = _userCredentialRuleEngine.GetUserCredentials(userID);
            return new HttpResult<Response<UserCredentialModel>>(responseObject, Request);
        }

        [HttpPost]
        public IHttpActionResult SaveMyProfileCredentials(UserModel user)
        {
            Response<UserModel> responseObject = _userCredentialRuleEngine.SaveUserCredentials(user);
            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        #endregion My Profile Public Methods
    }
}