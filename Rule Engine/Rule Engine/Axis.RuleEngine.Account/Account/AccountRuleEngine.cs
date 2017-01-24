using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Service.Account;

namespace Axis.RuleEngine.Account
{
    public class AccountRuleEngine : IAccountRuleEngine
    {
        private IAccountService accountService;
        public AccountRuleEngine(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public Response<UserModel> Authenticate(UserModel user)
        {
            return accountService.Authenticate(user);
        }

        public Response<UserModel> SetLoginData(UserModel user)
        {
            return accountService.SetLoginData(user);
        }

        public void SyncUser(UserModel user)
        {
            accountService.SyncUser(user);
        }

        public UserModel GetValidUserInfoByToken(AccessTokenModel accessToken)
        {
            return accountService.GetValidUserInfoByToken(accessToken);
        }

        public void LogAccessToken(AccessTokenModel accessToken)
        {
            accountService.LogAccessToken(accessToken);
        }

        public Response<ServerResourceModel> IsValidServerIP(string ipAddress)
        {
            return accountService.IsValidServerIP(ipAddress);
        }

        public Response<AccessTokenModel> GetTokenIssueExpireDate()
        {
            return accountService.GetTokenIssueExpireDate();
        }

        public Response<NavigationModel> GetNavigationItems(int userID)
        {
            return accountService.GetNavigationItems(userID);
        }
    }
}
