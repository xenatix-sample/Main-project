using System;

namespace Axis.Model.Common
{
    public class CallTypeModel
    {
        /// <summary>
        /// Gets or sets the call type identifier.
        /// </summary>
        /// <value>
        /// The call type identifier.
        /// </value>
        public Int16 CallTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the call.
        /// </summary>
        /// <value>
        /// The type of the call.
        /// </value>
        public string CallType { get; set; }
    }
}
