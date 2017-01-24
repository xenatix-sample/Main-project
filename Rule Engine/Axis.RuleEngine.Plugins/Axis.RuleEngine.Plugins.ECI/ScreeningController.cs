using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.RuleEngine.ECI;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.Constant;

namespace Axis.RuleEngine.Service.Controllers
{
   
    public class ScreeningController : BaseApiController
    {
        private readonly IScreeningRuleEngine _eciRuleEngine;

        public ScreeningController(IScreeningRuleEngine eciRuleEngine)
        {
            _eciRuleEngine = eciRuleEngine;
        }

        #region Public Methods
        [Authorization(PermissionKey = ECIPermissionKey.ECI_Screening_Screening, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddScreening(ScreeningModel screening)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciRuleEngine.AddScreening(screening), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Screening_Screening, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetScreenings(long contactID)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciRuleEngine.GetScreenings(contactID), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Screening_Screening, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetScreening(long screeningID)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciRuleEngine.GetScreening(screeningID), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Screening_Screening, Permission = Permission.Update)]
        [HttpPost]
        public IHttpActionResult UpdateScreening(ScreeningModel screening)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciRuleEngine.UpdateScreening(screening), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Screening_Screening, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult RemoveScreening(long screeningID, DateTime modifiedOn)
        {
            return new HttpResult<Response<bool>>(_eciRuleEngine.RemoveScreening(screeningID, modifiedOn), Request);
        }

        #endregion


    }
}