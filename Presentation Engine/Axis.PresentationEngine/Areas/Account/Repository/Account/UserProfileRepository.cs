using System.Collections.Specialized;
using System.Threading.Tasks;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Translator;
using Axis.Service;

namespace Axis.PresentationEngine.Areas.Account.Repository
{
    public class UserProfileRepository : IUserProfileRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "userProfile/";

        #endregion

        #region Constructors

        public UserProfileRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public UserProfileRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion
        
        #region Methods

        public Response<UserProfileViewModel> GetUserProfile(bool isMyProfile)
        {
            string route = isMyProfile ? "GetMyProfile" : "GetUserProfile";
            string apiUrl = baseRoute + route;

            var response = communicationManager.Get<Response<UserProfileModel>>(apiUrl);
            return response.ToModel();
        }

        public Response<UserProfileViewModel> GetUserProfileByID(int userID, bool isMyProfile)
        {
            string route = isMyProfile ? "GetMyProfileByID" : "GetUserProfileByID";
            string apiUrl = baseRoute + route;
            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());

            var response = communicationManager.Get<Response<UserProfileModel>>(param, apiUrl);
            return response.ToModel();
        }

        public async Task<Response<UserProfileViewModel>> GetUserProfileAsync(bool isMyProfile)
        {
            string route = isMyProfile ? "GetMyProfile" : "GetUserProfile";
            string apiUrl = baseRoute + route;
            var nvc = new NameValueCollection();

            return (await communicationManager.GetAsync<Response<UserProfileModel>>(nvc, apiUrl)).ToModel();
        }

        public Response<UserProfileViewModel> SaveUserProfile(UserProfileViewModel userProfile, bool isMyProfile)
        {
            var route = isMyProfile ? "SaveMyProfile" : "SaveUserProfile";
            string apiUrl = baseRoute + route;

            var response = communicationManager.Post<UserProfileModel, Response<UserProfileModel>>(userProfile.ToModel(), apiUrl);
            return response.ToModel();
        }

        #endregion
    }
}