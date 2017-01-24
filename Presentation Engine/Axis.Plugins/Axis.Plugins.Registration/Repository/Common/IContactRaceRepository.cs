using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IContactRaceRepository
    {
        /// <summary>
        /// Gets the contact Race.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactRaceViewModel> GetContactRace(long contactID);

        /// <summary>
        /// Adds the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        Response<ContactRaceViewModel> AddContactRace(List<ContactRaceViewModel> contactRace);

        /// <summary>
        /// Updates the contact Race.
        /// </summary>
        /// <param name="contactRace">The contact Race.</param>
        /// <returns></returns>
        Response<ContactRaceViewModel> UpdateContactRace(List<ContactRaceViewModel> contactRace);

        /// <summary>
        /// Deletes the contact Race.
        /// </summary>
        /// <param name="contactRaceID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<ContactRaceViewModel> DeleteContactRace(long contactRaceID, DateTime modifiedOn);
    }
}