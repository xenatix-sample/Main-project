using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Admin;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.DataEngine.Service.Controllers
{
    public class UserRoleController : BaseApiController
    {
        #region Class Variables

        readonly IUserRoleDataProvider _userRoleDataProvider = null;

        #endregion

        #region Constructors

        public UserRoleController(IUserRoleDataProvider userRoleDataProvider)
        {
            _userRoleDataProvider = userRoleDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetUserRoles(int userID)
        {
            var userResponse = _userRoleDataProvider.GetUserRoles(userID);
            return new HttpResult<Response<UserRoleModel>>(userResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult SaveUserRoles(UserModel user)
        {
            var userResponse = _userRoleDataProvider.SaveUserRoles(user);
            return new HttpResult<Response<UserModel>>(userResponse, Request);
        }

        #endregion
    }
}