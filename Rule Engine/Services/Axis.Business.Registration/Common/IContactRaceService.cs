using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Interface of Contact Race Service class to call the web api methods
    /// </summary>
    public interface IContactRaceService
    {
        /// <summary>
        /// Gets the contact Race.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactRaceModel> GetContactRace(long contactID);

        /// <summary>
        /// Adds the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        Response<ContactRaceModel> AddContactRace(List<ContactRaceModel> contactRace);

        /// <summary>
        /// Updates the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        Response<ContactRaceModel> UpdateContactRace(List<ContactRaceModel> contactRace);

        /// <summary>
        /// Deletes the contact Race.
        /// </summary>
        /// <param name="contactRaceID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<ContactRaceModel> DeleteContactRace(long contactRaceID, DateTime modifiedOn);
    }
}