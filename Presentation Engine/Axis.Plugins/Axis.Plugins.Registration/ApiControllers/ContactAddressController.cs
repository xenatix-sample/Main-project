using System;
using Axis.Constant;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers
{
    public class ContactAddressController : BaseApiController
    {
        #region Private Variable

        private readonly IContactAddressRepository contactAddressRepository;

        #endregion

        public ContactAddressController(IContactAddressRepository contactAddressRepository)
        {
            this.contactAddressRepository = contactAddressRepository;
        }

        /// <summary>
        /// Get Address 
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactAddressViewModel> GetAddresses(long contactID, int contactTypeID = (int)ContactType.Contact)
        {
            return contactAddressRepository.GetAddresses(contactID, contactTypeID);
        }

        /// <summary>
        /// Add update address.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public Response<ContactAddressViewModel> AddUpdateAddress(ContactAddressViewModel model)
        {
            var address = new List<ContactAddressViewModel>() { model };
            return contactAddressRepository.AddUpdateAddress(address);
        }

        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="contactAddressID"></param>
        /// <returns>JsonResult</returns>
        [HttpDelete]
        public void DeleteAddress(long contactAddressID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            contactAddressRepository.DeleteAddress(contactAddressID, modifiedOn);
        }
    }
}
