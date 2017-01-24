using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Security
{
    public class ActiveDirectory
    {
        public static bool Authenticate(string userName, string password)
        {
            bool validation;
            try
            {
                LdapConnection ldapCon = new LdapConnection(new LdapDirectoryIdentifier((string)null, false, false));
                NetworkCredential nc = new NetworkCredential(userName, password, Environment.UserDomainName);
                ldapCon.Credential = nc;
                ldapCon.AuthType = AuthType.Negotiate;
                ldapCon.Bind(nc);
                validation = true;
            }
            catch (LdapException)
            {
                validation = false;
            }
            return validation;
        }
    }
}