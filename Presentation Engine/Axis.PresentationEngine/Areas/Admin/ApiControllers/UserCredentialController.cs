using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class UserCredentialController : BaseApiController
    {
        #region Class Variables

        private readonly IUserCredentialRepository _userCredentialRepository;

        #endregion

        #region Constructors

        public UserCredentialController(IUserCredentialRepository userCredentialRepository)
        {
            _userCredentialRepository = userCredentialRepository;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public Response<UserCredentialModel> GetUserCredentials(int userID, bool isMyProfile)
        {
            return _userCredentialRepository.GetUserCredentials(userID, isMyProfile);
        }

        [HttpGet]
        public Response<UserCredentialModel> GetUserCredentialsWithServiceID(int userID, long moduleComponentID)
        {
            return _userCredentialRepository.GetUserCredentialsWithServiceID(userID, moduleComponentID);
        }

        [HttpPost]
        public Response<UserViewModel> SaveUserCredentials(UserViewModel user, bool isMyProfile)
        {
            var result = _userCredentialRepository.SaveUserCredentials(user, isMyProfile);
            ClearCache(result);
            return result;
        }

        #endregion
    }
}
