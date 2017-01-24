using System;

namespace Axis.Model.BusinessAdmin
{
    public class PotentialMergeContactsLastRunModel
    {
        /// <summary>
        /// User Id of Potentential Match last run
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserID { get; set; }

        /// <summary>
        /// Last run by of Potentential Match last run
        /// </summary>
        /// <value>
        /// The last run by.
        /// </value>
        public string LastRunBy { get; set; }

        /// <summary>
        /// last run date of Potentential Match last run
        /// </summary>
        /// <value>
        /// The last run date.
        /// </value>
        public DateTime LastRunDate { get; set; }
    }
}
