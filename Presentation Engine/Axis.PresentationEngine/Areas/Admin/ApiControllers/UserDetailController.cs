using System.Web.Http;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class UserDetailController : BaseApiController
    {
        #region Class Variables

        private readonly IUserDetailRepository _userDetailRepository;

        #endregion

        #region Constructors

        public UserDetailController(IUserDetailRepository userDetailRepository)
        {
            _userDetailRepository = userDetailRepository;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public Response<UserViewModel> GetUser(int userID)
        {
            return _userDetailRepository.GetUser(userID);
        }

        [HttpPost]
        public Response<UserViewModel> AddUser(UserViewModel userDetail)
        {
            var result = _userDetailRepository.AddUser(userDetail);
            ClearCache(result);
            return result;
        }

        [HttpPost]
        public Response<UserViewModel> UpdateUser(UserViewModel userDetail)
        {
            var result = _userDetailRepository.UpdateUser(userDetail);
            ClearCache(result);
            return result;
        }

        #endregion
    }
}
