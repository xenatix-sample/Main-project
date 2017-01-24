using System.Web.Http;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class StaffManagementController : BaseApiController
    {
        #region Class Variables

        private readonly IStaffManagementRepository _staffManagementRepository;

        #endregion

        #region Constructors

        public StaffManagementController(IStaffManagementRepository staffManagementRepository)
        {
            _staffManagementRepository = staffManagementRepository;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public Response<UserViewModel> GetStaff(string searchText)
        {
            return _staffManagementRepository.GetStaff(searchText);
        }

        [HttpDelete]
        public Response<UserViewModel> DeleteUser(int userID)
        {
            return _staffManagementRepository.DeleteUser(userID);
        }

        [HttpPost]
        public Response<UserViewModel> ActivateUser(int userID)
        {
            return _staffManagementRepository.ActivateUser(userID);
        }

        [HttpPost]
        public Response<UserViewModel> UnlockUser(int userID)
        {
            return _staffManagementRepository.UnlockUser(userID);
        }

        #endregion
    }
}