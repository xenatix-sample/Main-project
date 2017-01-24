using System;

namespace Axis.Model.Common
{
    public class ClientStatusModel
    {
        /// <summary>
        /// Gets or sets the client status identifier.
        /// </summary>
        /// <value>
        /// The client status identifier.
        /// </value>
        public Int16 ClientStatusID { get; set; }

        /// <summary>
        /// Gets or sets the client status.
        /// </summary>
        /// <value>
        /// The client status.
        /// </value>
        public string ClientStatus { get; set; }
    }
}
