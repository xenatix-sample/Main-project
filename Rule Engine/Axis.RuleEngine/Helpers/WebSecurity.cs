using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using Axis.Model.Account;
using Axis.Security;
using Axis.Configuration;
using System.Diagnostics;

namespace Axis.RuleEngine.Service.Helpers
{
    public class WebSecurity
    {
        public static void SignIn(UserModel user, AccessTokenModel token)
        {
            var principal = new SecurityPrincipal(new GenericIdentity(token.UserName), new string[] { "Roles" });

            principal.Access.User = user;
            principal.Access.Token = token;

            Thread.CurrentPrincipal = principal;            
        }
    }
}