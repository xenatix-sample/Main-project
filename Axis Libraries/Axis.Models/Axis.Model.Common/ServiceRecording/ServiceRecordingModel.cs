using Axis.Model.Common;
using Axis.Model.Common.ServiceRecording;
using System;
using System.Collections.Generic;

namespace Axis.Model.CallCenter
{
    public class ServiceRecordingModel : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingModel"/> class.
        /// </summary>
        public ServiceRecordingModel()
        {
            AttendedList = new List<ServiceRecordingAttendeeModel>();
            AdditionalUserList = new List<ServiceRecordingAdditionalUserModel>();
        }

        /// <summary>
        /// Gets or sets the Service Recording identifier.
        /// </summary>
        /// <value>
        /// The Service Recording identifier.
        /// </value>
        public long ServiceRecordingID { get; set; }

        /// <summary>
        /// Gets or sets the Service Recording parent identifier.
        /// </summary>
        /// <value>
        /// The Service Recording parent identifier.
        /// </value>
        public long? ParentServiceRecordingID { get; set; }

        /// <summary>
        /// Gets or sets the Service Recording void identifier.
        /// </summary>
        /// <value>
        /// The Service Recording void identifier.
        /// </value>
        public long? ServiceRecordingVoidID { get; set; }

        /// <summary>
        /// Gets or sets the Service Recording header identifier.
        /// </summary>
        /// <value>
        /// The Service Recording identifier.
        /// </value>
        public long? ServiceRecordingHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the Service Recording Source identifier.
        /// </summary>
        /// <value>
        /// The Service Recording Source identifier.
        /// </value>
        public int? ServiceRecordingSourceID { get; set; }

        /// <summary>
        /// Gets or sets the Source header identifier.
        /// </summary>
        /// <value>
        /// The source header identifier.
        /// </value>
        public long? CallCenterHeaderID { get; set; }


        /// <summary>
        /// Gets or sets the Source header identifier.
        /// </summary>
        /// <value>
        /// The source header identifier.
        /// </value>
        public long? SourceHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the Service Item identifier.
        /// </summary>
        /// <value>
        /// The Service Item identifier.
        /// </value>
        public int? ServiceItemID { get; set; }

        /// <summary>
        /// Gets or sets the Attendance Status identifier.
        /// </summary>
        /// <value>
        /// TheAttendance Status identifier.
        /// </value>
        public short? AttendanceStatusID { get; set; }

        /// <summary>
        /// Gets or sets DeliveryMethod identifier.
        /// </summary>
        /// <value>
        /// The DeliveryMethod identifier.
        /// </value>
        public short? DeliveryMethodID { get; set; }

        /// <summary>
        /// Gets or sets ServiceStatus identifier.
        /// </summary>
        /// <value>
        /// The ServiceStatus identifier.
        /// </value>
        public short? ServiceStatusID { get; set; }

        /// <summary>
        /// Gets or sets ServiceLocation identifier.
        /// </summary>
        /// <value>
        /// The ServiceLocation identifier.
        /// </value>
        public short? ServiceLocationID { get; set; }

        /// <summary>
        /// Gets or sets RecipientCode identifier.
        /// </summary>
        /// <value>
        /// The RecipientCode identifier.
        /// </value>
        public short? RecipientCodeID { get; set; }

        /// <summary>
        /// Gets or sets RecipientCode identifier.
        /// </summary>
        /// <value>
        /// The RecipientCode identifier.
        /// </value>
        public short? RecipientCode { get; set; }

        /// <summary>
        /// Gets or sets Number Of Recipients.
        /// </summary>
        /// <value>
        /// The Number Of Recipients.
        /// </value>
        public short? NumberOfRecipients { get; set; }

        /// <summary>
        /// Gets or sets ConversionStatus identifier.
        /// </summary>
        /// <value>
        /// The ConversionStatus identifier.
        /// </value>
        public short? ConversionStatusID { get; set; }

        /// <summary>
        /// Gets or sets Conversion DateTime.
        /// </summary>
        /// <value>
        /// The Conversion DateTime.
        /// </value>
        public DateTime? ConversionDateTime { get; set; }

        /// <summary>
        /// Gets or sets End DateTime.
        /// </summary>
        /// <value>
        /// The End DateTime.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the User identifier.
        /// </summary>
        /// <value>
        /// The User identifier.
        /// </value>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public long? OrganizationID { get; set; }

        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>
        /// The service type identifier.
        /// </value>
        public short? ServiceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the service start date.
        /// </summary>
        /// <value>
        /// The service start date.
        /// </value>
        public DateTime? ServiceStartDate { get; set; }

        /// <summary>
        /// Gets or sets the service start time.
        /// </summary>
        /// <value>
        /// The service start time.
        /// </value>
        public TimeSpan? ServiceStartTime { get; set; }

        /// <summary>
        /// Gets or sets the service end date.
        /// </summary>
        /// <value>
        /// The service end date.
        /// </value>
        public DateTime? ServiceEndDate { get; set; }

        /// <summary>
        /// Gets or sets the service end time.
        /// </summary>
        /// <value>
        /// The service end time.
        /// </value>
        public TimeSpan? ServiceEndTime { get; set; }

        /// <summary>
        /// Gets or sets the supervisor user identifier.
        /// </summary>
        /// <value>
        /// The supervisor user identifier.
        /// </value>
        public int? SupervisorUserID { get; set; }

        /// <summary>
        /// Gets or sets the tracking field identifier.
        /// </summary>
        /// <value>
        /// The tracking field identifier.
        /// </value>
        public int? TrackingFieldID { get; set; }

        /// <summary>
        /// Gets or sets the attended list.
        /// </summary>
        /// <value>
        /// The attended list.
        /// </value>
        public List<ServiceRecordingAttendeeModel> AttendedList { get; set; }

        /// <summary>
        /// Gets or sets the additional user list.
        /// </summary>
        /// <value>
        /// The additional user list.
        /// </value>
        public List<ServiceRecordingAdditionalUserModel> AdditionalUserList { get; set; }

        /// <summary>
        /// Gets or sets the document status identifier.
        /// </summary>
        /// <value>
        /// The document status identifier.
        /// </value>
        public short? DocumentStatusID { get; set; }

        /// <summary>
        /// Gets or sets the call status.
        /// </summary>
        /// <value>
        /// The call status.
        /// </value>
        public Int16? CallStatusID { get; set; }

        /// <summary>
        /// Gets or sets the is voided.
        /// </summary>
        /// <value>
        /// The is voided.
        /// </value>
        public bool? IsVoided { get; set; }

        /// <summary>
        /// Gets or sets the signed on.
        /// </summary>
        /// <value>
        /// The signed on.
        /// </value>
        public DateTime? SignedOn { get; set; }

        /// <summary>
        /// Gets the CMHCSent on Date.
        /// </summary>
        /// <value>
        /// The signed on.
        /// </value>
        public DateTime? SentToCMHCDate { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the service duration identifier.
        /// </summary>
        /// <value>
        /// The service duration identifier.
        /// </value>
        public int? ServiceDurationID { get; set; }

    }
}
