namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ClientIdentifierTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the client identifier type identifier.
        /// </summary>
        /// <value>
        /// The client identifier type identifier.
        /// </value>
        public int ClientIdentifierTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the client identifier.
        /// </summary>
        /// <value>
        /// The type of the client identifier.
        /// </value>
        public string ClientIdentifierType { get; set; }
    }
}