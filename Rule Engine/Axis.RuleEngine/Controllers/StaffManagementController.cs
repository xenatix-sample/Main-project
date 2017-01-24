using System.Web.Http;
using Axis.Model.Account;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.Security;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;
using System;

namespace Axis.RuleEngine.Service.Controllers
{
    public class StaffManagementController : BaseApiController
    {
        #region Class Variables

        private readonly IStaffManagementRuleEngine _staffManagementRuleEngine = null;

        #endregion

        #region Constructors

        public StaffManagementController(IStaffManagementRuleEngine staffManagementRuleEngine)
        {
            _staffManagementRuleEngine = staffManagementRuleEngine;
        }

        #endregion

        #region Public Methods
        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserRoles,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DivisionPrograms,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Scheduling,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserProfile,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Credentials,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports}, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetStaff(string searchText)
        {
            int userID = AuthContext.Auth.User.UserID;
            Response<UserModel> responseObject = _staffManagementRuleEngine.GetStaff(searchText);
            responseObject.DataItems.Remove(responseObject.DataItems.Find(x => x.UserID == userID));

            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserRoles,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DivisionPrograms,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Scheduling,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserProfile,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Credentials,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports}, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteUser(int userID)
        {
            Response<UserModel> responseObject = _staffManagementRuleEngine.DeleteUser(userID);
            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserRoles,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DivisionPrograms,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Scheduling,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserProfile,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Credentials,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports}, Permission = Permission.Update)]
        [HttpPost]
        public IHttpActionResult ActivateUser(int userID)
        {
            Response<UserModel> responseObject = _staffManagementRuleEngine.ActivateUser(userID);
            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserRoles,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DivisionPrograms,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Scheduling,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserProfile,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Credentials,
        SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports}, Permission = Permission.Update)]
        [HttpPost]
        public IHttpActionResult UnlockUser(int userID)
        {
            Response<UserModel> responseObject = _staffManagementRuleEngine.UnlockUser(userID);
            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        #endregion
    }
}