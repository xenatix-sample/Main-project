using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Api Controller for Contact Race
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class ContactRaceController : BaseApiController
    {
        /// <summary>
        /// The contact Race repository
        /// </summary>
        private readonly IContactRaceRepository contactRaceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRaceController" /> class.
        /// </summary>
        /// <param name="contactRaceRepository">The contact Race repository.</param>
        public ContactRaceController(IContactRaceRepository contactRaceRepository)
        {
            this.contactRaceRepository = contactRaceRepository;
        }

        /// <summary>
        /// Gets the contact Race.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactRaceViewModel> GetContactRace(int contactID)
        {
            return contactRaceRepository.GetContactRace(contactID);
        }

        /// <summary>
        /// Adds the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactRaceViewModel> AddContactRace(ContactRaceViewModel contactRace)
        {
            var emails = new List<ContactRaceViewModel> { contactRace };
            return contactRaceRepository.AddContactRace(emails);
        }

        /// <summary>
        /// Updates the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ContactRaceViewModel> UpdateContactRace(ContactRaceViewModel contactRace)
        {
            var emails = new List<ContactRaceViewModel> { contactRace };
            return contactRaceRepository.UpdateContactRace(emails);
        }

        /// <summary>
        /// Deletes the contact Race.
        /// </summary>
        /// <param name="contactRaceID">The contact Race identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactRaceViewModel> DeleteContactRace(long contactRaceID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return contactRaceRepository.DeleteContactRace(contactRaceID, modifiedOn);
        }
    }
}