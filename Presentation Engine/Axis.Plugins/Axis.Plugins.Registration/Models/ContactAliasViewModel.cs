using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Model
{
    /// <summary>
    /// Represents Contact's Alias
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Model.BaseViewModel" />
    public class ContactAliasViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contact alias identifier.
        /// </summary>
        /// <value>
        /// The contact alias identifier.
        /// </value>
        public long ContactAliasID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the alias.
        /// </summary>
        /// <value>
        /// The first name of the alias.
        /// </value>
        public string AliasFirstName { get; set; }

        /// <summary>
        /// Gets or sets the alias middle.
        /// </summary>
        /// <value>
        /// The alias middle.
        /// </value>
        public string AliasMiddle { get; set; }

        /// <summary>
        /// Gets or sets the last name of the alias.
        /// </summary>
        /// <value>
        /// The last name of the alias.
        /// </value>
        public string AliasLastName { get; set; }

        /// <summary>
        /// Gets or sets the suffix identifier.
        /// </summary>
        /// <value>
        /// The suffix identifier.
        /// </value>
        public int? SuffixID { get; set; }
    }
}
