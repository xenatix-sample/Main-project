using System;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using Axis.Service;
using Axis.Configuration;
using Axis.Helpers;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Axis.Model.Admin;
using Axis.PresentationEngine.Areas.Admin.Translator;

namespace Axis.PresentationEngine.Areas.Admin.Respository.DivisionProgram
{
    public class DivisionProgramRepository : IDivisionProgramRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "divisionprogram/";

        #endregion Class Variables

        #region Constructors
        public DivisionProgramRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public DivisionProgramRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }
        #endregion Constructors

        public Response<DivisionProgramViewModel> GetDivisionPrograms(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfileDivisionPrograms" : "GetDivisionPrograms";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<DivisionProgramModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        public async Task<Response<DivisionProgramViewModel>> GetDivisionProgramsAsync(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfileDivisionPrograms" : "GetDivisionPrograms";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return (await _communicationManager.GetAsync<Response<DivisionProgramModel>>(param, apiUrl)).ToViewModel();
        }

        public Response<DivisionProgramViewModel> SaveDivisionProgram(DivisionProgramViewModel divisionProgram, bool isMyProfile)
        {
            var route = isMyProfile ? "SaveMyProfileDivisionProgram" : "SaveDivisionProgram";
            var apiUrl = BaseRoute + route;

            var response = _communicationManager.Post<DivisionProgramModel, Response<DivisionProgramModel>>(divisionProgram.ToModel(), apiUrl);
            return response.ToViewModel();
        }
    }
}