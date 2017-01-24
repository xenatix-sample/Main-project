using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models.UserScheduling;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class UserSchedulingController : BaseApiController
    {
        #region Class Variables

        private readonly IUserSchedulingRepository _schedulingRepository;

        #endregion

        #region Constructors

        public UserSchedulingController(IUserSchedulingRepository schedulingRepository)
        {
            _schedulingRepository = schedulingRepository;
        }

        #endregion

        #region Public Methods

        public Response<UserSchedulingViewModel> GetUserFacilities(int userID, bool isMyProfile)
        {
            return _schedulingRepository.GetUserFacilities(userID, isMyProfile);
        }

        public Response<UserSchedulingViewModel> GetUserFacilitySchedule(int userID, int facilityID, bool isMyProfile)
        {
            return _schedulingRepository.GetUserFacilitySchedule(userID, facilityID, isMyProfile);
        }

        public Response<UserSchedulingViewModel> SaveUserFacilitySchedule(UserSchedulingViewModel userFacilitySchedule, bool isMyProfile)
        {
            return _schedulingRepository.SaveUserFacilitySchedule(userFacilitySchedule, isMyProfile);
        }

        #endregion
    }
}