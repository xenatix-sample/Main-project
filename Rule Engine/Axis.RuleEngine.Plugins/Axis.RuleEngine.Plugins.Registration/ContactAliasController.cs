using Axis.Helpers.Validation;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for Contact Alias
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Helpers.Controllers.BaseApiController" />
    public class ContactAliasController : BaseApiController
    {
        /// <summary>
        /// The contact alias rule engine
        /// </summary>
        private readonly IContactAliasRuleEngine contactAliasRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAliasController" /> class.
        /// </summary>
        /// <param name="contactAliasRuleEngine">The contact alias rule engine.</param>
        public ContactAliasController(IContactAliasRuleEngine contactAliasRuleEngine)
        {
            this.contactAliasRuleEngine = contactAliasRuleEngine;
        }

        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactAlias(long contactID)
        {
            return new HttpResult<Response<ContactAliasModel>>(contactAliasRuleEngine.GetContactAlias(contactID), Request);
        }

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddContactAlias(List<ContactAliasModel> contactAlias)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactAliasModel>>(contactAliasRuleEngine.AddContactAlias(contactAlias), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactAliasModel>() { DataItems = new List<ContactAliasModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactAliasModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateContactAlias(List<ContactAliasModel> contactAlias)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactAliasModel>>(contactAliasRuleEngine.UpdateContactAlias(contactAlias), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactAliasModel>() { DataItems = new List<ContactAliasModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactAliasModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact alias identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteContactAlias(long contactAliasID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactAliasModel>>(contactAliasRuleEngine.DeleteContactAlias(contactAliasID, modifiedOn), Request);
        }
    }
}