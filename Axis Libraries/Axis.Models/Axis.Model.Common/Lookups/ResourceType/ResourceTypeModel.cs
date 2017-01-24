namespace Axis.Model.Common
{
    public class ResourceTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public short ResourceTypeID { get; set; }
        /// <summary>
        /// Gets or sets the type of the  resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public string ResourceType { get; set; }
    }
}
