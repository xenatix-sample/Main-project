using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Account
{
    public class AuthenticationModel
    {
        public AuthenticationModel()
        {
            User = new UserModel();
            Token = new AccessTokenModel();
        }

        public UserModel User { get; set; }
        public AccessTokenModel Token { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// set Resultcode
        /// </summary>
        public int? Resultcode { get; set; }
    }
}
