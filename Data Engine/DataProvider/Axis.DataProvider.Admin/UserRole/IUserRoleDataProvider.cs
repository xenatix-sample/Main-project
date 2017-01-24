using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.DataProvider.Admin
{
    public interface IUserRoleDataProvider
    {
        Response<UserRoleModel> GetUserRoles(int userID);
        Response<UserModel> SaveUserRoles(UserModel user);
    }
}
