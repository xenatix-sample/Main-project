using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataProvider.Registration.Common;
using System.Collections.Generic;

namespace Axis.DataEngine.Plugins.Registration
{

    /// <summary>
    /// 
    /// </summary>
    public class ContactEmailController : BaseApiController
    {
        /// <summary>
        /// The _email data provider
        /// </summary>
        readonly IContactEmailDataProvider _emailDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationController"/> class.
        /// </summary>
        /// <param name="emailDataProvider">The email data provider.</param>
        public ContactEmailController(IContactEmailDataProvider emailDataProvider)
        {
            _emailDataProvider = emailDataProvider;
        }

        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetEmails(long contactID, int? contactTypeID)
        {
            return new HttpResult<Response<ContactEmailModel>>(_emailDataProvider.GetEmails(contactID, contactTypeID), Request);
        }
                

        /// <summary>
        /// add/update email
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
         [HttpPost]
        public IHttpActionResult AddUpdateEmails(List<ContactEmailModel> contactEmails)
        {
            
            long contactID = 0;
            if (contactEmails.Count > 0)
                contactID = contactEmails[0].ContactID;
            return new HttpResult<Response<ContactEmailModel>>(_emailDataProvider.UpdateEmails(contactID, contactEmails), Request);
        }

        /// <summary>
         /// remove email
        /// </summary>
        /// <param name="contactEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
         [HttpDelete]
         public IHttpActionResult DeleteEmail(long contactEmailID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactEmailModel>>(_emailDataProvider.DeleteEmail(contactEmailID, modifiedOn), Request);
        }

    }
}
