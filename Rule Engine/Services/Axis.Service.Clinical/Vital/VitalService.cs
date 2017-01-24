using System;
using Axis.Configuration;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Clinical.Vital
{
    public class VitalService : IVitalService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "vital/";

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalService" /> class.
        /// </summary>
        public VitalService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<VitalModel> GetContactVitals(long ContactId)
        {
            const string apiUrl = BaseRoute + "GetContactVitals";
            var requestXMLValueNvc = new NameValueCollection { { "ContactId", ContactId.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<VitalModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the Vital.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<VitalModel> AddVital(VitalModel vital)
        {
            const string apiUrl = BaseRoute + "AddVital";
            return communicationManager.Post<VitalModel, Response<VitalModel>>(vital, apiUrl);
        }

        /// <summary>
        /// Updates the Vital.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<VitalModel> UpdateVital(VitalModel vital)
        {
            const string apiUrl = BaseRoute + "UpdateVital";
            return communicationManager.Put<VitalModel, Response<VitalModel>>(vital, apiUrl);
        }

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<VitalModel> DeleteVital(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteVital";
            var requestXMLValueNvc = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<VitalModel>>(requestXMLValueNvc, apiUrl);
        }
    }
}
