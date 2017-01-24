using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Translator;

namespace Axis.Plugins.Registration.Repository.Common
{
    public class ContactAddressRepository : IContactAddressRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "ContactAddress/";

        /// <summary>
        /// constructor
        /// </summary>
        public ContactAddressRepository()
        {

            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">token</param>
        public ContactAddressRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }


        /// <summary>
        /// Get Contact Address
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactTypeId"></param>
        /// <returns></returns>
        public Response<ContactAddressViewModel> GetAddresses(long contactID, int contactTypeId)
        {
            const string apiUrl = baseRoute + "GetAddresses";
            var param = new NameValueCollection { 
                            { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeId", contactTypeId.ToString(CultureInfo.InvariantCulture) } };
            var retVal = communicationManager.Get<Response<ContactAddressModel>>(param, apiUrl);
            return retVal.ToViewModel();
        }

        /// <summary>
        /// Add update address
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="addressModel"></param>
        /// <returns></returns>
        public Response<ContactAddressViewModel> AddUpdateAddress(List<ContactAddressViewModel> addressModel)
        {
            const string apiUrl = baseRoute + "AddUpdateAddress";
            var response = communicationManager.Post<List<ContactAddressModel>, Response<ContactAddressModel>>(addressModel.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="contactAddressID"></param>
        /// <returns></returns>
        public Response<ContactAddressViewModel> DeleteAddress(long contactAddressID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteAddress";
            var requestId = new NameValueCollection { { "contactAddressID", contactAddressID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<ContactAddressModel>>(requestId, apiUrl);
            return response.ToViewModel();

        }
    }
}
