using System;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for collateral
    /// </summary>
    public class CollateralController : BaseApiController
    {
        readonly ICollateralRuleEngine collateralRuleEngine;

        public CollateralController(ICollateralRuleEngine collateralRuleEngine)
        {
            this.collateralRuleEngine = collateralRuleEngine;
        }

        /// <summary>
        /// Get collateral list for contact
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Contact Type Id</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Collateral, ECIPermissionKey.ECI_Registration_Collateral, ConsentsPermissionKey.Consents_Assessment_Agency, BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge }, Modules = new string[] { Module.General, Module.Benefits, Module.Consents, Module.Intake, Module.Chart }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult Getcollaterals(long contactID, int contactTypeId, bool getContactDetails)
        {
            return new HttpResult<Response<CollateralModel>>(collateralRuleEngine.GetCollaterals(contactID, contactTypeId, getContactDetails), Request);
        }

        /// <summary>
        /// Add collateral for contact
        /// </summary>
        /// <param name="collateralModel">collateral model</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Collateral, ECIPermissionKey.ECI_Registration_Collateral, GeneralPermissionKey.General_General_Collateral }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult Addcollateral(CollateralModel collateralModel)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<CollateralModel>>(collateralRuleEngine.AddCollateral(collateralModel), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<CollateralModel>() { DataItems = new List<CollateralModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<CollateralModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Update collateral for model
        /// </summary>
        /// <param name="collateralModel">collateral model</param>
        /// <returns></returns>
        /// 

        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Collateral, ECIPermissionKey.ECI_Registration_Collateral, GeneralPermissionKey.General_General_Collateral }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult Updatecollateral(CollateralModel collateralModel)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<CollateralModel>>(collateralRuleEngine.UpdateCollateral(collateralModel), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<CollateralModel>() { DataItems = new List<CollateralModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<CollateralModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Delete collateral
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Collateral, ECIPermissionKey.ECI_Registration_Collateral, GeneralPermissionKey.General_General_Collateral }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult Deletecollateral(long parentContactID, long contactID, DateTime modifiedOn)
        {
            return new HttpResult<Response<CollateralModel>>(collateralRuleEngine.DeleteCollateral(parentContactID, contactID, modifiedOn), Request);
        }
    }
}
