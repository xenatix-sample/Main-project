using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.Model.Scheduling
{
    public class CalendarModel : BaseEntity
    {
        /// <summary>
        /// Appointment Id
        /// </summary>
        public long AppointmentID { get; set; }

        /// <summary>
        /// Program Id
        /// </summary>
        public long ProgramID { get; set; }

        /// <summary>
        /// Appointment Type Id
        /// </summary>
        public int AppointmentTypeID { get; set; }

        /// <summary>
        /// Services Id
        /// </summary>
        public int? ServicesID { get; set; }

        /// <summary>
        /// Appointment Date
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Appointment Start Time
        /// </summary>
        public short AppointmentStartTime { get; set; }

        /// <summary>
        /// Appointment Length
        /// </summary>
        public short AppointmentLength { get; set; }

        /// <summary>
        /// Supervision Visit
        /// </summary>
        public bool SupervisionVisit { get; set; }

        /// <summary>
        /// Referred by
        /// </summary>
        public string ReferredBy { get; set; }

        /// <summary>
        /// Reason For Visit
        /// </summary>
        public string ReasonForVisit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is cancelled; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the FacilityID identifier.
        /// </summary>
        /// <value>
        /// The FacilityID identifier.
        /// </value>
        public int? FacilityID { get; set; }

        /// <summary>
        /// Gets or sets the appointment contacts
        /// </summary>
        /// <value>
        /// The appointment contacts
        /// </value>
        public List<AppointmentResourceModel> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the appointment resources
        /// </summary>
        /// <value>
        /// The appointment resources.
        /// </value>
        public List<AppointmentResourceModel> Resources { get; set; }

        /// <summary>
        /// Gets or sets the flag which represents a group appointment
        /// </summary>
        public bool? IsGroupAppointment { get; set; }

        public string GroupName { get; set;}
		public string AppointmentType {get; set;}
		public string ServiceName { get; set;}
        public string GroupType { get; set; }
        public string ProgramName { get; set; }
        public string FacilityName { get; set; }
        public string Comments { get; set; }
        public long? GroupID { get; set; }
        public long? GroupDetailID { get; set; }
        public string GroupComments { get; set; }
    }
}
