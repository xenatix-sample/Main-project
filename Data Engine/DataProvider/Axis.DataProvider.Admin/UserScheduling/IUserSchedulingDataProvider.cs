using Axis.Model.Admin.UserScheduling;
using Axis.Model.Common;

namespace Axis.DataProvider.Admin
{
    public interface IUserSchedulingDataProvider
    {
        Response<UserSchedulingModel> GetUserFacilities(int userID);

        Response<UserSchedulingModel> GetUserFacilitySchedule(int userID, int facilityID);

        Response<UserSchedulingModel> SaveUserFacilitySchedule(UserSchedulingModel userFacilitySchedule);
    }
}
