using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.RuleEngine.Admin
{
    public interface IUserCredentialRuleEngine
    {
        Response<UserCredentialModel> GetUserCredentials(int userID);
        Response<UserCredentialModel> GetUserCredentialsWithServiceID(int userID, long moduleComponentID);
        Response<UserModel> SaveUserCredentials(UserModel user);
    }
}
