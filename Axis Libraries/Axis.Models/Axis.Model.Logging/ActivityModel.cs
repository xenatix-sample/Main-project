using Axis.Model.Common;
namespace Axis.Model.Logging
{
    public class ActivityModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ActivityID PK
        /// </summary>
        public int ActivityID { get; set; }

        /// <summary>
        /// Gets or sets the short message
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the full exception
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the page URL
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the referrer URL
        /// </summary>
        public string ReferrerUrl { get; set; }
    }
}
