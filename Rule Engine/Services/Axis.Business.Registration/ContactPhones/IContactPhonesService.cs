using System;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Registration;
using System.Collections.Generic;

namespace Axis.Service.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContactPhonesService
    {
        /// <summary>
        /// Gets the contact phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        Response<ContactPhoneModel> GetContactPhones(long contactID, int? contactTypeID);
        /// <summary>
        /// Adds the update contact phones.
        /// </summary>
        /// <param name="contactPhoneModel">The contact phone model.</param>
        /// <returns></returns>
        Response<ContactPhoneModel> AddUpdateContactPhones(ContactPhoneModel contactPhoneModel);

        /// <summary>
        /// Deletes the contact phones.
        /// </summary>
        /// <param name="contactPhoneId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ContactPhoneModel> DeleteContactPhone(long contactPhoneId, DateTime modifiedOn);
    }
}