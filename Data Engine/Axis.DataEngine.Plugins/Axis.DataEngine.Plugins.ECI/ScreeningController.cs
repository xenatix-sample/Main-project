using System;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI.Screening;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataEngine.Plugins.ECI
{
    public class ScreeningController : BaseApiController
    {
        IScreeningDataProvider _eciDataProvider;

        public ScreeningController(IScreeningDataProvider eciDataProvider)
        {
            _eciDataProvider = eciDataProvider;
        }

        #region Public Methods

        [HttpPost]
        public IHttpActionResult AddScreening(ScreeningModel eciScreening)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciDataProvider.AddScreening(eciScreening), Request);
        }

        [HttpGet]
        public IHttpActionResult GetScreenings(long contactID)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciDataProvider.GetScreenings(contactID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetScreening(long screeningID)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciDataProvider.GetScreening(screeningID), Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateScreening(ScreeningModel eciScreening)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciDataProvider.UpdateScreening(eciScreening), Request);
        }

        [HttpDelete]
        public IHttpActionResult RemoveScreening(long screeningID, DateTime modifiedOn)
        {
            return new HttpResult<Response<bool>>(_eciDataProvider.RemoveScreening(screeningID, modifiedOn), Request);
        }

        #endregion
    }
}