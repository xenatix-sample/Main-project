using System;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Service.ECI;

namespace Axis.RuleEngine.ECI
{
    /// <summary>
    ///
    /// </summary>
    public class ScreeningRuleEngine : IScreeningRuleEngine
    {
        #region Class Variables

        private IScreeningService _ScreeningService;

        #endregion

        #region Constructors

        public ScreeningRuleEngine(IScreeningService screeningService)
        {
            _ScreeningService = screeningService;
        }

        #endregion

        #region Public Methods

        public Response<ScreeningModel> AddScreening(ScreeningModel screening)
        {
            return _ScreeningService.AddScreening(screening);
        }

        public Response<ScreeningModel> GetScreenings(long contactId)
        {
            return _ScreeningService.GetScreenings(contactId);
        }

        public Response<ScreeningModel> GetScreening(long screeningID)
        {
            return _ScreeningService.GetScreening(screeningID);
        }

        public Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn)
        {
            return _ScreeningService.RemoveScreening(screeningID, modifiedOn);
        }

        public Response<ScreeningModel> UpdateScreening(ScreeningModel screeningModel)
        {
            return _ScreeningService.UpdateScreening(screeningModel);
        }

        #endregion

    }
}