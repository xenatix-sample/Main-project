using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net;
using Axis.RuleEngine.Security;
using System.Net.Http;
using Axis.Security;
using Axis.Constant;

namespace Axis.RuleEngine.Helpers.Filters
{
    /// <summary>
    ///  Used for authenticate/authorize each incoming request to IIS
    /// </summary>
    public class Authorization : AuthorizeAttribute
    {
        public string Module { get; set; }
        public string[] Modules { get; set; }
        public string PermissionKey { get; set; }
        public string[] PermissionKeys { get; set; }
        public string Permission { get; set; }
        public string[] Permissions { get; set; }

        /// <summary>
        /// Authenticate and authorize every request to IIS
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(HttpActionContext filterContext)
        {

            var permissionKeys = PermissionKey;
            var permissions = Permission;
            var modules = Module;
            if (PermissionKeys != null && PermissionKeys.Length != 0)
            {
                permissionKeys = string.Join("|", PermissionKeys);
            }

            if (Permissions != null && Permissions.Length != 0)
            {
                permissions = string.Join("|", Permissions);
            }

            if (Modules != null && Modules.Length != 0)
            {
                modules = string.Join("|", Modules);
            }

            var securityService = (ISecurityRuleEngine)filterContext.Request.GetDependencyScope().GetService(typeof(ISecurityRuleEngine));
            var hasPermission = securityService.VerifyRolePermission(AuthContext.Auth.User.UserID, modules, permissionKeys, permissions);

            if (hasPermission == null || hasPermission.DataItems == null || hasPermission.DataItems.Count == 0 || !hasPermission.DataItems.FirstOrDefault().Result)
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else
            {
                return;
            }
        }
    }
}