using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Collections.Generic;

namespace Axis.DataProvider.Registration.Common
{
    public interface  IContactEmailDataProvider
    {
       /// <summary>
       /// Get emails
       /// </summary>
       /// <param name="contactID">Contact identifier</param>
       /// <param name="contactTypeID">Contact Type</param>
       /// <returns></returns>
        Response<ContactEmailModel> GetEmails(long contactID, int? contactTypeID);

        /// <summary>
        /// Add Emails
        /// </summary>
        /// <param name="contactID">Contact identifier</param>
        /// <param name="contact">Emails</param>
        /// <returns></returns>
        Response<ContactEmailModel> AddEmails(long contactID, List<ContactEmailModel> contact);

        /// <summary>
        /// Update Eamils
        /// </summary>
        /// <param name="contactID">Contact identifier</param>
        /// <param name="contact">Emails</param>
        /// <returns></returns>
        Response<ContactEmailModel> UpdateEmails(long contactID, List<ContactEmailModel> contact);

        /// <summary>
        /// Deletes Emails.
        /// </summary>
        /// <param name="contactEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ContactEmailModel> DeleteEmail(long contactEmailID, DateTime modifiedOn);

    }
}
