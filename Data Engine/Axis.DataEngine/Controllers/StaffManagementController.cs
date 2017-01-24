using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Admin;
using Axis.Model.Account;
using Axis.Model.Admin;
using Axis.Model.Common;

namespace Axis.DataEngine.Service.Controllers
{
    public class StaffManagementController : BaseApiController
    {
        #region Class Variables

        readonly IStaffManagementDataProvider _staffManagementDataProvider = null;

        #endregion

        #region Constructors

        public StaffManagementController(IStaffManagementDataProvider staffManagementDataProvider)
        {
            _staffManagementDataProvider = staffManagementDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetStaff(string searchText)
        {
            var staffResponse = _staffManagementDataProvider.GetStaff(searchText);
            return new HttpResult<Response<UserModel>>(staffResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteUser(int userID)
        {
            var staffResponse = _staffManagementDataProvider.DeleteUser(userID);
            return new HttpResult<Response<UserModel>>(staffResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult ActivateUser(int userID)
        {
            var staffResponse = _staffManagementDataProvider.ActivateUser(userID);
            return new HttpResult<Response<UserModel>>(staffResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult UnlockUser(int userID)
        {
            var staffResponse = _staffManagementDataProvider.UnlockUser(userID);
            return new HttpResult<Response<UserModel>>(staffResponse, Request);
        }

        #endregion
    }
}