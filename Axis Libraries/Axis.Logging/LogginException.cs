using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Logging
{
    /// <summary>
    /// This expection will throw when a exception has already been logged in any layer.
    /// </summary>
    public class XenatixException : Exception
    {
        public string ErrorMessage { get; set; }
        public XenatixException()
            : base()
        {
        
        }

        public XenatixException(string message)
        {
            ErrorMessage = message;
        }
    }
}
