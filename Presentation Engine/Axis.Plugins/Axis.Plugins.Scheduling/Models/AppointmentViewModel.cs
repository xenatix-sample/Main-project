using Axis.PresentationEngine.Helpers.Model;
using Axis.PresentationEngine.Models;
using System;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentViewModel : BaseViewModel
    {
        /// <summary>
        /// Appointment Id
        /// </summary>
        /// <value>
        /// The appointment identifier.
        /// </value>
        public long AppointmentID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public int? ContactID { get; set; }

        /// <summary>
        /// Program Id
        /// </summary>
        /// <value>
        /// The program identifier.
        /// </value>
        public long ProgramID { get; set; }

        /// <summary>
        /// Appointment Type Id
        /// </summary>
        /// <value>
        /// The appointment type identifier.
        /// </value>
        public int AppointmentTypeID { get; set; }

        /// <summary>
        /// Services Id
        /// </summary>
        /// <value>
        /// The services identifier.
        /// </value>
        public int? ServicesID { get; set; }

        /// <summary>
        /// Gets or sets the ServiceStatusID
        /// </summary>
        /// <value>
        /// The service status identifier.
        /// </value>
        public Int16? ServiceStatusID { get; set; }

        /// <summary>
        /// Appointment Date
        /// </summary>
        /// <value>
        /// The appointment date.
        /// </value>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Appointment Start Time
        /// </summary>
        /// <value>
        /// The appointment start time.
        /// </value>
        public short AppointmentStartTime { get; set; }

        /// <summary>
        /// Appointment Length
        /// </summary>
        /// <value>
        /// The length of the appointment.
        /// </value>
        public short AppointmentLength { get; set; }

        /// <summary>
        /// Supervision Visit
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supervision visit]; otherwise, <c>false</c>.
        /// </value>
        public bool SupervisionVisit { get; set; }

        /// <summary>
        /// Referred by
        /// </summary>
        /// <value>
        /// The referred by.
        /// </value>
        public string ReferredBy { get; set; }

        /// <summary>
        /// Reason For Visit
        /// </summary>
        /// <value>
        /// The reason for visit.
        /// </value>
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

        public bool IsCancelAllAppoitment { get; set; }

        /// <summary>
        /// Gets or sets the CancelReasonID identifier.
        /// </summary>
        /// <value>
        /// The CancelReasonID identifier.
        /// </value>
        public int? CancelReasonID { get; set; }

        /// <summary>
        /// Gets or sets the CancelComment
        /// </summary>
        /// <value>
        /// The CancelComment
        /// </value>
        public string CancelComment { get; set; }

        /// <summary>
        /// Gets or sets the Comments
        /// </summary>
        /// <value>
        /// The CancelComment
        /// </value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the IsInterpreterRequired flag
        /// </summary>
        /// <value>
        /// The is interpreter required.
        /// </value>
        public bool? IsInterpreterRequired { get; set; }

        /// <summary>
        /// Gets or sets the non MHMR appointment.
        /// </summary>
        /// <value>
        /// The non MHMR appointment.
        /// </value>
        public string NonMHMRAppointment { get; set; }

        /// <summary>
        /// Gets or sets the recurrence.
        /// </summary>
        /// <value>
        /// The recurrence.
        /// </value>
        public RecurrenceViewModel Recurrence { get; set; }

        /// <summary>
        /// Gets or sets the recurrence identifier.
        /// </summary>
        /// <value>
        /// The recurrence identifier.
        /// </value>
        public long? RecurrenceID { get; set; }

        /// <summary>
        /// Gets or sets the flag which represents a group appointment
        /// </summary>
        public bool? IsGroupAppointment { get; set; }

        /// <summary>
        /// Gets or sets the Appointment Status identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public int? AppointmentStatusID { get; set; }

        public string GroupName { get; set; }
        public string AppointmentType { get; set; }
        public string ServiceName { get; set; }
        public string GroupType { get; set; }
        public string ProgramName { get; set; }
        public string FacilityName { get; set; }
        public long? GroupID { get; set; }
        public long? GroupDetailID { get; set; }
        public string GroupComments { get; set; }

        public DateTime? RecurrenceEndDate { get; set; }
        public string RecurrenceDay { get; set; }
        public string RecurrenceFrequency { get; set; }
        public int? NumberOfOccurences { get; set; }
        public int? RecurrID { get; set; }
        public bool IsRecurringAptEdit { get; set; }
    }
}
