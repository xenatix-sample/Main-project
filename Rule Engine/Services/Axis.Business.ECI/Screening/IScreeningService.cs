using System;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.Service.ECI
{
    /// <summary>
    ///
    /// </summary>
    public interface IScreeningService
    {
        Response<ScreeningModel> AddScreening(ScreeningModel consent);
        Response<ScreeningModel> GetScreenings(long contactID);
        Response<ScreeningModel> GetScreening(long screeningID);
        Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn);
        Response<ScreeningModel> UpdateScreening(ScreeningModel screeningModel);
    }
}