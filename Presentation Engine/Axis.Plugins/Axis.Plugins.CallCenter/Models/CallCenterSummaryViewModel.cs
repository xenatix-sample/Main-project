using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.CallCenter.Models
{
    public class CallCenterSummaryViewModel
    {
        /// <summary>
        /// Gets or sets the call center identifier.
        /// </summary>
        /// <value>
        /// The call center identifier.
        /// </value>
        public long? CallCenterID { get; set; }

        /// <summary>
        /// Gets or sets the call center type identifier.
        /// </summary>
        /// <value>
        /// The call center type identifier.
        /// </value>
        public Int16 CallCenterTypeID { get; set; }

        /// <summary>
        /// Gets or sets the call center header identifier.
        /// </summary>
        /// <value>
        /// The call center header identifier.
        /// </value>
        public long? CallCenterHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the MRN identifier.
        /// </summary>
        /// <value>
        /// The MRN identifier.
        /// </value>
        public long? MRN { get; set; }

        /// <summary>
        /// Gets or sets the incident identifier.
        /// </summary>
        /// <value>
        /// The incident identifier.
        /// </value>
        public long? IncidentID { get; set; }

        /// <summary>
        /// Gets or sets the call date.
        /// </summary>
        /// <value>
        /// The call date.
        /// </value>
        public DateTime? CallDate { get; set; }

        /// <summary>
        /// Gets or sets the caller.
        /// </summary>
        /// <value>
        /// The caller.
        /// </value>
        public string Caller { get; set; }

        /// <summary>
        /// Gets or sets the caller contact number.
        /// </summary>
        /// <value>
        /// The caller contact number.
        /// </value>
        public string CallerContactNumber { get; set; }

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
        /// Gets or sets the dateof incident.
        /// </summary>
        /// <value>
        /// The dateof incident.
        /// </value>
        public DateTime? DateofIncident { get; set; }

        /// <summary>
        /// Gets or sets the call status.
        /// </summary>
        /// <value>
        /// The call status.
        /// </value>
        public Int16? CallStatusID { get; set; }

        /// <summary>
        /// Gets or sets the call status.
        /// </summary>
        /// <value>
        /// The call status.
        /// </value>
        public string CallStatus { get; set; }

        /// <summary>
        /// Gets or sets the provider submitted by.
        /// </summary>
        /// <value>
        /// The provider submitted by.
        /// </value>
        public string ProviderSubmittedBy { get; set; }

        /// <summary>
        /// Gets or sets the referring agency identifier.
        /// </summary>
        /// <value>
        /// The referring agency identifier.
        /// </value>
        public int? ReferringAgencyID { get; set; }

        /// <summary>
        /// Gets or sets the referring agency.
        /// </summary>
        /// <value>
        /// The referring agency.
        /// </value>
        public string ReferringAgency { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long? ContactID { get; set; }

        /// <summary>
        /// Gets or sets the client type identifier.
        /// </summary>
        /// <value>
        /// The client type identifier.
        /// </value>
        public int? ClientTypeID { get; set; }

        /// <summary>
        /// Gets or sets the program unit identifier.
        /// </summary>
        /// <value>
        /// The program unit identifier.
        /// </value>
        public long? ProgramUnitID { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; set; } //Addedfor contact search

        /// <summary>
        /// Gets or sets the Last SignedOn Date.
        /// </summary>
        /// <value>
        /// The SignedOn.
        /// </value>
        public DateTime? SignedOn { get; set; }

        /// <summary>
        /// Gets or sets the ServiceRecordingID.
        /// </summary>
        /// <value>
        /// The ServiceRecordingID.
        /// </value>
        public long? ServiceRecordingID { get; set; }

        /// <summary>
        /// Gets or sets the IsSignedByUser.
        /// </summary>
        /// <value>
        /// The IsSignedByUser.
        /// </value>
        public bool? IsSignedByUser { get; set; }

        /// <summary>
        /// Gets or sets the IsVoided.
        /// </summary>
        /// <value>
        /// The IsVoided.
        /// </value>
        public bool? IsVoided { get; set; }

        /// <summary>
        /// Gets or sets the contact program unit.
        /// </summary>
        /// <value>
        /// The contact program unit.
        /// </value>
        public string ContactProgramUnit { get; set; }

        /// <summary>
        /// Gets or sets the NoteHeaderID.
        /// </summary>
        /// <value>
        /// The NoteHeaderID.
        /// </value>
        public long? NoteHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the county identifier.
        /// </summary>
        /// <value>
        /// The county identifier.
        /// </value>
        public int? CountyID { get; set; }

        /// <summary>
        /// Gets or sets the service item identifier.
        /// </summary>
        /// <value>
        /// The service item identifier.
        /// </value>
        public int? ServiceItemID { get; set; }

        /// <summary>
        /// Gets or sets the service status identifier.
        /// </summary>
        /// <value>
        /// The service status identifier.
        /// </value>
        public short? ServiceStatusID { get; set; }

        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>
        /// The service type identifier.
        /// </value>
        public short? ServiceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the tracking field.
        /// </summary>
        /// <value>
        /// The tracking field.
        /// </value>
        public string TrackingField { get; set; }

        /// <summary>
        /// Gets or sets the recipient code identifier.
        /// </summary>
        /// <value>
        /// The recipient code identifier.
        /// </value>
        public short? RecipientCodeID { get; set; }

        /// <summary>
        /// Gets or sets the attendance status identifier.
        /// </summary>
        /// <value>
        /// The attendance status identifier.
        /// </value>
        public short? AttendanceStatusID { get; set; }

        /// <summary>
        /// Gets or sets the suicide homicide identifier.
        /// </summary>
        /// <value>
        /// The suicide homicide identifier.
        /// </value>
        public short? SuicideHomicideID { get; set; }

        /// <summary>
        /// Gets or sets the call center priority identifier.
        /// </summary>
        /// <value>
        /// The call center priority identifier.
        /// </value>
        public short? CallCenterPriorityID { get; set; }

        /// <summary>
        /// Gets or sets the reason called.
        /// </summary>
        /// <value>
        /// The reason called.
        /// </value>
        public string ReasonCalled { get; set; }

        /// <summary>
        /// Gets or sets the disposition.
        /// </summary>
        /// <value>
        /// The disposition.
        /// </value>
        public string Disposition { get; set; }

        /// <summary>
        /// Gets or sets the other information.
        /// </summary>
        /// <value>
        /// The other information.
        /// </value>
        public string OtherInformation { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the begin date.
        /// </summary>
        /// <value>
        /// The begin date.
        /// </value>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [follow up required].
        /// </summary>
        /// <value><c>null</c> if [follow up required] contains no value, <c>true</c> if [follow up required]; otherwise, <c>false</c>.</value>
        public bool? FollowUpRequired { get; set; }
        /// <summary>
        /// Gets or sets the parent call center header identifier.
        /// </summary>
        /// <value>
        /// The parent call center header identifier.
        /// </value>
        public long? ParentCallCenterHeaderID { get; set; }

        /// <summary>
        /// Gets or sets HasChild.
        /// </summary>
        /// <value>
        /// The HasChild.
        /// </value>
        public int HasChild { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the is creator access.
        /// </summary>
        /// <value>
        /// The is creator access.
        /// </value>
        public bool? IsCreatorAccess { get; set; }

        /// <summary>
        /// Gets or sets the is manager access.
        /// </summary>
        /// <value>
        /// The is manager access.
        /// </value>
        public bool? IsManagerAccess { get; set; }
        /// <summary>
        /// Gets or sets the Nature of Call.
        /// </summary>
        /// <value>
        /// The Nature of Call.
        /// </value>
        public string NatureofCall { get; set; }

        /// <summary>
        /// Gets or sets the service end date.
        /// </summary>
        /// <value>
        /// The service end date.
        /// </value>
        public DateTime? ServiceEndDate { get; set; }
        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        /// <value>
        /// The Duration.
        /// </value>
        public string Duration { get; set; }
    }
}
