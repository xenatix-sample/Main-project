using Axis.Model.Common;
using System;

namespace Axis.Model.CallCenter
{
    /// <summary>
    /// Law Liaison Incident Related Items
    /// </summary>

    public class LawLiaisonIncidentRelatedItemsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the call center header identifier.
        /// </summary>
        /// <value>
        /// The call center header identifier.
        /// </value>
        public long CallCenterHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the parent call center header identifier.
        /// </summary>
        /// <value>
        /// The parent call center header identifier.
        /// </value>
        public long? ParentCallCenterHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the service item identifier.
        /// </summary>
        /// <value>
        /// The service item identifier.
        /// </value>
        public int? ServiceItemID { get; set; }

        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>
        /// The service type identifier.
        /// </value>
        public short? ServiceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public string ITEM { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string TYPE { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime? DATE { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the is voided.
        /// </summary>
        /// <value>
        /// isVoided.
        /// </value>
        public bool? IsVoided { get; set; }

        /// <summary>
        /// Gets or sets the is creator access.
        /// </summary>
        /// <value>
        /// The is creator access.
        /// </value>
        public bool? IsCreatorAccess { get; set; }
    }
}