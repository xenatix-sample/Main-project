using Axis.Model.Admin.UserScheduling;
using Axis.Model.Common;

namespace Axis.RuleEngine.Admin
{
    public interface IUserSchedulingRuleEngine
    {
        Response<UserSchedulingModel> GetUserFacilities(int userID);
        Response<UserSchedulingModel> GetUserFacilitySchedule(int userID, int facilityID);
        Response<UserSchedulingModel> SaveUserFacilitySchedule(UserSchedulingModel userFacilitySchedule);
    }
}
