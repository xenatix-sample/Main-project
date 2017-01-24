using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration.Admission
{
    /// <summary>
    /// Admission Service
    /// </summary>
    public class AdmissionService : IAdmissionService
    {
          /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "Admission/";

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmissionService"/> class.
        /// </summary>
        public AdmissionService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmissionService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public AdmissionService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }


        /// <summary>
        /// Gets the Admission.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AdmissionModal> GetAdmission(long contactId)
        {
            const string apiUrl = BaseRoute + "GetAdmission";
            var requestId = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<AdmissionModal>>(requestId, apiUrl);
        }

        /// <summary>
        /// Add Admission.
        /// </summary>
        /// <param name="admission">The Admission.</param>
        /// <returns></returns>
        public Response<AdmissionModal> AddAdmission(AdmissionModal model)
        {
            const string apiUrl = BaseRoute + "AddAdmission";
            return _communicationManager.Post<AdmissionModal, Response<AdmissionModal>>(model, apiUrl);
        }


        /// <summary>
        /// Updates admission.
        /// </summary>
        /// <param name="admission">The admission.</param>
        /// <returns></returns>
        public Response<AdmissionModal> UpdateAdmission(AdmissionModal model)
        {
            const string apiUrl = BaseRoute + "UpdateAdmission";
            return _communicationManager.Put<AdmissionModal, Response<AdmissionModal>>(model, apiUrl);
        }
    }
}
