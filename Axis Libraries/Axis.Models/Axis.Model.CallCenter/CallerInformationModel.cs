using Axis.Model.Common;
using System;

namespace Axis.Model.CallCenter
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class CallerInformationModel : BaseEntity
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
        /// Gets or sets the call center type identifier.
        /// </summary>
        /// <value>
        /// The call center type identifier.
        /// </value>
        public Int16 CallCenterTypeID { get; set; }

        /// <summary>
        /// Gets or sets the caller identifier.
        /// </summary>
        /// <value>
        /// The caller identifier.
        /// </value>
        public long? CallerID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long? ContactID { get; set; }


        /// <summary>
        /// Gets or sets the Contact Type ID.
        /// </summary>
        /// <value>
        /// The Contact Type ID.
        /// </value>
        public int? ContactTypeID { get; set; }



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
        /// Gets or sets the county identifier.
        /// </summary>
        /// <value>
        /// The county identifier.
        /// </value>
        public int? CountyID { get; set; }

        /////////////////// For Crisis Table  ///////////////////////

        /// <summary>
        /// Gets or sets the call center priority identifier.
        /// </summary>
        /// <value>
        /// The call center priority identifier.
        /// </value>
        public Int16? CallCenterPriorityID { get; set; }

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

        ////////////////// Common in Both tables ////////////////////

        /// <summary>
        /// Gets or sets the system created on.
        /// </summary>
        /// <value>
        /// The system created on.
        /// </value>
        public new DateTime SystemCreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the system modified on.
        /// </summary>
        /// <value>
        /// The system modified on.
        /// </value>
        public new DateTime SystemModifiedOn { get; set; }

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