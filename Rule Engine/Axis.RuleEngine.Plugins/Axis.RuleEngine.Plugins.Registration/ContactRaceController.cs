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
    /// Controller for Contact Race
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Helpers.Controllers.BaseApiController" />
    public class ContactRaceController : BaseApiController
    {
        /// <summary>
        /// The contact Race rule engine
        /// </summary>
        private readonly IContactRaceRuleEngine contactRaceRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRaceController" /> class.
        /// </summary>
        /// <param name="contactRaceRuleEngine">The contact Race rule engine.</param>
        public ContactRaceController(IContactRaceRuleEngine contactRaceRuleEngine)
        {
            this.contactRaceRuleEngine = contactRaceRuleEngine;
        }

        /// <summary>
        /// Gets the contact Race.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactRace(long contactID)
        {
            return new HttpResult<Response<ContactRaceModel>>(contactRaceRuleEngine.GetContactRace(contactID), Request);
        }

        /// <summary>
        /// Adds the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddContactRace(List<ContactRaceModel> contactRace)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactRaceModel>>(contactRaceRuleEngine.AddContactRace(contactRace), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactRaceModel>() { DataItems = new List<ContactRaceModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactRaceModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateContactRace(List<ContactRaceModel> contactRace)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactRaceModel>>(contactRaceRuleEngine.UpdateContactRace(contactRace), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactRaceModel>() { DataItems = new List<ContactRaceModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactRaceModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Deletes the contact Race.
        /// </summary>
        /// <param name="contactRaceID">The contact Race identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteContactRace(long contactRaceID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactRaceModel>>(contactRaceRuleEngine.DeleteContactRace(contactRaceID, modifiedOn), Request);
        }
    }
}