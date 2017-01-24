using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.Service.Admin
{
    public interface IUserRoleService
    {
        Response<UserRoleModel> GetUserRoles(int userID);
        Response<UserModel> SaveUserRoles(UserModel user);
    }
}
