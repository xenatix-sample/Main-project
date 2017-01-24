using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.Service.Account.UserProfile
{
    public interface IUserProfileService
    {
        Response<UserProfileModel> GetUserProfile(int userID);
        Response<UserProfileModel> SaveUserProfile(UserProfileModel userProfile);
    }
}
