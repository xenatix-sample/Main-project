using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class UserHeaderController : BaseApiController
    {
        #region Class Variables

        private readonly IUserHeaderRepository _userHeaderRepository;

        #endregion

        #region Constructors

        public UserHeaderController(IUserHeaderRepository userDetailRepository)
        {
            _userHeaderRepository = userDetailRepository;
        }

        #endregion

        #region Public Methods

        #endregion
    }
}