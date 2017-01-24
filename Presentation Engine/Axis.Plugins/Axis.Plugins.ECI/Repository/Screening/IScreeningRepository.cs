using System;
using Axis.Model.Common;
using Axis.Plugins.ECI.Model;

namespace Axis.Plugins.Screening.Repository
{
    public interface IScreeningRepository
    {
        Response<ScreeningViewModel> AddScreening(ScreeningViewModel screeningViewModel);
        Response<ScreeningViewModel> GetScreenings(long contactID);
        Response<ScreeningViewModel> GetScreening(long screeningID);
        Response<ScreeningViewModel> UpdateScreening(ScreeningViewModel screeningViewModel);
        Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn);
        Response<UserProgramFacilityModel> CoordinatorList(int programID);

    }
}
