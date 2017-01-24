using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.Model.CallCenter
{
    /// <summary>
    /// Law Liaison Incident Model
    /// </summary>
    public class LawLiaisonIncidentModel : BaseEntity
    {
        public LawLiaisonIncidentModel()
        {
            RelatedItems = new List<LawLiaisonIncidentRelatedItemsModel>();
        }

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
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }

        /// <summary>
        /// Gets or sets the call date.
        /// </summary>
        /// <value>
        /// The call date.
        /// </value>
        public DateTime? CallDate { get; set; }

        /// <summary>
        /// Gets or sets the caller identifier.
        /// </summary>
        /// <value>
        /// The caller identifier.
        /// </value>
        public long? CallerID { get; set; }

        /// <summary>
        /// Gets or sets the caller.
        /// </summary>
        /// <value>
        /// The caller.
        /// </value>
        public string Caller { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the client.
        /// </summary>
        /// <value>
        /// The first name of the client.
        /// </value>
        public string ClientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the client.
        /// </summary>
        /// <value>
        /// The last name of the client.
        /// </value>
        public string ClientLastName { get; set; }

        /// <summary>
        /// Gets or sets the call center priority identifier.
        /// </summary>
        /// <value>
        /// The call center priority identifier.
        /// </value>
        public short? CallCenterPriorityID { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public string Priority { get; set; }

        /// <summary>
        /// Gets or sets the is voided.
        /// </summary>
        /// <value>
        /// The is voided.
        /// </value>
        public bool? IsVoided { get; set; }

        /// <summary>
        /// Gets or sets the is creator access.
        /// </summary>
        /// <value>
        /// The is creator access.
        /// </value>
        public bool? IsCreatorAccess { get; set; }

        /// <summary>
        /// Gets or sets the related items.
        /// </summary>
        /// <value>
        /// The related items.
        /// </value>
        public List<LawLiaisonIncidentRelatedItemsModel> RelatedItems { get; set; }
    }
}