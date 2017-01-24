using System;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Configuration;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.Admin.DivisionProgram
{
    public class DivisionProgramService : IDivisionProgramService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "divisionprogram/";

        #endregion

        #region Constructors

        public DivisionProgramService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<DivisionProgramModel> GetDivisionPrograms(int userID)
        {
            var apiUrl = BaseRoute + "GetDivisionPrograms";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<DivisionProgramModel>>(param, apiUrl);
        }

        public Response<DivisionProgramModel> SaveDivisionProgram(DivisionProgramModel divisionProgram)
        {
            var apiUrl = BaseRoute + "SaveDivisionProgram";

            return _communicationManager.Post<DivisionProgramModel, Response<DivisionProgramModel>>(divisionProgram, apiUrl);
        }

        #endregion
    }
}
