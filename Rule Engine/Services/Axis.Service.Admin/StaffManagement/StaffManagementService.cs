using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Security;

namespace Axis.Service.Admin
{
    public class StaffManagementService : IStaffManagementService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "staffManagement/";

        #endregion

        #region Constructors

        public StaffManagementService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<UserModel> GetStaff(string searchText)
        {
            string apiUrl = BaseRoute + "GetStaff";
            var param = new NameValueCollection { { "searchText", searchText ?? String.Empty } };

            return _communicationManager.Get<Response<UserModel>>(param, apiUrl);
        }

        public Response<UserModel> DeleteUser(int userID)
        {
            string apiUrl = BaseRoute + "DeleteUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Delete<Response<UserModel>>(param, apiUrl);
        }

        public Response<UserModel> ActivateUser(int userID)
        {
            string apiUrl = BaseRoute + "ActivateUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Post<Response<UserModel>>(param, apiUrl);
        }

        public Response<UserModel> UnlockUser(int userID)
        {
            string apiUrl = BaseRoute + "UnlockUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Post<Response<UserModel>>(param, apiUrl);
        }

        #endregion
    }
}
