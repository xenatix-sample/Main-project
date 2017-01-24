using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.User;

namespace Axis.DataProvider.Account.UserProfile
{
    public interface IUserProfileDataProvider
    {
        Response<UserProfileModel> GetUserProfile(int userId);
        Response<UserProfileModel> SaveUserProfile(UserProfileModel userProfile);
    }
}
