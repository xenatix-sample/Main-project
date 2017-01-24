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
    public class UserRoleRepository : IUserRoleRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "userrole/";

        #endregion Class Variables

        #region Constructors

        public UserRoleRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public UserRoleRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods

       
        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            var apiUrl = BaseRoute + "GetUserRoles";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<UserRoleModel>>(param, apiUrl);
            return response;
        }

   
        public async Task<Response<UserRoleModel>> GetUserRolesAsync(int userID)
        {
            var apiUrl = BaseRoute + "GetUserRoles";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return (await _communicationManager.GetAsync<Response<UserRoleModel>>(param, apiUrl));
        }

       
        public Response<UserViewModel> SaveUserRoles(UserViewModel user)
        {
            var apiUrl = BaseRoute + "SaveUserRoles";
            var response = _communicationManager.Post<UserModel, Response<UserModel>>(user.ToModel(), apiUrl);

            return response.ToModel();
        }

        #endregion
    }
}