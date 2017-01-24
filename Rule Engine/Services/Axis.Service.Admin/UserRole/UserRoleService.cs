using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Security;

namespace Axis.Service.Admin
{
    public class UserRoleService : IUserRoleService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "userRole/";

        #endregion

        #region Constructors

        public UserRoleService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            string apiUrl = BaseRoute + "GetUserRoles";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<UserRoleModel>>(param, apiUrl);
        }

        public Response<UserModel> SaveUserRoles(UserModel user)
        {
            string apiUrl = BaseRoute + "SaveUserRoles";
            return _communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        #endregion
    }
}
