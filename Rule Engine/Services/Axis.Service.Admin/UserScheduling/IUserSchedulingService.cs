using Axis.Model.Admin.UserScheduling;
using Axis.Model.Common;

namespace Axis.Service.Admin
{
    public interface IUserSchedulingService
    {
        Response<UserSchedulingModel> GetUserFacilities(int userID);
        Response<UserSchedulingModel> GetUserFacilitySchedule(int userID, int facilityID);
        Response<UserSchedulingModel> SaveUserFacilitySchedule(UserSchedulingModel userFacilitySchedule);
    }
}
