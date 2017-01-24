using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Model;
using Axis.Plugins.ECI.Translator;
using Axis.Service;


namespace Axis.Plugins.Screening.Repository
{
    public class ScreeningRepository : IScreeningRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "Screening/";
        private const string baseRouteEligibility = "eligibilitydetermination/";


        public ScreeningRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public ScreeningRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

      
        public Response<ScreeningViewModel> AddScreening(ScreeningViewModel screening)
        {
            string apiUrl = baseRoute + "AddScreening";
            var response = communicationManager.Post<ScreeningModel, Response<ScreeningModel>>(screening.ToModel(), apiUrl);
            return response.ToViewModel();
        }

     
        public Response<ScreeningViewModel> GetScreenings(long contactID)
        {
            string apiUrl = baseRoute + "GetScreenings";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            var response = communicationManager.Get<Response<ScreeningModel>>(param, apiUrl);
            return response.ToViewModel();
        }

       
        public Response<ScreeningViewModel> GetScreening(long screeningID)
        {
            string apiUrl = baseRoute + "GetScreening";
            var param = new NameValueCollection();
            param.Add("screeningID", screeningID.ToString());
            var response = communicationManager.Get<Response<ScreeningModel>>(param, apiUrl);
            return response.ToViewModel();
        }

       
        public Response<ScreeningViewModel> UpdateScreening(ScreeningViewModel screening)
        {
            string apiUrl = baseRoute + "UpdateScreening";
            var response = communicationManager.Post<ScreeningModel, Response<ScreeningModel>>(screening.ToModel(), apiUrl);
            return response.ToViewModel();
        }

    
        public Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute + "RemoveScreening";
            var param = new NameValueCollection
            {
                {"screeningID", screeningID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<bool>>(param, apiUrl);
        }

     
        public Response<UserProgramFacilityModel> CoordinatorList(int programID)
        {
            string apiUrl = baseRouteEligibility + "GetTeamMemberList";
            var param = new NameValueCollection { { "programID", programID.ToString() } };

            var response = communicationManager.Get<Response<UserProgramFacilityModel>>(param, apiUrl);
            return response;
        }

    }
}
