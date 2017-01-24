using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Common;
using System.Web.Http;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>

    public class ContactPhotoController : BaseApiController
    {
        /// <summary>
        /// The contact photo rule engine
        /// </summary>
        private readonly IContactPhotoRuleEngine contactPhotoRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhotoController"/> class.
        /// </summary>
        /// <param name="contactPhotoRuleEngine">The contact photo rule engine.</param>
        public ContactPhotoController(IContactPhotoRuleEngine contactPhotoRuleEngine)
        {
            this.contactPhotoRuleEngine = contactPhotoRuleEngine;
        }

        /// <summary>
        /// Gets the contact photo.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.SiteAdministration, Module.Registration, Module.ECI, Module.CrisisLine, Module.General, Module.Reports, Module.Benefits, Module.Consents, Module.Intake, Module.LawLiaison }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetContactPhoto(long contactID)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoRuleEngine.GetContactPhoto(contactID), Request);
        }

        /// <summary>
        /// Gets the contact photo by identifier.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetContactPhotoById(long contactPhotoID)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoRuleEngine.GetContactPhotoById(contactPhotoID), Request);
        }

        /// <summary>
        /// Gets the contact photo thumbnails.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetContactPhotoThumbnails(long contactID)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoRuleEngine.GetContactPhotoThumbnails(contactID), Request);
        }

        /// <summary>
        /// Adds the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddContactPhoto(ContactPhotoModel contactPhoto)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoRuleEngine.AddContactPhoto(contactPhoto), Request);
        }

        /// <summary>
        /// Updates the contact photo.
        /// </summary>
        /// <param name="contactPhoto">The contact photo.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateContactPhoto(ContactPhotoModel contactPhoto)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoRuleEngine.UpdateContactPhoto(contactPhoto), Request);
        }

        /// <summary>
        /// Deletes the contact photo.
        /// </summary>
        /// <param name="contactPhotoID">The contact photo identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteContactPhoto(long contactPhotoID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactPhotoModel>>(contactPhotoRuleEngine.DeleteContactPhoto(contactPhotoID, modifiedOn), Request);
        }
    }
}