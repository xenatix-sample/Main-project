using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    public class IntakeFormsController : BaseApiController
    {
        private readonly IIntakeFormsRepository _IntakeFormsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsController"/> class.
        /// </summary>
        /// <param name="FormsRepository">The Forms repository.</param>
        public IntakeFormsController(IIntakeFormsRepository IntakeFormsRepository)
        {
            this._IntakeFormsRepository = IntakeFormsRepository;
        }

        /// <summary>
        /// Gets the intake form.
        /// </summary>
        /// <param name="contactID">The contact forms identifier.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        [HttpGet]
        public Response<FormsViewModel> GetIntakeForm(long contactFormsID)
        {
            return _IntakeFormsRepository.GetIntakeForm(contactFormsID);
        }

        /// <summary>
        /// Gets the intake forms by contact id.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        [HttpGet]
        public Response<FormsViewModel> GetIntakeFormsByContactID(long contactID)
        {
            return _IntakeFormsRepository.GetIntakeFormsByContactID(contactID);
        }

        /// <summary>
        /// Adds the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        [HttpPost]
        public Response<FormsViewModel> AddIntakeForms(FormsViewModel FormsModel)
        {
            return _IntakeFormsRepository.AddIntakeForms(FormsModel);
        }

        /// <summary>
        /// Updates the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        [HttpPut]
        public Response<FormsViewModel> UpdateIntakeForms(FormsViewModel FormsModel)
        {
            return _IntakeFormsRepository.UpdateIntakeForms(FormsModel);
        }

        /// <summary>
        /// Deletes the Forms.
        /// </summary>
        /// <param name="contactFormsID">The contact Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        [HttpDelete]
        public Response<FormsViewModel> DeleteIntakeForms(long contactFormsID, DateTime modifiedOn)
        {
            return _IntakeFormsRepository.DeleteIntakeForms(contactFormsID, modifiedOn);
        }
    }
}
