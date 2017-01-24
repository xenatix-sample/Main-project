using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models.UserScheduling;
using Axis.Service;
using System.Collections.Specialized;
using System.Collections.Generic;
using Axis.Model.Admin.UserScheduling;
using Axis.PresentationEngine.Areas.Admin.Translator;
using System.Threading.Tasks;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public class UserSchedulingRepository : IUserSchedulingRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "userscheduling/";

        #endregion Class Variables

        #region Constructors
        public UserSchedulingRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public UserSchedulingRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }
        #endregion Constructors

        #region Public Methods

        public Response<UserSchedulingViewModel> GetUserFacilities(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfileFacilities" : "GetUserFacilities";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<UserSchedulingModel>>(param, apiUrl);
            return response.ToViewModel();
        }


        public async Task<Response<UserSchedulingViewModel>> GetUserFacilitiesAsync(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfileFacilities" : "GetUserFacilities";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return (await _communicationManager.GetAsync<Response<UserSchedulingModel>>(param, apiUrl)).ToViewModel();
        }


        public Response<UserSchedulingViewModel> GetUserFacilitySchedule(int userID, int facilityID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfileFacilitySchedule" : "GetUserFacilitySchedule";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() }, { "facilityID", facilityID.ToString()} };

            var response = _communicationManager.Get<Response<UserSchedulingModel>>(param, apiUrl);
            return response.ToViewModel();
        }


        public Response<UserSchedulingViewModel> SaveUserFacilitySchedule(UserSchedulingViewModel userFacilitySchedule, bool isMyProfile)
        {
            var route = isMyProfile ? "SaveMyProfileFacilitySchedule" : "SaveUserFacilitySchedule";
            var apiUrl = BaseRoute + route;

            var response = _communicationManager.Post<UserSchedulingModel, Response<UserSchedulingModel>>(userFacilitySchedule.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        #endregion
    }
}