
using System;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataProvider.ECI.Screening
{
    public interface IScreeningDataProvider
    {
        Response<ScreeningModel> GetScreenings(long contactID);
        Response<ScreeningModel> GetScreening(long screeningID);
        Response<ScreeningModel> AddScreening(ScreeningModel screening);
        Response<ScreeningModel> UpdateScreening(ScreeningModel screening);
        Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn);
    }
}
