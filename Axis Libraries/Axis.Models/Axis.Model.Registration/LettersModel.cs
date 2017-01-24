using Axis.Model.Common;
using System;

namespace Axis.Model.Registration 
{
    public class LettersModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the contact letters identifier.
        /// </summary>
        /// <value>The contact letters identifier.</value>
        public long ContactLettersID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>The contact identifier.</value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>The assessment identifier.</value>
        public long? AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>The response identifier.</value>
        public long? ResponseID { get; set; }

        /// <summary>
        /// Gets or sets the sent date.
        /// </summary>
        /// <value>The sent date.</value>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int? UserID { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>The name of the provider.</value>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the letter outcome identifier.
        /// </summary>
        /// <value>The letter outcome identifier.</value>
        public int? LetterOutcomeID { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments { get; set; }
    }
}
