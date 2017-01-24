namespace Axis.Model.Common
{
    public class ServiceModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ServicesId
        /// 
        /// </summary>
        /// Service  Identifier
        public int ServiceID { get; set; }

        /// <summary>
        /// Gets or sets the Service Name
        /// </summary>
        /// Exposed Service Name
        public string ServiceName { get; set; }

        /// <summary>
        /// Get Program ID.
        /// </summary>
        public long ProgramID { get; set; }
    }
}
