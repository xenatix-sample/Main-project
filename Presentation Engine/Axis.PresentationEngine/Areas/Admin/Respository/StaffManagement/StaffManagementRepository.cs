using System;
using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Translator;
using Axis.Service;


namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public class StaffManagementRepository : IStaffManagementRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "staffmanagement/";

        #endregion Class Variables

        #region Constructors

        public StaffManagementRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public StaffManagementRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods

       
        public Response<UserViewModel> GetStaff(string searchText)
        {
            var apiUrl = BaseRoute + "GetStaff";
            var param = new NameValueCollection { { "searchText", searchText ?? String.Empty } };

            var response = _communicationManager.Get<Response<UserModel>>(param, apiUrl);
            return response.ToModel();
        }

      
        public Response<UserViewModel> DeleteUser(int userID)
        {
            var apiUrl = BaseRoute + "DeleteUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Delete<Response<UserModel>>(param, apiUrl);
            return response.ToModel();
        }

     
        public Response<UserViewModel> ActivateUser(int userID)
        {
            var apiUrl = BaseRoute + "ActivateUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Post<Response<UserModel>>(param, apiUrl);
            return response.ToModel();
        }

        
        public Response<UserViewModel> UnlockUser(int userID)
        {
            var apiUrl = BaseRoute + "UnlockUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Post<Response<UserModel>>(param, apiUrl);
            return response.ToModel();
        }

        #endregion
    }
}