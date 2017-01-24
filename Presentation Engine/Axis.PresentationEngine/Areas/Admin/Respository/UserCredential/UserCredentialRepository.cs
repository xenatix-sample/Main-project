using System.Collections.Specialized;
using System.Threading.Tasks;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Translator;
using Axis.Service;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public class UserCredentialRepository : IUserCredentialRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "usercredential/";

        #endregion Class Variables

        #region Constructors

        public UserCredentialRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public UserCredentialRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods


        public Response<UserCredentialModel> GetUserCredentials(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfileCredentials" : "GetUserCredentials";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<UserCredentialModel>>(param, apiUrl);
            return response;
        }

        public Response<UserCredentialModel> GetUserCredentialsWithServiceID(int userID, long moduleComponentID)
        {
            var apiUrl = BaseRoute + "GetUserCredentialsWithServiceID";
            var param = new NameValueCollection { { "userID", userID.ToString() }, { "moduleComponentID", moduleComponentID.ToString() } };
            var response = _communicationManager.Get<Response<UserCredentialModel>>(param, apiUrl);
            return response;
        }


        public async Task<Response<UserViewModel>> GetUserCredentialsAsync(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfileCredentials" : "GetUserCredentials";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return (await _communicationManager.GetAsync<Response<UserModel>>(param, apiUrl)).ToModel();
        }


        public Response<UserViewModel> SaveUserCredentials(UserViewModel user, bool isMyProfile)
        {
            var route = isMyProfile ? "SaveMyProfileCredentials" : "SaveUserCredentials";
            var apiUrl = BaseRoute + route;
            var response = _communicationManager.Post<UserModel, Response<UserModel>>(user.ToModel(), apiUrl);

            return response.ToModel();
        }

        #endregion
    }
}