using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using Axis.PresentationEngine.Helpers.Translator.ServiceRecording;
using Axis.Service;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.CallCenter.Repository.ServiceRecording
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.Plugins.CallCenter.Repository.ServiceRecording.IServiceRecordingRepository" />
    public class ServiceRecordingRepository : IServiceRecordingRepository
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ServiceRecording/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingRepository" /> class.
        /// </summary>
        public ServiceRecordingRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ServiceRecordingRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the service recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>

        public Response<ServiceRecordingViewModel> GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID)
        {
            const string apiUrl = baseRoute + "GetServiceRecording";
            var param = new NameValueCollection { { "SourceHeaderID", SourceHeaderID.ToString(CultureInfo.InvariantCulture) },
                                                    { "ServiceRecordingSourceID", ServiceRecordingSourceID.ToString(CultureInfo.InvariantCulture)}};
            var response = _communicationManager.Get<Response<ServiceRecordingModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Add the Service Recording.
        /// </summary>
        /// <param name="model">ServiceRecording ViewM odel.</param>
        /// <returns></returns>

        public Response<ServiceRecordingViewModel> AddServiceRecording(ServiceRecordingViewModel model)
        {
            const string apiUrl = baseRoute + "AddServiceRecording";
            var response = _communicationManager.Post<ServiceRecordingModel, Response<ServiceRecordingModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">ServiceRecording View Model</param>
        /// <returns></returns>

        public Response<ServiceRecordingViewModel> UpdateServiceRecording(ServiceRecordingViewModel model)
        {
            const string apiUrl = baseRoute + "UpdateServiceRecording";
            var response = _communicationManager.Put<ServiceRecordingModel, Response<ServiceRecordingModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the service recordings.
        /// </summary>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <param name="ContactID">The contact identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public Response<ServiceRecordingViewModel> GetServiceRecordings(int ServiceRecordingSourceID, long ContactID, DateTime? startDate, DateTime? endDate)
        {
            const string apiUrl = baseRoute + "GetServiceRecordings";
            var param = new NameValueCollection { { "ServiceRecordingSourceID", ServiceRecordingSourceID.ToString(CultureInfo.InvariantCulture) },
                                                    { "ContactID", ContactID.ToString(CultureInfo.InvariantCulture)},
                                                    { "startDate", startDate.HasValue? startDate.Value.ToString(CultureInfo.InvariantCulture):null},
                                                    { "endDate", endDate.HasValue?  endDate.Value.ToString(CultureInfo.InvariantCulture):null}
            };
            var response = _communicationManager.Get<Response<ServiceRecordingModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        public Response<ProgramUnitModel> GetProgramUnits(string datakey)
        {
            const string apiUrl = baseRoute + "GetProgramUnits";
            var param = new NameValueCollection { { "datakey", datakey.ToString(CultureInfo.InvariantCulture) }};
            var response = _communicationManager.Get<Response<ProgramUnitModel>>(param, apiUrl);
            return response;
        }
    }
}
