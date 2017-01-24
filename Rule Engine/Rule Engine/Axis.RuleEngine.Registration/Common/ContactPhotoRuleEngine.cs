using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration.Common;

namespace Axis.RuleEngine.Registration.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhotoRuleEngine : IContactPhotoRuleEngine
    {
        /// <summary>
        /// The contact photo service
        /// </summary>
        private readonly IContactPhotoService contactPhotoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhotoRuleEngine"/> class.
        /// </summary>
        /// <param name="contactPhotoService">The contact photo service.</param>
        public ContactPhotoRuleEngine(IContactPhotoService contactPhotoService)
        {
            this.contactPhotoService = contactPhotoService;
        }

        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> GetContactPhoto(long contactID)
        {
            return contactPhotoService.GetContactPhoto(contactID);
        }

        public Response<ContactPhotoModel> GetContactPhotoById(long contactPhotoID)
        {
            return contactPhotoService.GetContactPhotoById(contactPhotoID);
        }

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> GetContactPhotoThumbnails(long contactID)
        {
            return contactPhotoService.GetContactPhotoThumbnails(contactID);
        }

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> AddContactPhoto(ContactPhotoModel contactPhoto)
        {
            return contactPhotoService.AddContactPhoto(contactPhoto);
        }

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        public Response<ContactPhotoModel> UpdateContactPhoto(ContactPhotoModel contactPhoto)
        {
            return contactPhotoService.UpdateContactPhoto(contactPhoto);
        }

        /// <summary>
        /// Deletes the contact photo.
        /// </summary>
        /// <param name="contactPhotoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactPhotoModel> DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn)
        {
            return contactPhotoService.DeleteContactPhoto(contactPhotoID, modifiedOn);
        }
    }
}