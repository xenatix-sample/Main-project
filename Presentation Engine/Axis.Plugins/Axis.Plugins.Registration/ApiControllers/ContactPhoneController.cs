using System;
using System.Web.Http;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhoneController : BaseApiController
    {
        /// <summary>
        /// The contact phones repository
        /// </summary>
        private readonly IContactPhoneRepository contactPhonesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhoneController" /> class.
        /// </summary>
        /// <param name="contactPhonesRepository">The contact phones repository.</param>
        public ContactPhoneController(IContactPhoneRepository contactPhonesRepository)
        {
            this.contactPhonesRepository = contactPhonesRepository;
        }

        /// <summary>
        /// Gets the contacts phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactPhoneModel> GetContactPhones(long contactID, int? contactTypeID)
        {
            return contactPhonesRepository.GetContactPhones(contactID, contactTypeID);
        }

        /// <summary>
        /// Adds the update contact phone.
        /// </summary>
        /// <param name="contactPhoneModel">The contact phone model.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactPhoneModel> AddUpdateContactPhone(ContactPhoneModel contactPhoneModel)
        {
            return contactPhonesRepository.AddUpdateContactPhones(contactPhoneModel);
        }

        /// <summary>
        /// Deletes the contact phone.
        /// </summary>
        /// <param name="contactPhoneModel">The contact phone model.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactPhoneModel> DeleteContactPhone(long contactPhoneID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return contactPhonesRepository.DeleteContactPhones(contactPhoneID, modifiedOn);
        }
    }
}