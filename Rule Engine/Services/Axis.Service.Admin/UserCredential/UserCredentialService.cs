using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Security;

namespace Axis.Service.Admin
{
    public class UserCredentialService : IUserCredentialService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "userCredential/";

        #endregion

        #region Constructors

        public UserCredentialService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<UserCredentialModel> GetUserCredentials(int userID)
        {
            string apiUrl = BaseRoute + "GetUserCredentials";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<UserCredentialModel>>(param, apiUrl);
        }

        public Response<UserCredentialModel> GetUserCredentialsWithServiceID(int userID, long moduleComponentID)
        {
            string apiUrl = BaseRoute + "GetUserCredentialsWithServiceID";
            var param = new NameValueCollection { { "userID", userID.ToString() }, { "moduleComponentID", moduleComponentID.ToString() } };

            return _communicationManager.Get<Response<UserCredentialModel>>(param, apiUrl);
        }

        public Response<UserModel> SaveUserCredentials(UserModel user)
        {
            string apiUrl = BaseRoute + "SaveUserCredentials";
            return _communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        #endregion
    }
}
