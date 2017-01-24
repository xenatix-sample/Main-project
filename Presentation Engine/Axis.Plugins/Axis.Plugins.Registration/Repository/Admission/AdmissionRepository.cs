using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Translator;

namespace Axis.Plugins.Registration.Repository.Admission
{
    /// <summary>
    /// Admission repository
    /// </summary>
    public class AdmissionRepository : IAdmissionRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "Admission/";

        /// <summary>
        /// constructor
        /// </summary>
        public AdmissionRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">token</param>
        public AdmissionRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To get Admission list
        /// </summary>
        /// <param name="contactID">Contact Id of contact</param>       
        /// <returns>Response of type AdmissionModal</returns>
       
        public Response<AdmissionViewModal> GetAdmission(long contactID)
        {
            string apiUrl = baseRoute + "GetAdmission";
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Get<Response<AdmissionModal>>(parameters, apiUrl);
            return response.ToViewModel();
        }


        /// <summary>
        /// To add admission
        /// </summary>        
        /// <returns>Response of type AdmissionViewModal</returns>
        public Response<AdmissionViewModal> AddAdmission(AdmissionViewModal admission)
        {
            const string apiUrl = baseRoute + "AddAdmission";
            var response = communicationManager.Post<AdmissionModal, Response<AdmissionModal>>(admission.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// To update admission
        /// </summary>
        /// <param name="admission"></param>
        /// <returns></returns>
      
        public Response<AdmissionViewModal> UpdateAdmission(AdmissionViewModal admission)
        {
            const string apiUrl = baseRoute + "UpdateAdmission";
            var response = communicationManager.Put<AdmissionModal, Response<AdmissionModal>>(admission.ToModel(), apiUrl);
            return response.ToViewModel();
        }
    }
}
