using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Admin;
using Axis.Model.Admin;
using Axis.Model.Admin.UserScheduling;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class UserSchedulingController : BaseApiController
    {
        #region Class Variables

        readonly IUserSchedulingDataProvider _userSchedulingDataProvider = null;

        #endregion

         #region Constructors

        public UserSchedulingController(IUserSchedulingDataProvider userSchedulingDataProvider)
        {
            _userSchedulingDataProvider = userSchedulingDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetUserFacilities(int userID)
        {
            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingDataProvider.GetUserFacilities(userID), Request);
        }
        

        [HttpGet]
        public IHttpActionResult GetUserFacilitySchedule(int userID, int facilityID)
        {
            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingDataProvider.GetUserFacilitySchedule(userID, facilityID), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveUserFacilitySchedule(UserSchedulingModel userFacilitySchedule)
        {
            return new HttpResult<Response<UserSchedulingModel>>(_userSchedulingDataProvider.SaveUserFacilitySchedule(userFacilitySchedule), Request);
        }

        #endregion


    }
}