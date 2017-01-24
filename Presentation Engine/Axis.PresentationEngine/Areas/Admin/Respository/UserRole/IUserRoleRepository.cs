using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public interface IUserRoleRepository
    {
        Response<UserRoleModel> GetUserRoles(int userID);
        Task<Response<UserRoleModel>> GetUserRolesAsync(int userID);
        Response<UserViewModel> SaveUserRoles(UserViewModel user);
    }
}
