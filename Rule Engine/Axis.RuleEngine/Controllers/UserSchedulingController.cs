using Axis.Constant;
using Axis.Model.Admin.UserScheduling;
using Axis.Model.Common;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.Security;
using System;
using System.Web.Http;


namespace Axis.RuleEngine.Service.Controllers
{
    public class UserSchedulingController : BaseApiController
    {
        #region Class Variables

        private readonly IUserSchedulingRuleEngine _userSchedulingRuleEngine = null;

        #endregion

        #region Constructors

        public UserSchedulingController(IUserSchedulingRuleEngine userSchedulingRuleEngine)
        {
            _userSchedulingRuleEngine = userSchedulingRuleEngine;
        }

        #endregion

        #region Site Admin Public Methods
        [HttpGet]
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Scheduling, Permission = Permission.Read)]
        public IHttpActionResult GetUserFacilities(int userID)
        {
            if (userID == 0)
                userID = AuthContext.Auth.User.UserID;

            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingRuleEngine.GetUserFacilities(userID), Request);
        }
       
        [HttpGet]
        [Authorization(PermissionKey =SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Scheduling, Permission = Permission.Read)]
        public IHttpActionResult GetUserFacilitySchedule(int userID, int facilityID)
        {
            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingRuleEngine.GetUserFacilitySchedule(userID, facilityID), Request);
        }
     
        [HttpPost]
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Scheduling, Permissions = new String[]{Permission.Create,Permission.Update})]
        public IHttpActionResult SaveUserFacilitySchedule(UserSchedulingModel userFacilitySchedule)
        {
            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingRuleEngine.SaveUserFacilitySchedule(userFacilitySchedule), Request);
        }
        #endregion Site Admin Public Methods

        #region My Profile Public Methods
        [HttpGet]
        public IHttpActionResult GetMyProfileFacilities(int userID)
        {
            if (userID == 0)
                userID = AuthContext.Auth.User.UserID;

            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingRuleEngine.GetUserFacilities(userID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetMyProfileFacilitySchedule(int userID, int facilityID)
        {
            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingRuleEngine.GetUserFacilitySchedule(userID, facilityID), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveMyProfileFacilitySchedule(UserSchedulingModel userFacilitySchedule)
        {
            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingRuleEngine.SaveUserFacilitySchedule(userFacilitySchedule), Request);
        }
        #endregion My Profile Public Methods

    }
}