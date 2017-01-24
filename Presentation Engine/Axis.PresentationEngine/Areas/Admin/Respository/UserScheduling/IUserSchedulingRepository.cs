using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models.UserScheduling;
using System.Threading.Tasks;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public interface IUserSchedulingRepository
    {
        Response<UserSchedulingViewModel> GetUserFacilities(int userID, bool isMyProfile);

        Task<Response<UserSchedulingViewModel>> GetUserFacilitiesAsync(int userID, bool isMyProfile);

        Response<UserSchedulingViewModel> GetUserFacilitySchedule(int userID, int facilityID, bool isMyProfile);

        Response<UserSchedulingViewModel> SaveUserFacilitySchedule(UserSchedulingViewModel userFacilitySchedule, bool isMyProfile);
    }
}