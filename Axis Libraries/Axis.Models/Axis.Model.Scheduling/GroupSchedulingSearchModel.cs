using Axis.Model.Common;
using System;

namespace Axis.Model.Scheduling
{
    public class GroupSchedulingSearchModel : BaseEntity
    {
        /// <summary>
        /// Group Id
        /// </summary>
        public long GroupID { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        public long GroupDetailID { get; set; }

        /// <summary>
        /// Group Name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// GroupType Id
        /// </summary>
        public int GroupTypeID { get; set; }

        /// <summary>
        /// Program Id
        /// </summary>
        public long? ProgramID { get; set; }

        /// <summary>
        /// Program Name
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// Gets or sets the FacilityID identifier.
        /// </summary>
        /// <value>
        /// The FacilityID identifier.
        /// </value>
        public int? FacilityID { get; set; }

        /// <summary>
        /// Gets or sets the Facility Name.
        /// </summary>
        /// <value>
        /// The Facility Name.
        /// </value>
        public string FacilityName { get; set; }

        /// <summary>
        /// Gets or Sets the AppointmentID
        /// </summary>
        public long? AppointmentID { get; set; }

        /// <summary>
        /// Gets or Sets the AppointmentStatusId
        /// </summary>
        public int AppointmentStatusId { get; set; }

        /// <summary>
        /// Appointment Date
        /// </summary>
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// Appointment Start Time
        /// </summary>
        public short? AppointmentStartTime { get; set; }

        /// <summary>
        /// Gets or sets the appointment reoccuring.
        /// </summary>
        public int? Recurring { get; set; }

        /// <summary>
        /// Gets or Sets the CancelReasonID
        /// </summary>
        public int CancelReasonID { get; set; }

        /// <summary>
        /// Gets or Sets the IsCancelAllAppoitment
        /// </summary>
        public bool IsCancelAllAppoitment { get; set; }

        /// <summary>
        /// Gets or Sets the CancelComment
        /// </summary>
        public string CancelComment { get; set; }
    }
}
