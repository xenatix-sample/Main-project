
using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Repository
{

    /// <summary>
    /// 
    /// </summary>
    public interface IContactPhoneRepository
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
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Response<ContactPhoneModel> DeleteContactPhones(long contactPhoneId, DateTime modifiedOn);
    }
}
