
using Axis.Model.Admin.UserScheduling;
using Axis.Model.Common;
using Axis.Service.Admin;
namespace Axis.RuleEngine.Admin
{
    public class UserSchedulingRuleEngine : IUserSchedulingRuleEngine
    {
        #region Class Variables

        private readonly IUserSchedulingService _userSchedulingService;

        #endregion

        #region Constructors

        public UserSchedulingRuleEngine(IUserSchedulingService userSchedulingService)
        {
            _userSchedulingService = userSchedulingService;
        }

        #endregion

        #region Public Methods

        public Response<UserSchedulingModel> GetUserFacilities(int userID)
        {
            return _userSchedulingService.GetUserFacilities(userID);
        }

        public Response<UserSchedulingModel> GetUserFacilitySchedule(int userID, int facilityID)
        {
            return _userSchedulingService.GetUserFacilitySchedule(userID, facilityID);
        }

        public Response<UserSchedulingModel> SaveUserFacilitySchedule(UserSchedulingModel userFacilitySchedule)
        {
            return _userSchedulingService.SaveUserFacilitySchedule(userFacilitySchedule);
        }

        #endregion
    }
}
