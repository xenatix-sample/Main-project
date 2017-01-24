using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Axis.Configuration;
using Axis.Constant;
using Axis.Model.Account;
using Axis.RuleEngine.Account;
using Axis.RuleEngine.Helpers.Results;
using Axis.Security;
using System.Diagnostics;
using Axis.Model.Common;
using Axis.RuleEngine.Service.Helpers;
using Axis.RuleEngine.Helpers.Controllers;

namespace Axis.RuleEngine.Service
{

    public class AccountController : BaseApiController
    {
        IAccountRuleEngine accountRuleEngine = null;
        public AccountController(IAccountRuleEngine accountRuleEngine)
        {
            this.accountRuleEngine = accountRuleEngine;
        }

        [HttpPost]        
        public IHttpActionResult Authenticate(UserModel user)
        {
            var isAuthenticated = false;
            var authenticationModel = new AuthenticationModel() { IsAuthenticated = isAuthenticated };
            UserModel potentialUserModel = new UserModel() { UserID = -1, UserName = user.UserName, Password = string.Empty, Roles = new List<Model.Security.RoleModel>() }; 

            if (Convert.ToInt32(ConfigurationManager.AppSettings["AuthenticationType"]) == (int)AuthenticationType.ADFS)
            {
                isAuthenticated = ActiveDirectory.Authenticate(user.UserName, user.Password);
            }
            else if (Convert.ToInt32(ConfigurationManager.AppSettings["AuthenticationType"]) == (int)AuthenticationType.Forms)
            {
                //We found a user with the provided username and password
                var loginResponse = accountRuleEngine.Authenticate(user);

                if (loginResponse.DataItems.Count > 0)
                {
                    potentialUserModel = loginResponse.DataItems.FirstOrDefault();
                }

                isAuthenticated = potentialUserModel.UserID > 0;
                if (!isAuthenticated) {
                    authenticationModel.Resultcode = loginResponse.ResultCode;
                }
            }

            if (isAuthenticated)
            {
                authenticationModel.IsAuthenticated = true;

                // to retrive issue on and expire on date from database
                var tokenIssueExpireOnDate = accountRuleEngine.GetTokenIssueExpireDate();

                if (user.UserName.Contains("\\"))
                {
                    user.UserName = user.UserName.Split('\\')[1];
                }

                var token = new AccessTokenModel()
                {
                    UserId = potentialUserModel.UserID,
                    UserName = user.UserName,
                    ClientIP = user.IPAddress,
                    SessionID = user.SessionID,
                    GeneratedOn = tokenIssueExpireOnDate.DataItems[0].GeneratedOn,
                    ExpirationDate = tokenIssueExpireOnDate.DataItems[0].ExpirationDate                    
                };

                token.Token = token.Encrypt();
                authenticationModel.Token.Token = token.Encrypt();
                authenticationModel.Message = "Authenticated";
                authenticationModel.User = potentialUserModel;

                WebSecurity.SignIn(potentialUserModel, token);

                accountRuleEngine.LogAccessToken(token);

                if (Convert.ToInt32(ConfigurationManager.AppSettings["AuthenticationType"]) == (int)AuthenticationType.ADFS)
                {
                    accountRuleEngine.SyncUser(user);
                }
            }

            //Update the loginattempts, lastlogin, etc...
            accountRuleEngine.SetLoginData(potentialUserModel);

            return new HttpResult<AuthenticationModel>(authenticationModel, Request);
        }

        [HttpGet]
        public IHttpActionResult GetNavigationItems()
        {
            int userID = AuthContext.Auth.User.UserID;
            return new HttpResult<Response<NavigationModel>>(accountRuleEngine.GetNavigationItems(userID), Request);
        }
    }
}
