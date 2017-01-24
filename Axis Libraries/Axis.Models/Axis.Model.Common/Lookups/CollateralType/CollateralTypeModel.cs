namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class CollateralTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the collateral type identifier.
        /// </summary>
        /// <value>
        /// The collateral type identifier.
        /// </value>
        public int CollateralTypeID { get; set; }

        /// <summary>
        /// Gets or sets the relationship group identifier.
        /// </summary>
        /// <value>
        /// The relationship group identifier.
        /// </value>
        public long RelationshipGroupID { get; set; }

        /// <summary>
        /// Gets or sets the type of the collateral.
        /// </summary>
        /// <value>
        /// The type of the collateral.
        /// </value>
        public string CollateralType { get; set; }
    }
}