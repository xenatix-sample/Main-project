using Axis.Model.Common;

namespace Axis.Model.Registration
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ContactRaceModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the contact race identifier.
        /// </summary>
        /// <value>
        /// The contact race identifier.
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
        /// Gets or sets the race identifier.
        /// </summary>
        /// <value>
        /// The race identifier.
        /// </value>
        public int RaceID { get; set; }
    }
}