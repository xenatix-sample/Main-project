using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Common;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhotoController : BaseApiController
    {
        /// <summary>
        /// The contact photo data provider
        /// </summary>
        private IContactPhotoDataProvider contactPhotoDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressController"/> class.
        /// </summary>
        /// <param name="contactPhotoDataProvider">The contact photo data provider.</param>
        public ContactPhotoController(IContactPhotoDataProvider contactPhotoDataProvider)
        {
            this.contactPhotoDataProvider = contactPhotoDataProvider;
        }

        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactPhoto(long contactID)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoDataProvider.GetContactPhoto(contactID), Request);
        }

        /// <summary>
        /// Gets the contact photo by identifier.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactPhotoById(long contactPhotoID)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoDataProvider.GetContactPhotoById(contactPhotoID), Request);
        }

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactPhotoThumbnails(long contactID)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoDataProvider.GetContactPhotoThumbnails(contactID), Request);
        }

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddContactPhoto(ContactPhotoModel contactPhoto)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoDataProvider.AddContactPhoto(contactPhoto), Request);
        }

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateContactPhoto(ContactPhotoModel contactPhoto)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoDataProvider.UpdateContactPhoto(contactPhoto), Request);
        }

        /// <summary>
        /// Deletes the contact photo.
        /// </summary>
        /// <param name="contactPhotoID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoDataProvider.DeleteContactPhoto(contactPhotoID, modifiedOn), Request);
        }
    }
}