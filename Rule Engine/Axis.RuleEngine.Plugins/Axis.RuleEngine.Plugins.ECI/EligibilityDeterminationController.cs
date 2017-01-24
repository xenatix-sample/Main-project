using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.RuleEngine.ECI.EligibilityDetermination;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.ECI
{
    
    public class EligibilityDeterminationController : BaseApiController
    {
        #region Class Variables

        readonly IEligibilityDeterminationRuleEngine _eligibilityDeterminationRuleEngine = null;

        #endregion

        #region Constructors

        public EligibilityDeterminationController(IEligibilityDeterminationRuleEngine eligibilityDeterminationRuleEngine)
        {
            _eligibilityDeterminationRuleEngine = eligibilityDeterminationRuleEngine;
        }

        #endregion

        #region Public Methods
        [Authorization(PermissionKey = ECIPermissionKey.ECI_Eligibility_Eligibility, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetEligibilityDetermination(long contactID)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationRuleEngine.GetEligibilityDetermination(contactID), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Eligibility_Eligibility, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetEligibility(long eligibilityID)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationRuleEngine.GetEligibility(eligibilityID), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Eligibility_Eligibility, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetContactEligibilityMembers(long contactID)
        {
            return new HttpResult<Response<EligibilityTeamMemberModel>>(_eligibilityDeterminationRuleEngine.GetContactEligibilityMembers(contactID), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Eligibility_Eligibility, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetEligibilityNote(long eligibilityID)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationRuleEngine.GetEligibilityNote(eligibilityID), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Eligibility_Eligibility, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationRuleEngine.AddEligibility(eligibilityDetermination), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Eligibility_Eligibility, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationRuleEngine.UpdateEligibility(eligibilityDetermination), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Eligibility_Eligibility, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult SaveEligibilityNote(EligibilityDeterminationModel eligibilityDetermination)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationRuleEngine.SaveEligibilityNote(eligibilityDetermination), Request);
        }

        [Authorization(PermissionKey = ECIPermissionKey.ECI_Eligibility_Eligibility, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeactivateEligibility(long eligibilityID, DateTime modifiedOn)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationRuleEngine.DeactivateEligibility(eligibilityID, modifiedOn), Request);
        }

        #endregion
    }
}
