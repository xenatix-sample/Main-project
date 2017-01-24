using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.Service.Account
{
    public interface IAccountService
    {
        Response<UserModel> Authenticate(UserModel user);
        Response<UserModel> SetLoginData(UserModel user);
        void SyncUser(UserModel user);
        UserModel GetValidUserInfoByToken(AccessTokenModel accessToken);
        void LogAccessToken(AccessTokenModel accessToken);
        Response<ServerResourceModel> IsValidServerIP(string ipAddress);
        Response<AccessTokenModel> GetTokenIssueExpireDate();
        Response<NavigationModel> GetNavigationItems(int userID);
    }
}