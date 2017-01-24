using System;

namespace Axis.Model.Common
{
    public class DeliveryMethodModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Delivery Method identifier.
        /// </summary>
        /// <value>
        /// The Delivery Method identifier.
        /// </value>
        public short DeliveryMethodID { get; set; }

        /// <summary>
        /// Gets or sets the Delivery Method identifier.
        /// </summary>
        /// <value>
        /// The Delivery Method identifier.
        /// </value>
        public string DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the Delivery Method identifier.
        /// </summary>
        /// <value>
        /// The Delivery Method identifier.
        /// </value>
        public long DeliveryMethodModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the Delivery Method identifier.
        /// </summary>
        /// <value>
        /// The Delivery Method identifier.
        /// </value>
        public long ModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the Delivery Method identifier.
        /// </summary>
        /// <value>
        /// The Delivery Method identifier.
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
