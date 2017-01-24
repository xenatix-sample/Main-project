using System;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.RuleEngine.ECI
{
    /// <summary>
    ///
    /// </summary>
    public interface IScreeningRuleEngine
    {
        Response<ScreeningModel> AddScreening(ScreeningModel screening);
        Response<ScreeningModel> GetScreenings(long contactId);
        Response<ScreeningModel> GetScreening(long screeningID);
        Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn);
        Response<ScreeningModel> UpdateScreening(ScreeningModel screeningModel);
    }
}