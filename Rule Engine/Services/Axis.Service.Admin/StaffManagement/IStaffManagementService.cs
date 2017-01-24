using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.Service.Admin
{
    public interface IStaffManagementService
    {
        Response<UserModel> GetStaff(string searchText);
        Response<UserModel> DeleteUser(int userID);
        Response<UserModel> ActivateUser(int userID);
        Response<UserModel> UnlockUser(int userID);
    }
}
