using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.RuleEngine.Account.UserProfile
{
    public interface IUserProfileRuleEngine
    {
        Response<UserProfileModel> GetUserProfile(int userID);
        Response<UserProfileModel> SaveUserProfile(UserProfileModel userProfile);
    }
}
