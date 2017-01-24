using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Service.Admin;

namespace Axis.RuleEngine.Admin
{
    public class UserCredentialRuleEngine : IUserCredentialRuleEngine
    {
        #region Class Variables

        private readonly IUserCredentialService _userCredentialService;

        #endregion

        #region Constructors

        public UserCredentialRuleEngine(IUserCredentialService userCredentialService)
        {
            _userCredentialService = userCredentialService;
        }

        #endregion

        #region Public Methods

        public Response<UserCredentialModel> GetUserCredentials(int userID)
        {
            return _userCredentialService.GetUserCredentials(userID);
        }

        public Response<UserCredentialModel> GetUserCredentialsWithServiceID(int userID, long moduleComponentID)
        {
            return _userCredentialService.GetUserCredentialsWithServiceID(userID, moduleComponentID);
        }

        public Response<UserModel> SaveUserCredentials(UserModel user)
        {
            return _userCredentialService.SaveUserCredentials(user);
        }

        #endregion
    }
}
