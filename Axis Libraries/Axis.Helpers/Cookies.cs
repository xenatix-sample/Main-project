using System;
using System.Linq;
using System.Web;

namespace Axis.Helpers
{
    /// <summary>
    /// This class is used for defining all cookies operations
    /// </summary>
    public class Cookies
    {
        #region Public Methods

        /// <summary>
        /// Add value to cookie
        /// </summary>
        /// <param name="key">Cookie key name</param>
        /// <param name="value">Cookie value</param>
        public static void AddCookies(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Expires = DateTime.Now.AddDays(15);
            HttpContext.Current.Response.SetCookie(cookie);
        }

        /// <summary>
        /// Get value from cookie based on key
        /// </summary>
        /// <param name="key">Cookie key name</param>
        /// <returns>Returns value from cookie</returns>
        public static string GetCookies(string key)
        {
            string cookiesData = string.Empty;

            //Checks if cookie exists
            if (HttpContext.Current != null && HttpContext.Current.Request.Cookies.AllKeys.Contains(key))
            {
                //Decrypt cookie value
                cookiesData = HttpContext.Current.Request.Cookies[key].Value;
            }

            return cookiesData;
        }

        /// <summary>
        /// Remove cookie
        /// </summary>
        /// <param name="key">Cookie key name</param>
        public static void RemoveCookies(string key)
        {
            //Checks if cookie exists
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(key))
            {
                HttpCookie Cookie = HttpContext.Current.Request.Cookies[key];

                //Cookie will be removed after setting expire date to past
                Cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(Cookie);
            }
        }

        #endregion Public Methods
    }
}