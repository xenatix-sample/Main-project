
using Axis.Configuration;
using Axis.Model.Admin.UserScheduling;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;
namespace Axis.Service.Admin
{
    public class UserSchedulingService : IUserSchedulingService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "userscheduling/";

        #endregion
        
        #region Constructors

        public UserSchedulingService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<UserSchedulingModel> GetUserFacilities(int userID)
        {
            string apiUrl = BaseRoute + "GetUserFacilities";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<UserSchedulingModel>>(param, apiUrl);
        }

        public Response<UserSchedulingModel> GetUserFacilitySchedule(int userID, int facilityID)
        {
            string apiUrl = BaseRoute + "GetUserFacilitySchedule";
            var param = new NameValueCollection { { "userID", userID.ToString() }, { "facilityID", facilityID.ToString() } };

            return _communicationManager.Get<Response<UserSchedulingModel>>(param, apiUrl);
        }

        public Response<UserSchedulingModel> SaveUserFacilitySchedule(UserSchedulingModel userFacilitySchedule)
        {
            string apiUrl = BaseRoute + "SaveUserFacilitySchedule";
            return _communicationManager.Post<UserSchedulingModel, Response<UserSchedulingModel>>(userFacilitySchedule, apiUrl);
        }

        #endregion
    }
}
