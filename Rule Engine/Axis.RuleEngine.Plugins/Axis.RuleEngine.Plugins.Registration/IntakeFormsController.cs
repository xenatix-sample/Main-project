using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration
{
    public class IntakeFormsController : BaseApiController
    {
        private readonly IIntakeFormsRuleEngine _IntakeFormsRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsController"/> class.
        /// </summary>
        /// <param name="FormsRuleEngine">The Forms rule engine.</param>
        public IntakeFormsController(IIntakeFormsRuleEngine intakeFormsRuleEngine)
        {
            this._IntakeFormsRuleEngine = intakeFormsRuleEngine;
        }

        /// <summary>
        /// Gets the Forms.
        /// </summary>
        /// <param name="contactID">The contact form identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        //[Authorization(PermissionKey = IntakeFormsPermissionKey.Intake_IDD_Forms, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetIntakeForm(long contactFormsID)
        {
            return new HttpResult<Response<FormsModel>>(_IntakeFormsRuleEngine.GetIntakeForm(contactFormsID), Request);
        }

        /// <summary>
        /// Gets the intake forms by contact id.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        //[Authorization(PermissionKey = IntakeFormsPermissionKey.Intake_IDD_Forms, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetIntakeFormsByContactID(long contactID)
        {
            return new HttpResult<Response<FormsModel>>(_IntakeFormsRuleEngine.GetIntakeFormsByContactID(contactID), Request);
        }

        /// <summary>
        /// Adds the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKey = IntakeFormsPermissionKey.Intake_IDD_Forms, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddIntakeForms(FormsModel FormsModel)
        {
            return new HttpResult<Response<FormsModel>>(_IntakeFormsRuleEngine.AddIntakeForms(FormsModel), Request);
        }

        /// <summary>
        /// Updates the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKey = IntakeFormsPermissionKey.Intake_IDD_Forms, Permissions = new string[] { Permission.Update, Permission.Create })]
        [HttpPut]
        public IHttpActionResult UpdateIntakeForms(FormsModel FormsModel)
        {
            return new HttpResult<Response<FormsModel>>(_IntakeFormsRuleEngine.UpdateIntakeForms(FormsModel), Request);
        }

        /// <summary>
        /// Deletes the Forms.
        /// </summary>
        /// <param name="contactFormsID">The contact Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKey = IntakeFormsPermissionKey.Intake_IDD_Forms, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteIntakeForms(long contactFormsID, DateTime modifiedOn)
        {
            return new HttpResult<Response<FormsModel>>(_IntakeFormsRuleEngine.DeleteIntakeForms(contactFormsID, modifiedOn), Request);
        }
    }
}
