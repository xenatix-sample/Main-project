namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    public class RelationshipTypeModel
    {
        /// <summary>
        /// Gets or sets the relationship type identifier.
        /// </summary>
        /// <value>
        /// The relationship type identifier.
        /// </value>
        public int RelationshipTypeID { get; set; }

        /// <summary>
        /// Gets or sets the relationship group identifier.
        /// </summary>
        /// <value>
        /// The relationship group identifier.
        /// </value>
        public long RelationshipGroupID { get; set; }

        /// <summary>
        /// Gets or sets the type of the relation ship.
        /// </summary>
        /// <value>
        /// The type of the relation ship.
        /// </value>
        public string RelationshipType { get; set; }
    }
}