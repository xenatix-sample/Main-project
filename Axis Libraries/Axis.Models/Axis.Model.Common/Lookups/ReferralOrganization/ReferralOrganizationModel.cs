namespace Axis.Model.Common
{
    public class ReferralOrganizationModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Organization identifier.
        /// </summary>
        /// <value>
        /// The Organization identifier.
        /// </value>
        public int ReferralOrganizationID { get; set; }
        /// <summary>
        /// Gets or sets the Organization.
        /// </summary>
        /// <value>
        /// The Organization.
        /// </value>
        public string ReferralOrganization { get; set; }
    }
}
