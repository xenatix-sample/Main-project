using System;

namespace Axis.Model.Common
{
    public class RecipientCodeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Recipent code identifier.
        /// </summary>
        /// <value>
        /// The recipent code identifier.
        /// </value>
        public short CodeID { get; set; }

        /// <summary>
        /// Gets or sets the Recipent code identifier.
        /// </summary>
        /// <value>
        /// The recipent code identifier.
        /// </value>
        public string CodeDescription { get; set; }

        /// <summary>
        /// Gets or sets the Recipent code identifier.
        /// </summary>
        /// <value>
        /// The recipent code identifier.
        /// </value>
        public long RecipientCodeModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the Recipent code identifier.
        /// </summary>
        /// <value>
        /// The recipent code identifier.
        /// </value>
        public short Code { get; set; }

        /// <summary>
        /// Gets or sets the Recipent code identifier.
        /// </summary>
        /// <value>
        /// The recipent code identifier.
        /// </value>
        public long ModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the Recipent code identifier.
        /// </summary>
        /// <value>
        /// The recipent code identifier.
        /// </value>
        public string DataKey { get; set; }

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        public int? ServiceID { get; set; }
    }
}
