using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Admin;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.DataEngine.Service.Controllers
{
    public class UserCredentialController : BaseApiController
    {
        #region Class Variables

        readonly IUserCredentialDataProvider _userCredentialDataProvider = null;

        #endregion

        #region Constructors

        public UserCredentialController(IUserCredentialDataProvider userCredentialDataProvider)
        {
            _userCredentialDataProvider = userCredentialDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetUserCredentials(int userID)
        {
            var userResponse = _userCredentialDataProvider.GetUserCredentials(userID);
            return new HttpResult<Response<UserCredentialModel>>(userResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetUserCredentialsWithServiceID(int userID, long moduleComponentID)
        {
            var userResponse = _userCredentialDataProvider.GetUserCredentialsWithServiceID(userID, moduleComponentID);
            return new HttpResult<Response<UserCredentialModel>>(userResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult SaveUserCredentials(UserModel user)
        {
            var userResponse = _userCredentialDataProvider.SaveUserCredentials(user);
            return new HttpResult<Response<UserModel>>(userResponse, Request);
        }

        #endregion
    }
}