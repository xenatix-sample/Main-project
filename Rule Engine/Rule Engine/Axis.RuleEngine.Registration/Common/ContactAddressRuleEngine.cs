using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration.Common
{
    public class ContactAddressRuleEngine : IContactAddressRuleEngine
    {
        private readonly IContactAddressService addressService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="addressService"></param>
        public ContactAddressRuleEngine(IContactAddressService addressService)
        {
            this.addressService = addressService;
        }

        /// <summary>
        /// Get Contact Address
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactTypeId"></param>
        /// <returns></returns>
        public Response<ContactAddressModel> GetAddresses(long contactID, int contactTypeId)
        {
            return addressService.GetAddresses(contactID, contactTypeId);
        }

        /// <summary>
        /// Add Update Contact Address
        /// </summary>
        /// <param name="contactAddress"></param>
        /// <returns></returns>
        public Response<ContactAddressModel> AddUpdateAddress(List<ContactAddressModel> contactAddress)
        {
            return addressService.AddUpdateAddress(contactAddress);
        }

        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="contactAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactAddressModel> DeleteAddress(long contactAddressID, DateTime modifiedOn)
        {
            return addressService.DeleteAddress(contactAddressID, modifiedOn);
        }
    }
}
