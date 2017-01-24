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
        public IHttpActionResult AddScreening(ScreeningModel ediScreening)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciDataProvider.AddScreening(ediScreening), Request);
        }

        [HttpGet]
        public IHttpActionResult GetScreenings(long contactID, long screeningID)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciDataProvider.GetScreenings(contactID, screeningID), Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateScreening(ScreeningModel ediScreening)
        {
            return new HttpResult<Response<ScreeningModel>>(_eciDataProvider.UpdateScreening(ediScreening), Request);
        }

        [HttpDelete]
        public IHttpActionResult RemoveScreening(long screeningID)
        {
            return new HttpResult<Response<bool>>(_eciDataProvider.RemoveScreening(screeningID), Request);
        }

        #endregion
    }
}