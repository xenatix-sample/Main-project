using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Axis.Service.Account
{
    public class AccountService : IAccountService
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "account/";

        public AccountService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public Response<UserModel> Authenticate(UserModel user)
        {
            var apiUrl = baseRoute + "authenticate";
            return communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        public Response<UserModel> SetLoginData(UserModel user)
        {
            var apiUrl = baseRoute + "SetLoginData";
            return communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        public void SyncUser(UserModel user)
        {
            var apiUrl = baseRoute + "/syncUser";
            communicationManager.Post<UserModel, int>(user, apiUrl);
        }

        public UserModel GetValidUserInfoByToken(AccessTokenModel accessToken)
        {
            var apiUrl = baseRoute + "/GetValidUserInfoByToken";
            return communicationManager.Get<AccessTokenModel, UserModel>(accessToken, apiUrl);
        }

        public void LogAccessToken(AccessTokenModel accessToken)
        {
            var apiUrl = baseRoute + "/logAccessToken";
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
            communicationManager.Post<AccessTokenModel, int>(accessToken, apiUrl);
        }

        public Response<ServerResourceModel> IsValidServerIP(string ipAddress)
        {
            var apiUrl = baseRoute + "/IsValidServerIP";
            var param = new NameValueCollection();
            param.Add("ipAddress", ipAddress ?? string.Empty);

            return communicationManager.Get<Response<ServerResourceModel>>(param, apiUrl);
        }

        public Response<AccessTokenModel> GetTokenIssueExpireDate()
        {
            var apiUrl = baseRoute + "/GetTokenIssueExpireDate";

            return communicationManager.Get<Response<AccessTokenModel>>(apiUrl);
        }

        public Response<NavigationModel> GetNavigationItems(int userID)
        {
            var apiUrl = baseRoute + "GetNavigationItems";
            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());

            return communicationManager.Get<Response<NavigationModel>>(param, apiUrl);
        }
    }
}