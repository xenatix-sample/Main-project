using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Account;

namespace Axis.Security
{
    public class AuthContext
    {
        public static AuthenticationModel Auth
        {
            get
            {
                return System.Threading.Thread.CurrentPrincipal is SecurityPrincipal ? ((SecurityPrincipal)(System.Threading.Thread.CurrentPrincipal)).Access : new AuthenticationModel();
            }
        }
    }
}
