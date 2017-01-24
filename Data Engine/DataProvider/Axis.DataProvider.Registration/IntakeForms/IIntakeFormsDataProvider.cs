using Axis.Model.Common;
using Axis.Model.Registration;
using System;

namespace Axis.DataProvider.Registration
{
   public interface IIntakeFormsDataProvider
    {

        /// <summary>
        /// Gets the intake form.
        /// </summary>
        /// <param name="contactFormsID">The intake form identifier.</param>
        /// <returns></returns>
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
        /// Updates the intake Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        Response<FormsModel> UpdateIntakeForms(FormsModel FormsModel);

        /// <summary>
        /// Deletes the intake Forms.
        /// </summary>
        /// <param name="contactFormsID">The intake Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        Response<FormsModel> DeleteIntakeForms(long contactFormsID, DateTime modifiedOn);
    }
}
