using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Service.Admin;

namespace Axis.RuleEngine.Admin
{
    public class StaffManagementRuleEngine : IStaffManagementRuleEngine
    {
        #region Class Variables

        private readonly IStaffManagementService _staffManagementService;

        #endregion

        #region Constructors

        public StaffManagementRuleEngine(IStaffManagementService staffManagementService)
        {
            _staffManagementService = staffManagementService;
        }

        #endregion

        #region Public Methods

        public Response<UserModel> GetStaff(string searchText)
        {
            return _staffManagementService.GetStaff(searchText);
        }

        public Response<UserModel> DeleteUser(int userID)
        {
            return _staffManagementService.DeleteUser(userID);
        }

        public Response<UserModel> ActivateUser(int userID)
        {
            return _staffManagementService.ActivateUser(userID);
        }

        public Response<UserModel> UnlockUser(int userID)
        {
            return _staffManagementService.UnlockUser(userID);
        }

        #endregion
    }
}
