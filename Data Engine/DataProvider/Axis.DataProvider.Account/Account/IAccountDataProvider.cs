using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.DataProvider.Account
{
    public interface IAccountDataProvider
    {
        Response<UserModel> Authenticate(UserModel user);

        Response<UserModel> SetLoginData(UserModel user);

        void SyncUser(UserModel user);

        UserModel GetValidUserInfoByToken(AccessTokenModel accessToken);

        void LogAccessToken(AccessTokenModel accessToken);

        Response<ServerResourceModel> IsValidServerIP(string ipAddress);

        Response<AccessTokenModel> GetTokenIssueExpireDate();

        Response<NavigationModel> GetNavigationItems(int userID);

        Response<UserOrganizationStructureModel> GetOrganizationStructureByUser(int userID);
    }
}