using System.Collections.Specialized;
using System.Threading.Tasks;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Translator;
using Axis.Service;
using Axis.PresentationEngine.Areas.Admin.Model;
using Axis.PresentationEngine.Areas.Admin.Translator;      
using System.Globalization;
using Axis.Model.Admin;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public class UserDetailRepository : IUserDetailRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "userdetail/";

        #endregion Class Variables

        #region Constructors

        public UserDetailRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public UserDetailRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods

      
        public Response<UserViewModel> GetUser(int userID)
        {
            var apiUrl = BaseRoute + "GetUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<UserModel>>(param, apiUrl);
            return response.ToModel();
        }

       
        public async Task<Response<UserViewModel>> GetUserAsync(int userID)
        {
            var apiUrl = BaseRoute + "GetUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return (await _communicationManager.GetAsync<Response<UserModel>>(param, apiUrl)).ToModel();
        }

        
        public Response<UserViewModel> AddUser(UserViewModel userDetail)
        {
            var apiUrl = BaseRoute + "AddUser";
            var response = _communicationManager.Post<UserModel, Response<UserModel>>(userDetail.ToModel(), apiUrl);

            return response.ToModel();
        }

       
        public Response<UserViewModel> UpdateUser(UserViewModel userDetail)
        {
            var apiUrl = BaseRoute + "UpdateUser";
            var response = _communicationManager.Post<UserModel, Response<UserModel>>(userDetail.ToModel(), apiUrl);

            return response.ToModel();
        }

        public Response<CoSignaturesViewModel> GetCoSignatures(int userID)
        {
            var apiUrl = BaseRoute + "GetCoSignatures";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<CoSignaturesModel>>(param, apiUrl);
            return response.ToModel();
        }

        public async Task<Response<CoSignaturesViewModel>> GetCoSignaturesAsync(int userID)
        {
            var apiUrl = BaseRoute + "GetCoSignatures";
            var param = new NameValueCollection { { "userID", userID.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<CoSignaturesModel>>(param, apiUrl);

            return response.ToModel();
        }

        public Response<CoSignaturesViewModel> AddCoSignatures(CoSignaturesViewModel signature)
        {
            const string apiUrl = BaseRoute + "AddCoSignatures";
            var response = _communicationManager.Post<CoSignaturesModel, Response<CoSignaturesModel>>(signature.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<CoSignaturesViewModel> UpdateCoSignatures(CoSignaturesViewModel signature)
        {
            const string apiUrl = BaseRoute + "UpdateCoSignatures";
            var response = _communicationManager.Put<CoSignaturesModel, Response<CoSignaturesModel>>(signature.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<CoSignaturesViewModel> DeleteCoSignatures(long id, System.DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteCoSignatures";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Delete<Response<CoSignaturesModel>>(requestId, apiUrl);
            return response.ToModel();
        }

        public Response<UserIdentifierViewModel> GetUserIdentifierDetails(int userID)
        {
            var apiUrl = BaseRoute + "GetUserIdentifierDetails";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<UserIdentifierDetailsModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<UserIdentifierViewModel> AddUserIdentifierDetails(UserIdentifierViewModel useridentifier)
        {
            const string apiUrl = BaseRoute + "AddUserIdentifierDetails";
            var response = _communicationManager.Post<UserIdentifierDetailsModel, Response<UserIdentifierDetailsModel>>(useridentifier.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<UserIdentifierViewModel> UpdateUserIdentifierDetails(UserIdentifierViewModel useridentifier)
        {
            const string apiUrl = BaseRoute + "UpdateUserIdentifierDetails";
            var response = _communicationManager.Put<UserIdentifierDetailsModel, Response<UserIdentifierDetailsModel>>(useridentifier.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<UserIdentifierViewModel> DeleteUserIdentifierDetails(long id, System.DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteUserIdentifierDetails";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Delete<Response<UserIdentifierDetailsModel>>(requestId, apiUrl);
            return response.ToModel();
        }

        public Response<UserAdditionalDetailsViewModel> GetUserAdditionalDetails(int userID)
        {
            var apiUrl = BaseRoute + "GetUserAdditionalDetails";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<UserAdditionalDetailsModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<UserAdditionalDetailsViewModel> AddUserAdditionalDetails(UserAdditionalDetailsViewModel details)
        {
            const string apiUrl = BaseRoute + "AddUserAdditionalDetails";
            var response = _communicationManager.Post<UserAdditionalDetailsModel, Response<UserAdditionalDetailsModel>>(details.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<UserAdditionalDetailsViewModel> UpdateUserAdditionalDetails(UserAdditionalDetailsViewModel details)
        {
            const string apiUrl = BaseRoute + "UpdateUserAdditionalDetails";
            var response = _communicationManager.Put<UserAdditionalDetailsModel, Response<UserAdditionalDetailsModel>>(details.ToModel(), apiUrl);
            return response.ToModel();
        }

        public Response<UserAdditionalDetailsViewModel> DeleteUserAdditionalDetails(long id, System.DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteUserAdditionalDetails";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Delete<Response<UserAdditionalDetailsModel>>(requestId, apiUrl);
            return response.ToModel();
        }

        public async Task<Response<UserIdentifierViewModel>> GetUserIdentifierDetailsAsync(int userID)
        {
            var apiUrl = BaseRoute + "GetUserIdentifierDetails";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = await _communicationManager.GetAsync<Response<UserIdentifierDetailsModel>>(param, apiUrl);
            return response.ToModel();
        }

        public async Task<Response<UserAdditionalDetailsViewModel>> GetUserAdditionalDetailsAsync(int userID)
        {
            var apiUrl = BaseRoute + "GetUserAdditionalDetails";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = await _communicationManager.GetAsync<Response<UserAdditionalDetailsModel>>(param, apiUrl);
            return response.ToModel();
        }
        #endregion
    
    }
}