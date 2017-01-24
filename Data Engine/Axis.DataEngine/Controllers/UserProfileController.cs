using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Account.UserProfile;
using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.DataEngine.Service.Controllers
{
    public class UserProfileController : BaseApiController
    {
        #region Class Variables

        IUserProfileDataProvider _userProfileDataProvider = null;

        #endregion

        #region Constructors

        public UserProfileController(IUserProfileDataProvider userProfileDataProvider)
        {
            _userProfileDataProvider = userProfileDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetUserProfile(int userID)
        {
            return new HttpResult<Response<UserProfileModel>>(_userProfileDataProvider.GetUserProfile(userID), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveUserProfile(UserProfileModel userProfile)
        {
            return new HttpResult<Response<UserProfileModel>>(_userProfileDataProvider.SaveUserProfile(userProfile), Request);
        }

        #endregion
    }
}