using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.DataProvider.Admin
{
    public interface IUserCredentialDataProvider
    {
        Response<UserCredentialModel> GetUserCredentials(int userID);
        Response<UserCredentialModel> GetUserCredentialsWithServiceID(int userID, long moduleComponentID);
        Response<UserModel> SaveUserCredentials(UserModel user);
        
    }
}
