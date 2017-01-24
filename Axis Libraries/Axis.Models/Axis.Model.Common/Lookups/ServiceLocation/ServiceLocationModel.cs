using System;
namespace Axis.Model.Common
{
    public class ServiceLocationModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the service location identifier.
        /// </summary>
        /// <value>
        /// The service location identifier.
        /// </value>
        public Int16 ServiceLocationID { get; set; }

        /// <summary>
        /// Gets or sets the service location identifier.
        /// </summary>
        /// <value>
        /// The service location identifier.
        /// </value>
        public string ServiceLocation { get; set; }


        /// <summary>
        /// Gets or sets the service location identifier.
        /// </summary>
        /// <value>
        /// The service location identifier.
        /// </value>
        public long ServiceLocationModuleComponentID { get; set; }


        /// <summary>
        /// Gets or sets the service location identifier.
        /// </summary>
        /// <value>
        /// The service location identifier.
        /// </value>
        public long ModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the service location identifier.
        /// </summary>
        /// <value>
        /// The service location identifier.
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
