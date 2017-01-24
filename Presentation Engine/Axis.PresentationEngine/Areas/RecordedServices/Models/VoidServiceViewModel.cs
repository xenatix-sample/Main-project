using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.PresentationEngine.Areas.RecordedServices.Models
{
    public class VoidServiceViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the ServiceRecordingVoidID.
        /// </summary>
        /// <value>
        /// The ServiceRecordingVoidID.
        /// </value>
        public long? ServiceRecordingVoidID { get; set; }

        /// <summary>
        /// Gets or sets the ServiceRecordingID.
        /// </summary>
        /// <value>
        /// The ServiceRecordingID.
        /// </value>
        public long ServiceRecordingID { get; set; }

        /// <summary>
        /// Gets or sets the ServiceRecordingVoidReasonID.
        /// </summary>
        /// <value>
        /// The ServiceRecordingVoidReasonID.
        /// </value>
        public short ServiceRecordingVoidReasonID { get; set; }

        /// <summary>
        /// Gets or sets the IsCreateCopyToEdit.
        /// </summary>
        /// <value>
        /// The IsCreateCopyToEdit.
        /// </value>
        public bool IsCreateCopyToEdit { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectOrganization.
        /// </summary>
        /// <value>
        /// The IncorrectOrganization.
        /// </value>
        public bool IncorrectOrganization { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectServiceType.
        /// </summary>
        /// <value>
        /// The IncorrectServiceType.
        /// </value>
        public bool IncorrectServiceType { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectServiceItem.
        /// </summary>
        /// <value>
        /// The IncorrectServiceItem.
        /// </value>
        public bool IncorrectServiceItem { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectServiceStatus.
        /// </summary>
        /// <value>
        /// The IncorrectServiceStatus.
        /// </value>
        public bool IncorrectServiceStatus { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectSupervisor.
        /// </summary>
        /// <value>
        /// The IncorrectSupervisor.
        /// </value>
        public bool IncorrectSupervisor { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectAdditionalUser.
        /// </summary>
        /// <value>
        /// The IncorrectAdditionalUser.
        /// </value>
        public bool IncorrectAdditionalUser { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectAttendanceStatus.
        /// </summary>
        /// <value>
        /// The IncorrectAttendanceStatus.
        /// </value>
        public bool IncorrectAttendanceStatus { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectStartDate.
        /// </summary>
        /// <value>
        /// The IncorrectStartDate.
        /// </value>
        public bool IncorrectStartDate { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectStartTime.
        /// </summary>
        /// <value>
        /// The IncorrectStartTime.
        /// </value>
        public bool IncorrectStartTime { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectEndDate.
        /// </summary>
        /// <value>
        /// The IncorrectEndDate.
        /// </value>
        public bool? IncorrectEndDate { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectEndTime.
        /// </summary>
        /// <value>
        /// The IncorrectEndTime.
        /// </value>
        public bool IncorrectEndTime { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectDeliveryMethod.
        /// </summary>
        /// <value>
        /// The IncorrectDeliveryMethod.
        /// </value>
        public bool IncorrectDeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectServiceLocation.
        /// </summary>
        /// <value>
        /// The IncorrectServiceLocation.
        /// </value>
        public bool IncorrectServiceLocation { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectRecipientCode.
        /// </summary>
        /// <value>
        /// The IncorrectRecipientCode.
        /// </value>
        public bool IncorrectRecipientCode { get; set; }

        /// <summary>
        /// Gets or sets the IncorrectTrackingField.
        /// </summary>
        /// <value>
        /// The IncorrectTrackingField.
        /// </value>
        public bool IncorrectTrackingField { get; set; }

        /// <summary>
        /// Gets or sets the Comments.
        /// </summary>
        /// <value>
        /// The Comments.
        /// </value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the NoteHeaderID.
        /// </summary>
        /// <value>
        /// The NoteHeaderID.
        /// </value>
        public long? NoteHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }
    }
}