using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Logging
{
    public class ExceptionModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ExceptionID PK
        /// </summary>
        public int ExceptionID { get; set; }

        /// <summary>
        /// Gets or sets the short message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the full exception
        /// </summary>
        public string Source { get; set; }

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
