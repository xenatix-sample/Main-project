using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using System;

namespace Axis.Plugins.Registration.Repository
{
    public interface IIntakeFormsRepository
    {

        /// <summary>
        /// Gets the intake form.
        /// </summary>
        /// <param name="contactID">The contact forms identifier.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        Response<FormsViewModel> GetIntakeForm(long contactFormsID);

        /// <summary>
        /// Gets the intake forms by contact id.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        Response<FormsViewModel> GetIntakeFormsByContactID(long contactID);

        /// <summary>
        /// Adds the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        Response<FormsViewModel> AddIntakeForms(FormsViewModel FormsModel);

        /// <summary>
        /// Updates the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        Response<FormsViewModel> UpdateIntakeForms(FormsViewModel FormsModel);

        /// <summary>
        /// Deletes the Forms.
        /// </summary>
        /// <param name="contactFormsID">The contact Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;FormsViewModel&gt;.</returns>
        Response<FormsViewModel> DeleteIntakeForms(long contactFormsID, DateTime modifiedOn);
    }

}
