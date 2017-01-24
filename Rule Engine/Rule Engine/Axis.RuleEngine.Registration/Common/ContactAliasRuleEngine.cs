using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// Rule engine class for calling service method of contact alias Service
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Registration.IContactAliasRuleEngine" />
    public class ContactAliasRuleEngine : IContactAliasRuleEngine
    {
        /// <summary>
        /// The contact alias service
        /// </summary>
        private readonly IContactAliasService contactAliasService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAliasRuleEngine"/> class.
        /// </summary>
        /// <param name="contactAliasService">The contact alias service.</param>
        public ContactAliasRuleEngine(IContactAliasService contactAliasService)
        {
            this.contactAliasService = contactAliasService;
        }

        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactAliasModel> GetContactAlias(long contactID)
        {
            return contactAliasService.GetContactAlias(contactID);
        }

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        public Response<ContactAliasModel> AddContactAlias(List<ContactAliasModel> contactAlias)
        {
            return contactAliasService.AddContactAlias(contactAlias);
        }

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        public Response<ContactAliasModel> UpdateContactAlias(List<ContactAliasModel> contactAlias)
        {
            return contactAliasService.UpdateContactAlias(contactAlias);
        }

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactAliasModel> DeleteContactAlias(long contactAliasID, DateTime modifiedOn)
        {
            return contactAliasService.DeleteContactAlias(contactAliasID, modifiedOn);
        }
    }
}