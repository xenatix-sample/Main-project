using System;
using System.Net;

namespace Axis.Model.ReportingServices
{
    public class ReportServerCredentials // : IReportServerCredentials
    {
        private string _Username = string.Empty;
        private string _Password = string.Empty;
        private string _Authority = string.Empty;

        public string Username { set { this._Username = value; } }

        public string Password { set { this._Password = value; } }

        public string Authority { set { this._Authority = value; } }

        public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;
            return false;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._Username) || string.IsNullOrWhiteSpace(this._Password))
                    return System.Net.CredentialCache.DefaultNetworkCredentials;
                else
                    return new NetworkCredential(this._Username, this._Password, this._Authority);
            }
        }
    }

    public class Credentials : ICredentials
    {
        private string _Username = string.Empty;
        private string _Password = string.Empty;
        private string _Authority = string.Empty;

        public string Username { set { this._Username = value; } }

        public string Password { set { this._Password = value; } }

        public string Authority { set { this._Authority = value; } }

        public NetworkCredential GetCredential(Uri uri, string authType)
        {
            return new NetworkCredential(this._Username, this._Password, this._Authority);
        }
    }
}
