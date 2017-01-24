using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// Rule engine class for calling service method of contact Race Service
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Registration.IContactRaceRuleEngine" />
    public class ContactRaceRuleEngine : IContactRaceRuleEngine
    {
        /// <summary>
        /// The contact Race service
        /// </summary>
        private readonly IContactRaceService contactRaceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRaceRuleEngine"/> class.
        /// </summary>
        /// <param name="contactRaceService">The contact Race service.</param>
        public ContactRaceRuleEngine(IContactRaceService contactRaceService)
        {
            this.contactRaceService = contactRaceService;
        }

        /// <summary>
        /// Gets the contact Race.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactRaceModel> GetContactRace(long contactID)
        {
            return contactRaceService.GetContactRace(contactID);
        }

        /// <summary>
        /// Adds the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        public Response<ContactRaceModel> AddContactRace(List<ContactRaceModel> contactRace)
        {
            return contactRaceService.AddContactRace(contactRace);
        }

        /// <summary>
        /// Updates the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        public Response<ContactRaceModel> UpdateContactRace(List<ContactRaceModel> contactRace)
        {
            return contactRaceService.UpdateContactRace(contactRace);
        }

        /// <summary>
        /// Deletes the contact Race.
        /// </summary>
        /// <param name="contactRaceID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactRaceModel> DeleteContactRace(long contactRaceID, DateTime modifiedOn)
        {
            return contactRaceService.DeleteContactRace(contactRaceID, modifiedOn);
        }
    }
}