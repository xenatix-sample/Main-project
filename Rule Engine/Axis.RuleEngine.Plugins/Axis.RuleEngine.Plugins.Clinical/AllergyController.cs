using System;
using System.Web.Http;
using Axis.Model.Clinical.Allergy;
using Axis.Model.Common;
using Axis.RuleEngine.Clinical.Allergy;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    public class AllergyController : BaseApiController
    {
        #region Class Variables

        readonly IAllergyRuleEngine _allergyRuleEngine = null;

        #endregion

        #region Constructors

        public AllergyController(IAllergyRuleEngine allergyRuleEngine)
        {
            _allergyRuleEngine = allergyRuleEngine;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Read)]
        public IHttpActionResult GetAllergyBundle(long contactID, Int16 allergyTypeID)
        {
            return new HttpResult<Response<ContactAllergyModel>>(_allergyRuleEngine.GetAllergyBundle(contactID, allergyTypeID), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Read)]
        public IHttpActionResult GetAllergy(long contactAllergyID)
        {
            return new HttpResult<Response<ContactAllergyModel>>(_allergyRuleEngine.GetAllergy(contactAllergyID), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Read)]
        public IHttpActionResult GetAllergyDetails(long contactAllergyID, Int16 allergyTypeID)
        {
            return new HttpResult<Response<ContactAllergyDetailsModel>>(_allergyRuleEngine.GetAllergyDetails(contactAllergyID, allergyTypeID), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Read)]
        public IHttpActionResult GetTopAllergies(long contactID)
        {
            return new HttpResult<Response<ContactAllergyHeaderModel>>(_allergyRuleEngine.GetTopAllergies(contactID), Request);
        }

        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Delete)]
        public IHttpActionResult DeleteAllergy(long contactAllergyID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactAllergyModel>>(_allergyRuleEngine.DeleteAllergy(contactAllergyID, modifiedOn), Request);
        }

        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Delete)]
        public IHttpActionResult DeleteAllergyDetail(long contactAllergyDetailID, string reasonForDeletion, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactAllergyDetailsModel>>(_allergyRuleEngine.DeleteAllergyDetail(contactAllergyDetailID, reasonForDeletion, modifiedOn), Request);
        }

        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Create)]
        public IHttpActionResult AddAllergy(ContactAllergyModel allergy)
        {
            return new HttpResult<Response<ContactAllergyModel>>(_allergyRuleEngine.AddAllergy(allergy), Request);
        }

        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Create)]
        public IHttpActionResult AddAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            return new HttpResult<Response<ContactAllergyDetailsModel>>(_allergyRuleEngine.AddAllergyDetail(allergy), Request);
        }

        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Update)]
        public IHttpActionResult UpdateAllergy(ContactAllergyModel allergy)
        {
            return new HttpResult<Response<ContactAllergyModel>>(_allergyRuleEngine.UpdateAllergy(allergy), Request);
        }

        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Allergy_Allergy, Permission = Permission.Update)]
        public IHttpActionResult UpdateAllergyDetail(ContactAllergyDetailsModel allergy)
        {
            return new HttpResult<Response<ContactAllergyDetailsModel>>(_allergyRuleEngine.UpdateAllergyDetail(allergy), Request);
        }

        #endregion
    }
}
