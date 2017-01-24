using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Service.Registration
{
    public interface IIntakeFormsService
    {
        
        /// <summary>
        /// Gets the intake Form.
        /// </summary>
        /// <param name="contactID">The contact forms identifier.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        Response<FormsModel> GetIntakeForm(long contactFormsID);

        /// <summary>
        /// Gets the Forms.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        Response<FormsModel> GetIntakeFormsByContactID(long contactID);

        /// <summary>
        /// Adds the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        Response<FormsModel> AddIntakeForms(FormsModel FormsModel);

        /// <summary>
        /// Updates the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        Response<FormsModel> UpdateIntakeForms(FormsModel FormsModel);

        /// <summary>
        /// Deletes the Forms.
        /// </summary>
        /// <param name="contactFormsID">The contact Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        Response<FormsModel> DeleteIntakeForms(long contactFormsID, DateTime modifiedOn);
    }
}
