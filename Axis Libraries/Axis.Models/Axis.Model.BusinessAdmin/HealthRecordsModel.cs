using Axis.Model.Address;
using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.Model.BusinessAdmin
{
    public class HealthRecordsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the First Name.
        /// <value>
        /// The First Name.
        /// </value>
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Middle Name.
        /// </summary>
        /// <value>
        /// The Middle Name.
        /// </value>
        public string Middle { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        /// <value>
        /// The Last Name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }

        /// <summary>
        /// Gets or sets the Printed By.
        /// </summary>
        /// <value>
        /// The Printed By.
        /// </value>
        public int? PrintedBy { get; set; }

        /// <summary>
        /// Gets or sets the Printed Date.
        /// </summary>
        /// <value>
        /// The Printed Date.
        /// </value>
        public DateTime? PrintedDate { get; set; }

        #region Requestor

        /// <summary>
        /// Gets or sets the Requested By.
        /// </summary>
        /// <value>
        /// The Requested By.
        /// </value>
        public string RequestedBy { get; set; }

        /// <summary>
        /// Gets or sets the Addresses.
        /// </summary>
        /// <value>
        /// The Addresses.
        /// </value>
        public AddressModel Addresses { get; set; }
        #endregion

        #region Request Details

        /// <summary>
        /// Gets or sets the Send Via ID.
        /// </summary>
        /// <value>
        /// The Send Via ID.
        /// </value>
        public int? SendViaID { get; set; }

        /// <summary>
        /// Gets or sets the Send Date.
        /// </summary>
        /// <value>
        /// The Send Date.
        /// </value>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// Gets or sets the Reason For Request.
        /// </summary>
        /// <value>
        /// The Reason For Request.
        /// </value>
        public int? ReasonForRequest { get; set; }

        /// <summary>
        /// Gets or sets the Authorized By.
        /// </summary>
        /// <value>
        /// The Authorized By.
        /// </value>
        public int? AuthorizedBy { get; set; }

        /// <summary>
        /// Gets or sets the From Date.
        /// </summary>
        /// <value>
        /// The From Date.
        /// </value>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Gets or sets the To Date.
        /// </summary>
        /// <value>
        /// The To Date.
        /// </value>
        public DateTime? ToDate { get; set; }
        #endregion

        #region Select Records

        /// <summary>
        /// Gets or sets the Is All Records Needed.
        /// </summary>
        /// <value>
        /// The Is All Records Needed.
        /// </value>
        public bool IsAllRecordsNeeded { get; set; }

        /// <summary>
        /// Gets or sets the Reports To Print.
        /// </summary>
        /// <value>
        /// The Reports To Print.
        /// </value>
        public long? ReportsToPrint { get; set; }
        #endregion

        #region Additional Information

        /// <summary>
        /// Gets or sets the Comments.
        /// </summary>
        /// <value>
        /// The Comments.
        /// </value>
        public string Comments { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the Status ID.
        /// </summary>
        /// <value>
        /// The Status ID.
        /// </value>
        public int? StatusID { get; set; }
    }
}
