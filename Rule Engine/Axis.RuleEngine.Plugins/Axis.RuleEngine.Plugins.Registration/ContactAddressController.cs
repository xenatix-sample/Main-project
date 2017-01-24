using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Common;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{

    public class ContactAddressController : BaseApiController
    {

        private readonly IContactAddressRuleEngine contactAddressRuleEngine;

        /// <summary>
        /// Constractor
        /// </summary>
        /// <param name="contactAddressRuleEngine"></param>
        public ContactAddressController(IContactAddressRuleEngine contactAddressRuleEngine)
        {
            this.contactAddressRuleEngine = contactAddressRuleEngine;
        }

        /// <summary>
        /// Get Contact Address
        /// </summary>
        /// <param name="ContactID"></param>
        /// <param name="contactTypeID"></param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.Intake, Module.CrisisLine, Module.LawLiaison, Module.Benefits,Module.BusinessAdministration}, PermissionKeys = new string[] {ChartPermissionKey.Chart_Chart_Assessment}, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAddresses(long ContactID, int contactTypeID)
        {
            return new HttpResult<Response<ContactAddressModel>>(contactAddressRuleEngine.GetAddresses(ContactID, contactTypeID), Request);
        }


        /// <summary>
        /// Add update contact address
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.CrisisLine, Module.LawLiaison }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddUpdateAddress(List<ContactAddressModel> model)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactAddressModel>>(contactAddressRuleEngine.AddUpdateAddress(model), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactAddressModel>() { DataItems = new List<ContactAddressModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactAddressModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Deletes the contact address.
        /// </summary>
        /// <param name="contactAddressID">The identifier.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.CrisisLine, Module.LawLiaison }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteAddress(long contactAddressID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactAddressModel>>(contactAddressRuleEngine.DeleteAddress(contactAddressID, modifiedOn), Request);
        }
    }
}
