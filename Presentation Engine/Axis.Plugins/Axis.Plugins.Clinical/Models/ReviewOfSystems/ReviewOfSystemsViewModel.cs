using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.Clinical.Models.ReviewOfSystems
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the RoSID identifier.
        /// </summary>
        /// <value>
        /// The review of systems identifier.
        /// </value>
        public long RoSID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the date entered.
        /// </summary>
        /// <value>
        /// The date entered.
        /// </value>
        public DateTime DateEntered { get; set; }

        /// <summary>
        /// Gets or sets the reviewd by.
        /// </summary>
        /// <value>
        /// The reviewd by.
        /// </value>
        public int ReviewdBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the reviewd by.
        /// </summary>
        /// <value>
        /// The name of the reviewd by.
        /// </value>
        public string ReviewdByName { get; set; }

        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>
        /// The response identifier.
        /// </value>
        public long ResponseID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is review changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is review changed; otherwise, <c>false</c>.
        /// </value>
        public bool? IsReviewChanged { get; set; }

        /// <summary>
        /// Gets or sets the last assesment on.
        /// </summary>
        /// <value>
        /// The last assesment on.
        /// </value>
        public DateTime? LastAssessmentOn { get; set; }
    }
}