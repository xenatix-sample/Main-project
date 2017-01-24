using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.CallCenter.Models
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Model.BaseViewModel" />
    public class CallerInformationViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the call center header identifier.
        /// </summary>
        /// <value>
        /// The call center header identifier.
        /// </value>
        public long CallCenterHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the referral agency identifier.
        /// </summary>
        /// <value>
        /// The referral agency identifier.
        /// </value>
        public Int32? ReferralAgencyID { get; set; }

        /// <summary>
        /// Gets or sets the other referral agency.
        /// </summary>
        /// <value>
        /// The other referral agency.
        /// </value>
        public string OtherReferralAgency { get; set; }

        /// <summary>
        /// Gets or sets the provider id.
        /// </summary>
        /// <value>
        /// The provider identifier.
        /// </value>
        public long? ProviderID { get; set; }

        /// <summary>
        /// Gets or sets the Contact Type ID.
        /// </summary>
        /// <value>
        /// The Contact Type ID.
        /// </value>
        public int? ContactTypeID { get; set; }


        /// <summary>
        /// Gets or sets the caller contact identifier.
        /// </summary>
        /// <value>
        /// The caller contact identifier.
        /// </value>
        public long? CallerContactID { get; set; }

        /// <summary>
        /// Gets or sets the client contact identifier.
        /// </summary>
        /// <value>
        /// The client contact identifier.
        /// </value>
        public long? ClientContactID { get; set; }

        /// <summary>
        /// Gets or sets the call start time.
        /// </summary>
        /// <value>
        /// The call start time.
        /// </value>
        public DateTime? CallStartTime { get; set; }

        /// <summary>
        /// Gets or sets the call end time.
        /// </summary>
        /// <value>
        /// The call end time.
        /// </value>
        public DateTime? CallEndTime { get; set; }

        /// <summary>
        /// Gets or sets the call status identifier.
        /// </summary>
        /// <value>
        /// The call status identifier.
        /// </value>
        public Int16? CallStatusID { get; set; }

        /// <summary>
        /// Gets or sets the program unit identifier.
        /// </summary>
        /// <value>
        /// The program unit identifier.
        /// </value>
        public long? ProgramUnitID { get; set; }

        /// <summary>
        /// Gets or sets the County.
        /// </summary>
        /// <value>
        /// The county identifier.
        /// </value>
        public int? CountyID { get; set; }

        /// <summary>
        /// Gets or sets the call center priority identifier.
        /// </summary>
        /// <value>
        /// The call center priority identifier.
        /// </value>
        public Int16? CallPriorityID { get; set; }

        /// <summary>
        /// Gets or sets the suicide homicide identifier.
        /// </summary>
        /// <value>
        /// The suicide homicide identifier.
        /// </value>
        public Int16? SuicideHomicideID { get; set; }

        /// <summary>
        /// Gets or sets the date of incident.
        /// </summary>
        /// <value>
        /// The date of incident.
        /// </value>
        public DateTime? DateOfIncident { get; set; }

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
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client and caller are same.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is caller client same; otherwise, <c>false</c>.
        /// </value>
        public bool IsCallerClientSame { get; set; }

        /// <summary>
        /// Gets or sets the call center type identifier.
        /// </summary>
        /// <value>
        /// The call center type identifier.
        /// </value>
        public short CallCenterTypeID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [follow up required].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [follow up required]; otherwise, <c>false</c>.
        /// </value>
        public bool? FollowUpRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [IsLinkedToContact].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [IsLinkedToContact]; otherwise, <c>false</c>.
        /// </value>
        public bool? IsLinkedToContact { get; set; }

        /// <summary>
        /// Gets or sets the parent call center header identifier.
        /// </summary>
        /// <value>
        /// The parent call center header identifier.
        /// </value>
        public long? ParentCallCenterHeaderID { get; set; }
    }
}