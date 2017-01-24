using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Model
{
    /// <summary>
    /// Represents Contact's Race
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Model.BaseViewModel" />
    public class ContactRaceViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contact Race identifier.
        /// </summary>
        /// <value>
        /// The contact Race identifier.
        /// </value>
        public long ContactRaceID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the raceid identifier.
        /// </summary>
        /// <value>
        /// The raceid identifier.
        /// </value>
        public int RaceID { get; set; }
    }
}
