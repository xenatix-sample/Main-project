using Axis.Constant;
using Axis.Helpers.Validation;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for Contact Relationship
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Helpers.Controllers.BaseApiController" />
    public class ContactRelationshipController : BaseApiController
    {
        /// <summary>
        /// The contact Relationship rule engine
        /// </summary>
        private readonly IContactRelationshipRuleEngine _contactRelationshipRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRelationshipController" /> class.
        /// </summary>
        /// <param name="contactRelationshipRuleEngine">The contact Relationship rule engine.</param>
        public ContactRelationshipController(IContactRelationshipRuleEngine contactRelationshipRuleEngine)
        {
            this._contactRelationshipRuleEngine = contactRelationshipRuleEngine;
        }

        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>

        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Collateral, ECIPermissionKey.ECI_Registration_Collateral, GeneralPermissionKey.General_General_Collateral, BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge }, Permission = Permission.Read)]

        [HttpGet]
        public IHttpActionResult GetContactRelationship(long contactID, long parentContactID)
        {
            return new HttpResult<Response<ContactRelationshipModel>>(_contactRelationshipRuleEngine.GetContactRelationship(contactID,parentContactID), Request);
        }

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>

        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Collateral, ECIPermissionKey.ECI_Registration_Collateral, GeneralPermissionKey.General_General_Collateral }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactRelationshipModel>>(_contactRelationshipRuleEngine.AddContactRelationship(contactRelationship), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactRelationshipModel>() { DataItems = new List<ContactRelationshipModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactRelationshipModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>


        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Collateral, ECIPermissionKey.ECI_Registration_Collateral, GeneralPermissionKey.General_General_Collateral }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPut]
        public IHttpActionResult UpdateContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactRelationshipModel>>(_contactRelationshipRuleEngine.UpdateContactRelationship(contactRelationship), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactRelationshipModel>() { DataItems = new List<ContactRelationshipModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactRelationshipModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipTypeID">The contact Relationship identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>

        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Collateral, ECIPermissionKey.ECI_Registration_Collateral, GeneralPermissionKey.General_General_Collateral }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactRelationshipModel>>(_contactRelationshipRuleEngine.DeleteContactRelationship(contactRelationshipTypeID, modifiedOn), Request);
        }
    }
}