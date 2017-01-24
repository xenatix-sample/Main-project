using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.DataProvider.Admin
{
    public interface IStaffManagementDataProvider
    {
        Response<UserModel> GetStaff(string searchText);
        Response<UserModel> DeleteUser(int userID);
        Response<UserModel> ActivateUser(int userID);
        Response<UserModel> UnlockUser(int userID);
    }
}
