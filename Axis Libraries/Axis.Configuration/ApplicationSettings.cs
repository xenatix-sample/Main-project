using System;
using System.Configuration;

namespace Axis.Configuration
{
    public class ApplicationSettings
    {
        public static string ApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiUrl"].ToString();
            }
        }

        public static string Token
        {
            get
            {
                return ConfigurationManager.AppSettings["Token"].ToString();
            }
        }

        public static bool EnableCaching
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCaching"]);
            }
        }

        public static string LogPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LogPath"];
            }
        }

        public static bool EnableLogging
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLogging"]);
            }
        }

        public static bool EnableDebugMessages
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDebugMessages"]);
            }
        }

        public static int LoggingMode
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LoggingMode"]);
            }
        }

        public static string ClearPluginsShadowDirectoryOnStartup
        {
            get
            {
                return "true";
            }
        }

        public static string PresentationApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PresentationApiUrl"];
            }
        }

        public static string ReportViewerPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportViewerPath"];
            }
        }

        public static bool EnableSsrsTest
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableSsrsTest"));
                }
                catch (Exception) // You don't need to have a variable name if you are not going to use it. Prevents a warning.
                {
                    return false;
                }
            }
        }

        public static string ReportPath
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("ReportPath"));
            }
        }

       
    }
}
