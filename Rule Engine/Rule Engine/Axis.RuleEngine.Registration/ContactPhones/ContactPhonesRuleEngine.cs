using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhonesRuleEngine : IContactPhonesRuleEngine
    {
        /// <summary>
        /// The contact phones service
        /// </summary>
        private readonly IContactPhonesService contactPhonesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhonesRuleEngine" /> class.
        /// </summary>
        /// <param name="contactPhonesService">The contact phones service.</param>
        public ContactPhonesRuleEngine(IContactPhonesService contactPhonesService)
        {
            this.contactPhonesService = contactPhonesService;
        }

        /// <summary>
        /// Gets the contact phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> GetContactPhones(long contactID, int? contactTypeID)
        {
            return contactPhonesService.GetContactPhones(contactID, contactTypeID);
        }

        /// <summary>
        /// Adds the update contact phones.
        /// </summary>
        /// <param name="contactPhoneModel">The contact phone model.</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> AddUpdateContactPhones(ContactPhoneModel contactPhoneModel)
        {
            return contactPhonesService.AddUpdateContactPhones(contactPhoneModel);
        }

        /// <summary>
        /// Deletes the contact phones.
        /// </summary>
        /// <param name="contactPhoneId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactPhoneModel> DeleteContactPhone(long contactPhoneId, DateTime modifiedOn)
        {
            return contactPhonesService.DeleteContactPhone(contactPhoneId, modifiedOn);
        }
    }
}