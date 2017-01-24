using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.RuleEngine.Admin
{
    public interface IUserRoleRuleEngine
    {
        Response<UserRoleModel> GetUserRoles(int userID);
        Response<UserModel> SaveUserRoles(UserModel user);
    }
}
