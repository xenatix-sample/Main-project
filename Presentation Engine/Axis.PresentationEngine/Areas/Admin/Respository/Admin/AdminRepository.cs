using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using Axis.PresentationEngine.Areas.Admin.Model;
using Axis.PresentationEngine.Areas.Admin.Translator;


namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    /// <summary>
    ///
    /// </summary>
    public class AdminRepository : IAdminRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "admin/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRepository"/> class.
        /// </summary>
        public AdminRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public AdminRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userSch">The generic users search field</param>
        /// <returns></returns>

        public Response<UserViewModel> GetUsers(string userSch)
        {
            string apiUrl = baseRoute + "GetUsers";
            var param = new NameValueCollection();
            param.Add("userSch", userSch);

            var response = communicationManager.Get<Response<UserModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
      
        public Response<UserViewModel> AddUser(UserViewModel user)
        {
            string apiUrl = baseRoute + "AddUser";
            var response = communicationManager.Post<UserModel, Response<UserModel>>(user.ToModel(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
       
        public Response<UserViewModel> UpdateUser(UserViewModel user)
        {
            string apiUrl = baseRoute + "UpdateUser";
            var response = communicationManager.Post<UserModel, Response<UserModel>>(user.ToModel(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
       
        public Response<UserViewModel> RemoveUser(UserViewModel user)
        {
            string apiUrl = baseRoute + "RemoveUser";
            DateTime finalDate = DateTime.UtcNow;
            if (user.ModifiedOn != null)
            {
                finalDate = (DateTime)user.ModifiedOn;
            }

            var param = new NameValueCollection
            {
                {"userID", user.UserID.ToString()},
                {"modifiedOn", finalDate.ToString(CultureInfo.InvariantCulture)}
            };
            var response = communicationManager.Delete<Response<UserModel>>(param, apiUrl);

            return response.ToModel();
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
     
        public Response<UserViewModel> ActivateUser(UserViewModel user)
        {
            string apiUrl = baseRoute + "ActivateUser";
            var response = communicationManager.Post<UserModel, Response<UserModel>>(user.ToModel(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Unlocks the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
      
        public Response<UserViewModel> UnlockUser(UserViewModel user)
        {
            string apiUrl = baseRoute + "UnlockUser";
            var response = communicationManager.Post<UserModel, Response<UserModel>>(user.ToModel(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
   
        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            string apiUrl = baseRoute + "GetUserRoles";
            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());

            return communicationManager.Get<Response<UserRoleModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the user credentials.
        /// </summary>
        /// <param name="userView">The user identifier.</param>
        /// <returns></returns>
      
        public Response<UserCredentialViewModel> GetUserCredentials(UserViewModel userView)
        {
            var apiUrl = baseRoute + "GetUserCredentials";
            
            var param = new NameValueCollection();
            param.Add("userID", userView.UserID.ToString());

            var response = communicationManager.Get<Response<UserCredentialModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Add the user credentials.
        /// </summary>
        /// <param name="userCredentialView">credential for a user.</param>
        /// <returns></returns>
    
        public Response<UserCredentialViewModel> AddUserCredential(UserCredentialViewModel userCredentialView)
        {
            var apiUrl = baseRoute + "AddUserCredential";

            var response = communicationManager.Post<UserCredentialModel, Response<UserCredentialModel>>(userCredentialView.ToModel(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Update the user credentials.
        /// </summary>
        /// <param name="userCredentialView">credential for a user.</param>
        /// <returns></returns>
     
        public Response<UserCredentialViewModel> UpdateUserCredential(UserCredentialViewModel userCredentialView)
        {
            var apiUrl = baseRoute + "UpdateUserCredential";

            var response = communicationManager.Post<UserCredentialModel, Response<UserCredentialModel>>(userCredentialView.ToModel(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Update the user credentials.
        /// </summary>
        /// <param name="userCredentialID">credential for a user.</param>
        /// <returns></returns>
      
        public Response<UserCredentialViewModel> DeleteUserCredential(int userCredentialID, DateTime modifiedOn)
        {
            var apiUrl = baseRoute + "DeleteUserCredential";

            var param = new NameValueCollection
            {
                {"userCredentialID", userCredentialID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            var response = communicationManager.Delete<Response<UserCredentialModel>>(param, apiUrl);
            return response.ToModel();
        }

        public Response<ResetPasswordViewModel> VerifySecurityDetails(ResetPasswordViewModel resetPassword)
        {
            var apiUrl = baseRoute + "verifySecurityDetails";
            var response = communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword.ToEntity(), apiUrl);
            return response.ToModel();
        }

        #endregion Methods
    }
}