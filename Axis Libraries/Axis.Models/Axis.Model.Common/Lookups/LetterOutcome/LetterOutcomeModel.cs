
namespace Axis.Model.Common
{
    public class LetterOutcomeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the letter outcome identifier.
        /// </summary>
        /// <value>The letter outcome identifier.</value>
        public int LetterOutcomeID { get; set; }
        /// <summary>
        /// Gets or sets the letter outcome.
        /// </summary>
        /// <value>The letter outcome.</value>
        public string LetterOutcome { get; set; }
    }
}
