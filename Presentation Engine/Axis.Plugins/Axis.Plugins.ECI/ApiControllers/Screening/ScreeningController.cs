using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.ECI.Model;
using Axis.Plugins.Screening.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using Newtonsoft.Json;

namespace Axis.Plugins.ECI.ApiControllers
{
    public class ScreeningController : BaseApiController
    {
        #region Class Variables

        private readonly IScreeningRepository _screeningRepository;

        #endregion

        #region Constructors

        public ScreeningController(IScreeningRepository screeningRepository)
        {
            _screeningRepository = screeningRepository;
        }

        public ScreeningController()
        {

        }

        #endregion

        [HttpPost]
        public Response<ScreeningViewModel> AddScreening(ScreeningViewModel screening)
        {
            return _screeningRepository.AddScreening(screening);
        }

        [HttpGet]
        public Response<ScreeningViewModel> GetScreenings(long contactID)
        {
            return _screeningRepository.GetScreenings(contactID);
        }

        [HttpGet]
        public Response<ScreeningViewModel> GetScreening(long screeningID)
        {
            return _screeningRepository.GetScreening(screeningID);
        }

        [HttpPut]
        public Response<ScreeningViewModel> UpdateScreening(ScreeningViewModel screening)
        {
            return _screeningRepository.UpdateScreening(screening);
        }

        [HttpDelete]
        public Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _screeningRepository.RemoveScreening(screeningID, modifiedOn);
        }

        [HttpGet]
        public Response<UserProgramFacilityModel> CoordinatorList(int programID)
        {
            return _screeningRepository.CoordinatorList(programID);
        }
    }
}
