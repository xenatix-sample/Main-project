namespace Axis.Model.Common
{
    public class ProvidersModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the phone identifier.
        /// </summary>
        /// <value>
        /// The phone identifier.
        /// </value>
        ///      
        public long? PhoneID { get; set; }
        /// <summary>
        /// Gets or sets the ContactNumber identifier.
        /// </summary>
        /// <value>
        /// The ContactNumber identifier.
        /// </value>
        public long? ContactNumberID { get; set; }
    }
}