using System;
using System.Web.Http;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;
using Axis.RuleEngine.Clinical.MedicalHistory;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    public class MedicalHistoryController : BaseApiController
    {
        #region Class Variables

        readonly IMedicalHistoryRuleEngine _medicalHistoryRuleEngine = null;

        #endregion

        #region Constructors

        public MedicalHistoryController(IMedicalHistoryRuleEngine medicalHistoryRuleEngine)
        {
            _medicalHistoryRuleEngine = medicalHistoryRuleEngine;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        public IHttpActionResult GetMedicalHistoryBundle(long contactID)
        {
            return new HttpResult<Response<MedicalHistoryModel>>(_medicalHistoryRuleEngine.GetMedicalHistoryBundle(contactID), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        public IHttpActionResult GetMedicalHistoryConditionDetails(long medicalHistoryID)
        {
            return new HttpResult<Response<MedicalHistoryModel>>(_medicalHistoryRuleEngine.GetMedicalHistoryConditionDetails(medicalHistoryID), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        public IHttpActionResult GetAllMedicalConditions(long medicalHistoryID)
        {
            return new HttpResult<Response<MedicalHistoryModel>>(_medicalHistoryRuleEngine.GetAllMedicalConditions(medicalHistoryID), Request);
        }

        [HttpDelete]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Delete)]
        public IHttpActionResult DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn)
        {
            return new HttpResult<Response<MedicalHistoryModel>>(_medicalHistoryRuleEngine.DeleteMedicalHistory(medicalHistoryID, modifiedOn), Request);
        }

        [HttpPost]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Create)]
        public IHttpActionResult AddMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            return new HttpResult<Response<MedicalHistoryModel>>(_medicalHistoryRuleEngine.AddMedicalHistory(medicalHistory), Request);
        }

        [HttpPost]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Create)]
        public IHttpActionResult UpdateMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            return new HttpResult<Response<MedicalHistoryModel>>(_medicalHistoryRuleEngine.UpdateMedicalHistory(medicalHistory), Request);
        }

        [HttpPost]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Create)]
        public IHttpActionResult SaveMedicalHistoryDetails(MedicalHistoryModel medicalHistory)
        {
            return new HttpResult<Response<MedicalHistoryModel>>(_medicalHistoryRuleEngine.SaveMedicalHistoryDetails(medicalHistory), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        public IHttpActionResult GetMedicalHistory(long medicalHistoryID)
        {
            return new HttpResult<Response<MedicalHistoryModel>>(_medicalHistoryRuleEngine.GetMedicalHistory(medicalHistoryID), Request);
        }
        #endregion
    }
}
