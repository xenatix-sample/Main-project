using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Security;

namespace Axis.Service.Account.UserProfile
{
    public class UserProfileService : IUserProfileService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "userProfile/";

        #endregion

        #region Constructors

        public UserProfileService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<UserProfileModel> GetUserProfile(int userID)
        {
            var apiUrl = BaseRoute + "GetUserProfile";
            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());

            return _communicationManager.Get<Response<UserProfileModel>>(param, apiUrl);
        }

        public Response<UserProfileModel> SaveUserProfile(UserProfileModel userProfile)
        {
            var apiUrl = BaseRoute + "SaveUserProfile";

            return _communicationManager.Post<UserProfileModel, Response<UserProfileModel>>(userProfile, apiUrl);
        }

        #endregion
    }
}
