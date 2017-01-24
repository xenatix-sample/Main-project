using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public interface IStaffManagementRepository
    {
        Response<UserViewModel> GetStaff(string searchText);
        Response<UserViewModel> DeleteUser(int userID);
        Response<UserViewModel> ActivateUser(int userID);
        Response<UserViewModel> UnlockUser(int userID);
    }
}