using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;

namespace Axis.RuleEngine.Registration
{
    public class IntakeFormsRuleEngine : IIntakeFormsRuleEngine
    {
         private readonly IIntakeFormsService _IntakeFormsService;

        /// <summary>
         /// Initializes a new instance of the <see cref="IntakeFormsRuleEngine"/> class.
        /// </summary>
         /// <param name="IntakeFormsRuleEngine">The Intake Forms service.</param>
         public IntakeFormsRuleEngine(IIntakeFormsService FormsService)
        {
            this._IntakeFormsService = FormsService;
        }

        /// <summary>
        /// Gets the Forms.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
         public Response<FormsModel> GetIntakeForm(long contactFormsID)
        {
            return _IntakeFormsService.GetIntakeForm(contactFormsID);
        }

        /// <summary>
        /// Gets the Forms.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> GetIntakeFormsByContactID(long contactID)
        {
            return _IntakeFormsService.GetIntakeFormsByContactID(contactID);
        }

        /// <summary>
        /// Adds the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> AddIntakeForms(FormsModel FormsModel)
        {
            return _IntakeFormsService.AddIntakeForms(FormsModel);
        }

        /// <summary>
        /// Updates the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> UpdateIntakeForms(FormsModel FormsModel)
        {
            return _IntakeFormsService.UpdateIntakeForms(FormsModel);
        }

        /// <summary>
        /// Deletes the Forms.
        /// </summary>
        /// <param name="contactFormsID">The contact Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> DeleteIntakeForms(long contactFormsID, DateTime modifiedOn)
        {
            return _IntakeFormsService.DeleteIntakeForms(contactFormsID, modifiedOn);
        }
    }
}
