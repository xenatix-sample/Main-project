using Axis.Model.Account;
using Axis.Model.Common;

namespace Axis.RuleEngine.Admin
{
    public interface IStaffManagementRuleEngine
    {
        Response<UserModel> GetStaff(string searchText);
        Response<UserModel> DeleteUser(int userID);
        Response<UserModel> ActivateUser(int userID);
        Response<UserModel> UnlockUser(int userID);
    }
}
