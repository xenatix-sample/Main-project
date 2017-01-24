using System;

namespace Axis.Model.Common
{
    public class OrganizationsModel
    {
        /// <summary>
        /// Gets or sets the detail identifier.
        /// </summary>
        /// <value>
        /// The detail identifier.
        /// </value>
        public long? DetailID { get; set; }

        /// <summary>
        /// Identity ID
        /// </summary>
        public long MappingID { get; set; }

        /// <summary>
        /// Get Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get ParentId
        /// </summary>
        public long? ParentID { get; set; }

        /// <summary>
        /// Get Datakey
        /// </summary>
        public string DataKey { get; set; }

    }
}
