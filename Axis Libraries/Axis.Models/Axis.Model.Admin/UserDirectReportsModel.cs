using Axis.Model.Account;

namespace Axis.Model.Admin
{
    public class UserDirectReportsModel : UserModel
    {
        /// <summary>
        /// Gets or sets the mapping identifier.
        /// </summary>
        /// <value>
        /// The mapping identifier.
        /// </value>
        public long? MappingID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is supervisor.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is supervisor; otherwise, <c>false</c>.
        /// </value>
        public bool IsSupervisor { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public long? ParentID { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has supervisor.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has supervisor; otherwise, <c>false</c>.
        /// </value>
        public bool HasSupervisor { get; set; }
    }
}
