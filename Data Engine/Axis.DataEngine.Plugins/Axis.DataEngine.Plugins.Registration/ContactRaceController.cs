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
    public class ContactRaceController : BaseApiController
    {
        /// <summary>
        /// The contact race data provider
        /// </summary>
        private IContactRaceDataProvider ContactRaceDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRaceController" /> class.
        /// </summary>
        /// <param name="ContactRaceDataProvider">The address provider.</param>
        public ContactRaceController(IContactRaceDataProvider ContactRaceDataProvider)
        {
            this.ContactRaceDataProvider = ContactRaceDataProvider;
        }

        /// <summary>
        /// Gets the contact race.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactRace(long contactID)
        {
            return new HttpResult<Response<ContactRaceModel>>(ContactRaceDataProvider.GetContactRace(contactID), Request);
        }

        /// <summary>
        /// Adds the contact race.
        /// </summary>
        /// <param name="ContactRace">The contact race.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddContactRace(List<ContactRaceModel> ContactRace)
        {
            return new HttpResult<Response<ContactRaceModel>>(ContactRaceDataProvider.AddContactRace(ContactRace), Request);
        }

        /// <summary>
        /// Updates the contact race.
        /// </summary>
        /// <param name="ContactRace">The contact race.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateContactRace(List<ContactRaceModel> ContactRace)
        {
            return new HttpResult<Response<ContactRaceModel>>(ContactRaceDataProvider.UpdateContactRace(ContactRace), Request);
        }

        /// <summary>
        /// Deletes the contact race.
        /// </summary>
        /// <param name="ContactRaceID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteContactRace(long ContactRaceID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactRaceModel>>(ContactRaceDataProvider.DeleteContactRace(ContactRaceID, modifiedOn), Request);
        }
    }
}