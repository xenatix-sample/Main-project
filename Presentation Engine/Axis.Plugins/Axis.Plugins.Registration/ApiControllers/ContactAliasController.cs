using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Api Controller for Contact Alias
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class ContactAliasController : BaseApiController
    {
        /// <summary>
        /// The contact alias repository
        /// </summary>
        private readonly IContactAliasRepository contactAliasRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAliasController" /> class.
        /// </summary>
        /// <param name="contactAliasRepository">The contact alias repository.</param>
        public ContactAliasController(IContactAliasRepository contactAliasRepository)
        {
            this.contactAliasRepository = contactAliasRepository;
        }

        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactAliasViewModel> GetContactAlias(int contactID)
        {
            return contactAliasRepository.GetContactAlias(contactID);
        }

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactAliasViewModel> AddContactAlias(ContactAliasViewModel contactAlias)
        {
            var emails = new List<ContactAliasViewModel> { contactAlias };
            return contactAliasRepository.AddContactAlias(emails);
        }

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ContactAliasViewModel> UpdateContactAlias(ContactAliasViewModel contactAlias)
        {
            var emails = new List<ContactAliasViewModel> { contactAlias };
            return contactAliasRepository.UpdateContactAlias(emails);
        }

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact alias identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactAliasViewModel> DeleteContactAlias(long contactAliasID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return contactAliasRepository.DeleteContactAlias(contactAliasID, modifiedOn);
        }
    }
}