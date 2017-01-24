using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactAliasRuleEngine
    {
        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactAliasModel> GetContactAlias(long contactID);

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        Response<ContactAliasModel> AddContactAlias(List<ContactAliasModel> contactAlias);

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        Response<ContactAliasModel> UpdateContactAlias(List<ContactAliasModel> contactAlias);

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<ContactAliasModel> DeleteContactAlias(long contactAliasID, DateTime modifiedOn);
    }
}