using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Web.Http;
using System.Collections.Generic;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class ContactPhonesController : BaseApiController
    {
        /// <summary>
        /// The contact phone data provider
        /// </summary>
        private readonly IContactPhoneDataProvider contactPhoneDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhonesController" /> class.
        /// </summary>
        /// <param name="contactPhoneDataProvider">The contact phone data provider.</param>
        public ContactPhonesController(IContactPhoneDataProvider contactPhoneDataProvider)
        {
            this.contactPhoneDataProvider = contactPhoneDataProvider;
        }

        /// <summary>
        /// Gets the contact phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactPhones(long contactID, int? contactTypeID)
        {
            return new HttpResult<Response<ContactPhoneModel>>(contactPhoneDataProvider.GetPhones(contactID, contactTypeID), Request);
        }

        /// <summary>
        /// Adds the update contact phones.
        /// </summary>
        /// <param name="contactPhoneModel">The contact phone model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUpdateContactPhones(ContactPhoneModel contactPhoneModel)
        {
            HttpResult<Response<ContactPhoneModel>> result = null;
            List<ContactPhoneModel> contactPhoneModelList = new List<ContactPhoneModel>() { contactPhoneModel };
            if (contactPhoneModel.ContactPhoneID == 0)
            {
                result = new HttpResult<Response<ContactPhoneModel>>(contactPhoneDataProvider.AddPhones(contactPhoneModel.ContactID, contactPhoneModelList), Request);
            }
            else
            {
                result = new HttpResult<Response<ContactPhoneModel>>(contactPhoneDataProvider.UpdatePhones(contactPhoneModel.ContactID, contactPhoneModelList), Request);
            }

            return result;
        }

        /// <summary>
        /// Deletes the contact phones.
        /// </summary>
        /// <param name="contactPhoneId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteContactPhone(long contactPhoneId, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactPhoneModel>>(contactPhoneDataProvider.DeleteContactPhone(contactPhoneId, modifiedOn), Request);
        }
    }
}