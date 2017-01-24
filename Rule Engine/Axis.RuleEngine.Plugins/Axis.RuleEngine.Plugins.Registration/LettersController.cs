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
    public class LettersController : BaseApiController
    {
        private readonly ILettersRuleEngine _lettersRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="LettersController"/> class.
        /// </summary>
        /// <param name="lettersRuleEngine">The letters rule engine.</param>
        public LettersController(ILettersRuleEngine lettersRuleEngine)
        {
            this._lettersRuleEngine = lettersRuleEngine;
        }

        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKeys = new string[] { LettersPermissionKey.Intake_IDD_Letters, IntakeFormsPermissionKey.Intake_IDD_Forms }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetLetters(long contactID)
        {
            return new HttpResult<Response<LettersModel>>(_lettersRuleEngine.GetLetters(contactID), Request);
        }

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKeys = new string[] { LettersPermissionKey.Intake_IDD_Letters, IntakeFormsPermissionKey.Intake_IDD_Forms }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddLetters(LettersModel lettersModel)
        {
            return new HttpResult<Response<LettersModel>>(_lettersRuleEngine.AddLetters(lettersModel), Request);
        }

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKeys = new string[] { LettersPermissionKey.Intake_IDD_Letters, IntakeFormsPermissionKey.Intake_IDD_Forms }, Permissions = new string[] { Permission.Update, Permission.Create })]
        [HttpPut]
        public IHttpActionResult UpdateLetters(LettersModel lettersModel)
        {
            return new HttpResult<Response<LettersModel>>(_lettersRuleEngine.UpdateLetters(lettersModel), Request);
        }

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The contact letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKeys = new string[] { LettersPermissionKey.Intake_IDD_Letters, IntakeFormsPermissionKey.Intake_IDD_Forms }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteLetters(long contactLettersID, DateTime modifiedOn)
        {
            return new HttpResult<Response<LettersModel>>(_lettersRuleEngine.DeleteLetters(contactLettersID, modifiedOn), Request);
        }
    }
}
