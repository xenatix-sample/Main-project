using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Common;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class ContactAliasController : BaseApiController
    {
        /// <summary>
        /// The contact alias data provider
        /// </summary>
        private IContactAliasDataProvider contactAliasDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAliasController" /> class.
        /// </summary>
        /// <param name="contactAliasDataProvider">The address provider.</param>
        public ContactAliasController(IContactAliasDataProvider contactAliasDataProvider)
        {
            this.contactAliasDataProvider = contactAliasDataProvider;
        }

        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactAlias(long contactID)
        {
            return new HttpResult<Response<ContactAliasModel>>(contactAliasDataProvider.GetContactAlias(contactID), Request);
        }

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddContactAlias(List<ContactAliasModel> contactAlias)
        {
            return new HttpResult<Response<ContactAliasModel>>(contactAliasDataProvider.AddContactAlias(contactAlias), Request);
        }

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateContactAlias(List<ContactAliasModel> contactAlias)
        {
            return new HttpResult<Response<ContactAliasModel>>(contactAliasDataProvider.UpdateContactAlias(contactAlias), Request);
        }

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteContactAlias(long contactAliasID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactAliasModel>>(contactAliasDataProvider.DeleteContactAlias(contactAliasID, modifiedOn), Request);
        }
    }
}