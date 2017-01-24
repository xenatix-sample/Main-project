using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class UserRoleController : BaseApiController
    {
        #region Class Variables

        private readonly IUserRoleRepository _userRoleRepository;

        #endregion

        #region Constructors

        public UserRoleController(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            return _userRoleRepository.GetUserRoles(userID);
        }

        [HttpPost]
        public Response<UserViewModel> SaveUserRoles(UserViewModel user)
        {
            return _userRoleRepository.SaveUserRoles(user);
        }

        #endregion
    }
}