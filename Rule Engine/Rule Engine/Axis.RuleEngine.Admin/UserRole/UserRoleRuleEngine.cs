using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Service.Admin;

namespace Axis.RuleEngine.Admin
{
    public class UserRoleRuleEngine : IUserRoleRuleEngine
    {
        #region Class Variables

        private readonly IUserRoleService _userRoleService;

        #endregion

        #region Constructors

        public UserRoleRuleEngine(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        #endregion

        #region Public Methods

        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            return _userRoleService.GetUserRoles(userID);
        }

        public Response<UserModel> SaveUserRoles(UserModel user)
        {
            return _userRoleService.SaveUserRoles(user);
        }

        #endregion
    }
}
