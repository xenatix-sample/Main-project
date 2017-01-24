using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using Axis.Configuration;
using Axis.Helpers;

namespace Axis.PresentationEngine.Helpers
{
    /// <summary>
    /// Used to handle authentication's related operations.
    /// </summary>
    public class WebSecurity
    {
        #region Public Methods

        /// <summary>
        /// Create authentication ticket and save into cookie
        /// </summary>
        /// <param name="userName">UserName of authenticated user</param>
        /// <param name="token">Role of authenticated user</param>
        public static void SignIn(string userName, string token)
        {
            int sessionTimeOut = 60;

            //Created authentication ticket
            var authTicket = new FormsAuthenticationTicket(userName, true, sessionTimeOut);

            //Encrypt authentication ticket
            string cookieContents = FormsAuthentication.Encrypt(authTicket);

            //Create cookie for authentication ticket
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
            {
                Path = FormsAuthentication.FormsCookiePath
            };

            //Save authentication ticket into cookie
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                var tokenKey = ApplicationSettings.Token;
                Cookies.AddCookies(tokenKey, token);
            }
        }

        /// <summary>
        /// Remove form authentication ticket from browser
        /// </summary>
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Returns true if user is authenticated.
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            return System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Get logged in user name
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            return System.Web.HttpContext.Current.User.Identity.Name;
        }

        /// <summary>
        /// Get logged in user's roles
        /// </summary>
        /// <returns></returns>
        public static string[] GetRoles()
        {
            string[] roles = null;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                roles = authTicket.UserData.Split(new char[] { '|' });
            }

            return roles;
        }

        #endregion Public Methods
    }
}