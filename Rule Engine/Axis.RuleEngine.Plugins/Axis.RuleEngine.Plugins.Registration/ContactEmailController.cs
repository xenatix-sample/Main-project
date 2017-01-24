using System;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for Email
    /// </summary>
    public class ContactEmailController : BaseApiController
    {
       private readonly IContactEmailRuleEngine contactEmailRuleEngine;

        public ContactEmailController(IContactEmailRuleEngine contactEmailRuleEngine)
        {
            this.contactEmailRuleEngine = contactEmailRuleEngine;
        }

        /// <summary>
        /// Get Email list for contact
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Contact Type Id</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.LawLiaison, Module.CrisisLine, Module.BusinessAdministration }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetEmails(long ContactID, int contactTypeID)
        {
            return new HttpResult<Response<ContactEmailModel>>(contactEmailRuleEngine.GetEmails(ContactID, contactTypeID), Request);
        }


        /// <summary>
        /// Update Email for model
        /// </summary>
        /// <param name="ContactEmailModel">Email model</param>
        /// <returns></returns>
        /// 
        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.LawLiaison }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddUpdateEmail(List<ContactEmailModel> contact)
        {            
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactEmailModel>>(contactEmailRuleEngine.AddUpdateEmails( contact), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactEmailModel>() { DataItems = new List<ContactEmailModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactEmailModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Delete Email
        /// </summary>
        /// <param name="Id">Contact Id of Email</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General, Module.Registration, Module.ECI, Module.LawLiaison }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteEmail(long contactEmailID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactEmailModel>>(contactEmailRuleEngine.DeleteEmail(contactEmailID, modifiedOn), Request);
        }
    }
}
