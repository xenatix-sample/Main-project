using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public interface IUserCredentialRepository
    {
        Response<UserCredentialModel> GetUserCredentials(int userID, bool isMyProfile);
        Response<UserCredentialModel> GetUserCredentialsWithServiceID(int userID, long moduleComponentID);
        Task<Response<UserViewModel>> GetUserCredentialsAsync(int userID, bool isMyProfile);
        Response<UserViewModel> SaveUserCredentials(UserViewModel user, bool isMyProfile);
    }
}
