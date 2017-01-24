using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.RuleEngine.Registration.Common
{
    public interface IContactPhotoRuleEngine
    {
        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactPhotoModel> GetContactPhoto(long contactID);

        /// <summary>
        /// Gets the contact photo by identifier.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        Response<ContactPhotoModel> GetContactPhotoById(long contactPhotoID);

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactPhotoModel> GetContactPhotoThumbnails(long contactID);

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        Response<ContactPhotoModel> AddContactPhoto(ContactPhotoModel contactPhoto);

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        Response<ContactPhotoModel> UpdateContactPhoto(ContactPhotoModel contactPhoto);

        /// <summary>
        /// Deletes the contact photo.
        /// </summary>
        /// <param name="contactPhotoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ContactPhotoModel> DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn);
    }
}