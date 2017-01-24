using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    ///  Rule engine class for calling service method of Email Service
    /// </summary>
    public class ContactEmailRuleEngine : IContactEmailRuleEngine
    {
        private readonly IContactEmailService contactEmailService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="EmailService"></param>
        public ContactEmailRuleEngine(IContactEmailService contactEmailService)
        {
            this.contactEmailService = contactEmailService;
        }

        /// <summary>
        /// To get list of Emails for contact
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactTypeId"></param>
        /// <returns></returns>
        public Response<ContactEmailModel> GetEmails(long contactID, int contactTypeID)
        {
            return contactEmailService.GetEmails(contactID,contactTypeID);
        }
        

        /// <summary>
        /// To update Email for contact
        /// </summary>
        /// <param name="ContactEmailModel"></param>
        /// <returns></returns>
        public Response<ContactEmailModel> AddUpdateEmails(List<ContactEmailModel> contactEmailModel)
        {
            return contactEmailService.AddUpdateEmails(contactEmailModel);
        }

        /// <summary>
        /// To remove Email for contact
        /// </summary>
        /// <param name="contactEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactEmailModel> DeleteEmail(long contactEmailID, DateTime modifiedOn)
        {
            return contactEmailService.DeleteEmail(contactEmailID, modifiedOn);
        }
    }
}
