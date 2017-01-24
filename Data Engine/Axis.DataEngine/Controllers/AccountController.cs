using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Account;
using Axis.Model.Account;
using Axis.Model.Common;
using System.Net;
using System.Web.Http;
using Axis.Logging;

namespace Axis.DataEngine.Service.Controllers
{

    public class AccountController : BaseApiController
    {
        IAccountDataProvider accountDataProvider = null;
        public AccountController(IAccountDataProvider accountDataProvider)
        {
            this.accountDataProvider = accountDataProvider;
        }

        [HttpPost]
        public IHttpActionResult Authenticate(UserModel user)
        {
            var authenticationResult = accountDataProvider.Authenticate(user);

            return new HttpResult<Response<UserModel>>(authenticationResult, Request);
        }

        [HttpPost]
        public IHttpActionResult SetLoginData(UserModel user)
        {
            var setLoginDataResult = accountDataProvider.SetLoginData(user);

            return new HttpResult<Response<UserModel>>(setLoginDataResult, Request);
        }

        [HttpPost]
        public IHttpActionResult SyncUser(UserModel user)
        {
            accountDataProvider.SyncUser(user);

            return new HttpResult<int>((int)HttpStatusCode.Created, Request);
        }

        [HttpGet]
        [SkipLogActionFilter]
        public IHttpActionResult GetValidUserInfoByToken([FromUri]AccessTokenModel accessToken)
        {
            return new HttpResult<UserModel>(accountDataProvider.GetValidUserInfoByToken(accessToken), Request);
        }

        [HttpPost]
        public IHttpActionResult LogAccessToken(AccessTokenModel accessToken)
        {
            accountDataProvider.LogAccessToken(accessToken);

            return new HttpResult<int>((int)HttpStatusCode.Created, Request);
        }

        [HttpGet]
        [SkipLogActionFilter]
        public IHttpActionResult IsValidServerIP(string IPAddress)
        {
            var serverResourceResult = accountDataProvider.IsValidServerIP(IPAddress);

            return new HttpResult<Response<ServerResourceModel>>(serverResourceResult, Request);            
        }

        [HttpGet]
        public IHttpActionResult GetTokenIssueExpireDate()
        {
            var serverResourceResult = accountDataProvider.GetTokenIssueExpireDate();

            return new HttpResult<Response<AccessTokenModel>>(serverResourceResult, Request);
        }

        [HttpGet]
        public IHttpActionResult GetNavigationItems(int userID)
        {
            return new HttpResult<Response<NavigationModel>>(accountDataProvider.GetNavigationItems(userID), Request);
        }
    }
}
