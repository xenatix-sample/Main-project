using Axis.Model.CallCenter;
using Axis.Model.Common;
using System;

namespace Axis.Model
{
    public class CallCenterProgressNoteModel : CallerInformationModel
    {

        /// <summary>
        /// Gets or sets the natureof call.
        /// </summary>
        /// <value>
        /// The natureof call.
        /// </value>
        public string NatureofCall { get; set; }

        /// <summary>
        /// Gets or sets the call type identifier.
        /// </summary>
        /// <value>
        /// The call type identifier.
        /// </value>
        public Int16? CallTypeID { get; set; }

        /// <summary>
        /// Gets or sets the call type other.
        /// </summary>
        /// <value>
        /// The call type other.
        /// </value>
        public string CallTypeOther { get; set; }

        /// <summary>
        /// Gets or sets the progress note identifier.
        /// </summary>
        /// <value>
        /// The progress note identifier.
        /// </value>
        public long? ProgressNoteID { get; set; }

        /// <summary>
        /// Gets or sets the followup plan.
        /// </summary>
        /// <value>
        /// The followup plan.
        /// </value>
        public string FollowupPlan { get; set; }

        /// <summary>
        /// Gets or sets the client status identifier.
        /// </summary>
        /// <value>
        /// The client status identifier.
        /// </value>
        public Int16? ClientStatusID { get; set; }

        /// <summary>
        /// Gets or sets the client provider.
        /// </summary>
        /// <value>
        /// The client provider.
        /// </value>
        public string ClientProvider { get; set; }

        /// <summary>
        /// Gets or sets the note header identifier.
        /// </summary>
        /// <value>
        /// The note header identifier.
        /// </value>
        public long? NoteHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the client provider identifier.
        /// </summary>
        /// <value>
        /// The client provider identifier.
        /// </value>
        public int? ClientProviderID { get; set; }

        /// <summary>
        /// Gets or sets the describe other.
        /// </summary>
        /// <value>
        /// The describe other.
        /// </value>
        public string DescribeOther { get; set; }

        /// <summary>
        /// Gets or sets the behavioral category identifier.
        /// </summary>
        /// <value>
        /// The behavioral category identifier.
        /// </value>
        public Int16? BehavioralCategoryID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [note added].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [note added]; otherwise, <c>false</c>.
        /// </value>
        public bool NoteAdded { get; set; }

        /// <summary>
        /// Gets or sets the IsCallerSame identifier.
        /// </summary>
        /// <value>
        /// The IsCallerSame identifier.
        /// </value>
        public Boolean? IsCallerSame { get; set; }

        /// <summary>
        /// Gets or sets the NewCallerID identifier.
        /// </summary>
        /// <value>
        /// The NewCallerID identifier.
        /// </value>
        public long? NewCallerID { get; set; }
    }
}
