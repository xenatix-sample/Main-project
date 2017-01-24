using System;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Plugins.Registration.Model;
using Axis.Constant;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Controller for Email screen
    /// </summary>
    public class ContactEmailController : BaseApiController
    {
        private readonly IContactEmailRepository emailRepository;

        public ContactEmailController(IContactEmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        /// <summary>
        /// To get emails
        /// </summary>
        /// <param name="contactID">Contact Id of patient</param>      
        /// <returns></returns>
        [HttpGet]
        public Response<ContactEmailViewModel> GetEmails(int contactID, int contactTypeID = (int)ContactType.Contact)
        {
            return emailRepository.GetEmails(contactID, contactTypeID);
        }


        /// <summary>
        /// Add/Update Email for patient
        /// </summary>
        /// <param name="Email">Email ViewModel</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactEmailViewModel> AddUpdateEmail(ContactEmailViewModel contactEmailModal)
        {
            var emails = new List<ContactEmailViewModel> { contactEmailModal };
            return emailRepository.AddUpdateEmail(emails);
        }

        /// <summary>
        /// Delete Email for patient
        /// </summary>
        /// <param name="Id">Contact Id of Email</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactEmailViewModel> DeleteEmail(long contactEmailID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return emailRepository.DeleteEmail(contactEmailID, modifiedOn);
        }
    }
}
