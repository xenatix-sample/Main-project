namespace Axis.Model.Common
{
    public class PresentingProblemTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the presenting problem type identifier.
        /// </summary>
        /// <value>
        /// The presenting problem type identifier.
        /// </value>
        public short PresentingProblemTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the presenting problem.
        /// </summary>
        /// <value>
        /// The type of the presenting problem.
        /// </value>
        public string PresentingProblemType { get; set; }
    }
}