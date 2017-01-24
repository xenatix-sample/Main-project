using System;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhotoController : BaseApiController
    {
        #region Private Variable

        /// <summary>
        /// The contact photo repository
        /// </summary>
        private readonly IContactPhotoRepository _contactPhotoRepository;

        #endregion Private Variable

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhotoController"/> class.
        /// </summary>
        /// <param name="contactPhotoRepository">The contact photo repository.</param>
        public ContactPhotoController(IContactPhotoRepository contactPhotoRepository)
        {
            this._contactPhotoRepository = contactPhotoRepository;
        }

        #region Data API

        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactPhotoViewModel> GetContactPhoto(long contactID)
        {
            return _contactPhotoRepository.GetContactPhoto(contactID);
        }

        /// <summary>
        /// Gets the contact photo by identifier.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactPhotoViewModel> GetContactPhotoById(long contactPhotoID)
        {
            return _contactPhotoRepository.GetContactPhotoById(contactPhotoID);
        }

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ContactPhotoViewModel> GetContactPhotoThumbnails(long contactID)
        {
            return _contactPhotoRepository.GetContactPhotoThumbnails(contactID);
        }

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ContactPhotoViewModel> AddContactPhoto(ContactPhotoViewModel contactPhoto)
        {
            return _contactPhotoRepository.AddContactPhoto(contactPhoto);
        }

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ContactPhotoViewModel> UpdateContactPhoto(ContactPhotoViewModel contactPhoto)
        {
            return _contactPhotoRepository.UpdateContactPhoto(contactPhoto);
        }

        /// <summary>
        /// Deletes the contact photo.
        /// </summary>
        /// <param name="contactPhotoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ContactPhotoViewModel> DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _contactPhotoRepository.DeleteContactPhoto(contactPhotoID, modifiedOn);
        }

        #endregion Action Method
    }
}