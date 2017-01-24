using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Repository.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContactPhotoRepository
    {
        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactPhotoViewModel> GetContactPhoto(long contactID);

        /// <summary>
        /// Gets the contact photo by identifier.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        Response<ContactPhotoViewModel> GetContactPhotoById(long contactPhotoID);

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ContactPhotoViewModel> GetContactPhotoThumbnails(long contactID);

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        Response<ContactPhotoViewModel> AddContactPhoto(ContactPhotoViewModel contactPhoto);

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        Response<ContactPhotoViewModel> UpdateContactPhoto(ContactPhotoViewModel contactPhoto);

        /// <summary>
        /// Deletes the contact photo.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        Response<ContactPhotoViewModel> DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn);
    }
}