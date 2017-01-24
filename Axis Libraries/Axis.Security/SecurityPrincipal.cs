using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Account;

namespace Axis.Security
{
    public class SecurityPrincipal : GenericPrincipal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityPrincipal"/> class.
        /// </summary>
        /// <param name="identity">A basic implementation of <see cref="T:System.Security.Principal.IIdentity" /> that represents any user.</param>
        /// <param name="roles">An array of role names to which the user represented by the <paramref name="identity" /> parameter belongs.</param>
        public SecurityPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles)
        {
            Access = new AuthenticationModel();
        }
        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        /// <value>
        /// The user profile.
        /// </value>
        public AuthenticationModel Access { get; set; }
    }
}
