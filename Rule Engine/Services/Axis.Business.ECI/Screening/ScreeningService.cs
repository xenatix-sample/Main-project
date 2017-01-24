using System;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Security;

namespace Axis.Service.ECI
{
    /// <summary>
    ///
    /// </summary>
    public class ScreeningService : IScreeningService
    {
        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "screening/";

        public ScreeningService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #region Public Methods

        public Response<ScreeningModel> AddScreening(ScreeningModel consent)
        {
            var apiUrl = BaseRoute + "AddScreening";
            return _communicationManager.Post<ScreeningModel, Response<ScreeningModel>>(consent, apiUrl);
        }

        public Response<ScreeningModel> GetScreenings(long contactID)
        {
            var apiUrl = BaseRoute + "GetScreenings";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            return _communicationManager.Get<Response<ScreeningModel>>(param, apiUrl);
        }

        public Response<ScreeningModel> GetScreening(long screeningID)
        {
            var apiUrl = BaseRoute + "GetScreening";
            var param = new NameValueCollection();
            param.Add("screeningID", screeningID.ToString());

            return _communicationManager.Get<Response<ScreeningModel>>(param, apiUrl);
        }

        public Response<ScreeningModel> UpdateScreening(ScreeningModel screeningModel)
        {
            var apiUrl = BaseRoute + "UpdateScreening";
            return _communicationManager.Post<ScreeningModel, Response<ScreeningModel>>(screeningModel, apiUrl);
        }

        public Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn)
        {
            var apiUrl = BaseRoute + "RemoveScreening";
            var param = new NameValueCollection
            {
                {"screeningID", screeningID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return _communicationManager.Delete<Response<bool>>(param, apiUrl);
        }

        #endregion

    }
}