using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;

namespace Axis.PresentationEngine.Areas.Account.Repository
{
    public interface IUserProfileRepository
    {
        Response<UserProfileViewModel> GetUserProfile(bool isMyProfile);
        Response<UserProfileViewModel> GetUserProfileByID(int userID, bool isMyProfile);
        Task<Response<UserProfileViewModel>> GetUserProfileAsync(bool isMyProfile);
        Response<UserProfileViewModel> SaveUserProfile(UserProfileViewModel userProfile, bool isMyProfile);
    }
}
