using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactAliasRepository
    {
        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactAliasViewModel> GetContactAlias(long contactID);

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        Response<ContactAliasViewModel> AddContactAlias(List<ContactAliasViewModel> contactAlias);

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        Response<ContactAliasViewModel> UpdateContactAlias(List<ContactAliasViewModel> contactAlias);

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<ContactAliasViewModel> DeleteContactAlias(long contactAliasID, DateTime modifiedOn);
    }
}