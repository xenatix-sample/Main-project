using Axis.Configuration;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Common.ServiceRecording
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.Service.Common.ServiceRecording.IServiceRecordingService" />
    public class ServiceRecordingService : IServiceRecordingService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ServiceRecording/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingService" /> class.
        /// </summary>
        public ServiceRecordingService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingService" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ServiceRecordingService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the Service Recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID)
        {
            const string apiUrl = BaseRoute + "GetServiceRecording";
            var param = new NameValueCollection { { "SourceHeaderID", SourceHeaderID.ToString(CultureInfo.InvariantCulture) },
                                                  { "ServiceRecordingSourceID", ServiceRecordingSourceID.ToString(CultureInfo.InvariantCulture) }};

            return _communicationManager.Get<Response<ServiceRecordingModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> AddServiceRecording(ServiceRecordingModel model)
        {
            const string apiUrl = BaseRoute + "AddServiceRecording";
            return _communicationManager.Post<ServiceRecordingModel, Response<ServiceRecordingModel>>(model, apiUrl);
        }

        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> UpdateServiceRecording(ServiceRecordingModel model)
        {
            const string apiUrl = BaseRoute + "UpdateServiceRecording";
            return _communicationManager.Put<ServiceRecordingModel, Response<ServiceRecordingModel>>(model, apiUrl);
        }


        /// <summary>
        /// Gets the service recordings.
        /// </summary>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <param name="ContactID">The contact identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> GetServiceRecordings(int ServiceRecordingSourceID, long ContactID, DateTime? startDate, DateTime? endDate)
        {
            const string apiUrl = BaseRoute + "GetServiceRecordings";
            var param = new NameValueCollection { { "ServiceRecordingSourceID", ServiceRecordingSourceID.ToString(CultureInfo.InvariantCulture) },
                                                  { "ContactID", ContactID.ToString(CultureInfo.InvariantCulture) } ,
                                                      { "startDate", startDate.HasValue? startDate.Value.ToString(CultureInfo.InvariantCulture):null},
                                                    { "endDate", endDate.HasValue?  endDate.Value.ToString(CultureInfo.InvariantCulture):null}
            };
            return _communicationManager.Get<Response<ServiceRecordingModel>>(param, apiUrl);
        }

        public Response<ProgramUnitModel> GetProgramUnits(string datakey)
        {
            const string apiUrl = BaseRoute + "GetProgramUnits";
            var requestXMLValueNvc = new NameValueCollection { { "datakey", datakey } };
            return _communicationManager.Get<Response<ProgramUnitModel>>(requestXMLValueNvc, apiUrl);
        }
    }
}
