using Axis.PresentationEngine.Areas.RecordedServices.Models;
using Axis.PresentationEngine.Areas.RecordedServices.Translator;
using Axis.Service;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.RecordedServices;
using System.Collections.Specialized;
using Axis.Constant;
using Axis.Configuration;
using System.Globalization;

namespace Axis.PresentationEngine.Areas.RecordedServices.Repository
{
    public class VoidServiceRepository : IVoidServiceRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "VoidService/";

        public VoidServiceRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// void the Service.
        /// </summary>
        /// <param name="voidService">voidService.</param>
        /// <returns></returns>
        public Response<VoidServiceViewModel> VoidService(VoidServiceViewModel voidService)
        {
            var apiUrl = baseRoute + "VoidRecordedService";
            var response = communicationManager.Post<VoidServiceModel,Response<VoidServiceModel>>(voidService.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// void the Service.
        /// </summary>
        /// <param name="voidService">voidService.</param>
        /// <returns></returns>
        public Response<VoidServiceViewModel> VoidServiceCallCenter(VoidServiceViewModel voidService)
        {
            var apiUrl = baseRoute + "VoidServiceCallCenter";
            var response = communicationManager.Post<VoidServiceModel, Response<VoidServiceModel>>(voidService.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Get void the Service.
        /// </summary>
        /// <param name="voidService">voidService.</param>
        /// <returns></returns>
        public Response<VoidServiceViewModel> GetVoidService(int serviceRecordingID)
        {
            var apiUrl = baseRoute + "GetVoidService";
            var param = new NameValueCollection { { "serviceRecordingID", serviceRecordingID.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Get<Response<VoidServiceModel>>(param, apiUrl);
            return response.ToViewModel();
        }
    }
}