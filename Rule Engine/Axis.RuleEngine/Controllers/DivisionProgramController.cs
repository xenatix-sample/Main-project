using Axis.RuleEngine.Admin.DivisionProgram;
using Axis.Model.Admin;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.Security;
using System.Web.Http;
using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;
using System;

namespace Axis.RuleEngine.Service.Controllers
{
    public class DivisionProgramController : BaseApiController
    {
        #region Class Variables

        private readonly IDivisionProgramRuleEngine _divisionProgramRuleEngine = null;

        #endregion

        #region Constructors

        public DivisionProgramController(IDivisionProgramRuleEngine divisionProgramRuleEngine)
        {
            _divisionProgramRuleEngine = divisionProgramRuleEngine;
        }

        #endregion

        #region Site Admin Public Methods
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DivisionPrograms, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetDivisionPrograms(int userID)
        {
            if (userID == 0)
                userID = AuthContext.Auth.User.UserID;

            return new HttpResult<Response<DivisionProgramModel>>(_divisionProgramRuleEngine.GetDivisionPrograms(userID), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DivisionPrograms, Permissions = new String[] { Permission.Create, Permission.Update,Permission.Delete })]
        [HttpPost]
        public IHttpActionResult SaveDivisionProgram(DivisionProgramModel divisionProgram)
        {
            return new HttpResult<Response<DivisionProgramModel>>(_divisionProgramRuleEngine.SaveDivisionProgram(divisionProgram), Request);
        }
        #endregion Site Admin Public Methods

        #region My Profile Public Methods
        [HttpGet]
        public IHttpActionResult GetMyProfileDivisionPrograms(int userID)
        {
            if (userID == 0)
                userID = AuthContext.Auth.User.UserID;

            return new HttpResult<Response<DivisionProgramModel>>(_divisionProgramRuleEngine.GetDivisionPrograms(userID), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveMyProfileDivisionProgram(DivisionProgramModel divisionProgram)
        {
            return new HttpResult<Response<DivisionProgramModel>>(_divisionProgramRuleEngine.SaveDivisionProgram(divisionProgram), Request);
        }
        #endregion My Profile Public Methods
    }
}