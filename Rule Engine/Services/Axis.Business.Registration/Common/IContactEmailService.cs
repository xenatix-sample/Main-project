using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Collections.Generic;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Interface of Email Service class to call the web api methods
    /// </summary>
    public interface IContactEmailService
    {
        /// <summary>
        /// Get emails
        /// </summary>
        /// <param name="contactID">Contact identifier</param>
        /// <param name="contactTypeID">Contact Type</param>
        /// <returns></returns>
        Response<ContactEmailModel> GetEmails(long contactID, int contactTypeID);
      

        /// <summary>
        /// Add/Update Eamils
        /// </summary>
        /// <param name="contactID">Contact identifier</param>
        /// <param name="contact">Emails list</param>
        /// <returns></returns>
        Response<ContactEmailModel> AddUpdateEmails(List<ContactEmailModel> contact);

        /// <summary>
        /// Deletes Emails.
        /// </summary>
        /// <param name="contactEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ContactEmailModel> DeleteEmail(long contactEmailID, DateTime modifiedOn);
    }
}
