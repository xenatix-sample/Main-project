using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Collections.Generic;
using Axis.Helpers.Validation;
using System.Web.Http;
using System;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;

namespace Axis.RuleEngine.Plugins.Registration
{
    public class ContactDischargeNoteController : BaseApiController
    {
              /// <summary>
        /// The contact discharge note rule engine
        /// </summary>
        private readonly IContactDischargeNoteRuleEngine contactDischargeNoteRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController" /> class.
        /// </summary>
        /// <param name="contactRuleEngine">The contact  rule engine.</param>
        public ContactDischargeNoteController(IContactDischargeNoteRuleEngine contactDischargeNoteRuleEngine)
        {
            this.contactDischargeNoteRuleEngine = contactDischargeNoteRuleEngine;
        }

        /// <summary>
        /// Gets the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_CompanyDischarge, GeneralPermissionKey.General_General_ProgramUnitDischarge }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetContactDischargeNote(long contactDischargeNoteID)
        {
            return new HttpResult<Response<ContactDischargeNote>>(contactDischargeNoteRuleEngine.GetContactDischargeNote(contactDischargeNoteID), Request);
        }

        /// <summary>
        /// Gets the contact discharge notes.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_CompanyDischarge, GeneralPermissionKey.General_General_ProgramUnitDischarge }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetContactDischargeNotes(long contactDischargeNoteID,int noteTypeID)
        {
            return new HttpResult<Response<ContactDischargeNote>>(contactDischargeNoteRuleEngine.GetContactDischargeNotes(contactDischargeNoteID, noteTypeID), Request);
        }

        /// <summary>
        /// Adds the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_CompanyDischarge, GeneralPermissionKey.General_General_ProgramUnitDischarge }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactDischargeNote>>(contactDischargeNoteRuleEngine.AddContactDischargeNote(contactDischargeNote), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactDischargeNote>() { DataItems = new List<ContactDischargeNote>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactDischargeNote>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNote">The contact discharge note.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_CompanyDischarge, GeneralPermissionKey.General_General_ProgramUnitDischarge }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactDischargeNote>>(contactDischargeNoteRuleEngine.UpdateContactDischargeNote(contactDischargeNote), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactDischargeNote>() { DataItems = new List<ContactDischargeNote>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactDischargeNote>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// delete the contact discharge note.
        /// </summary>
        /// <param name="contactDischargeNoteID">The contact discharge note identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_CompanyDischarge, GeneralPermissionKey.General_General_ProgramUnitDischarge }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteContactDischargeNote(long contactDischargeNoteID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactDischargeNote>>(contactDischargeNoteRuleEngine.DeleteContactDischargeNote(contactDischargeNoteID, modifiedOn), Request);
        }
    }
}
