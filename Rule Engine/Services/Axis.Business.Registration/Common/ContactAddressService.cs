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

namespace Axis.Service.Registration.Common
{
    public class ContactAddressService :IContactAddressService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "contactAddress/";

        /// <summary>
        /// Constructor
        /// </summary>
        public ContactAddressService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public ContactAddressService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Get Contact Address
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactTypeID"></param>
        /// <returns></returns>
        public Response<ContactAddressModel> GetAddresses(long contactID, int contactTypeID)
        {
            const string apiUrl = BaseRoute + "GetAddresses";
            var requestId = new NameValueCollection { 
                            { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeId", contactTypeID.ToString(CultureInfo.InvariantCulture) } 
                            };
            return communicationManager.Get<Response<ContactAddressModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Add Update contact Address
        /// </summary>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        public Response<ContactAddressModel> AddUpdateAddress(List<ContactAddressModel> addressModel)
        {
            const string apiUrl = BaseRoute + "AddUpdateAddress";
            return communicationManager.Post<List<ContactAddressModel>, Response<ContactAddressModel>>(addressModel, apiUrl);
        }


        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="contactAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactAddressModel> DeleteAddress(long contactAddressID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteAddress";
            var requestId = new NameValueCollection
            {
                { "contactAddressID", contactAddressID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<ContactAddressModel>>(requestId, apiUrl);
        }
    }
}
