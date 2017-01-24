using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Service.Account.UserProfile;

namespace Axis.RuleEngine.Account.UserProfile
{
    public class UserProfileRuleEngine : IUserProfileRuleEngine
    {
        #region Class Variables

        private readonly IUserProfileService _userProfileService;

        #endregion

        #region Constructors

        public UserProfileRuleEngine(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        #endregion

        #region Public Methods

        public Response<UserProfileModel> GetUserProfile(int userID)
        {
            return _userProfileService.GetUserProfile(userID);
        }

        public Response<UserProfileModel> SaveUserProfile(UserProfileModel userProfile)
        {
            return _userProfileService.SaveUserProfile(userProfile);
        }

        #endregion
    }
}
