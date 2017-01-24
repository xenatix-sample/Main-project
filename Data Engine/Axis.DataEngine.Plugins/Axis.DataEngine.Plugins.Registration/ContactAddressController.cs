using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    public class ContactAddressController : BaseApiController
    {
        IContactAddressDataProvider addressProvider;
        public ContactAddressController(IContactAddressDataProvider addressProvider)
        {
            this.addressProvider = addressProvider;
        }

        /// <summary>
        /// To get contact address list
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAddresses(long contactID, int? contactTypeID)
        {
            return new HttpResult<Response<ContactAddressModel>>(addressProvider.GetAddresses(contactID, contactTypeID), Request);
        }

        /// <summary>
        /// To add/Uppdate contact Address
        /// </summary>
        /// <param name="ContactAddressModel">Contact Address Model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUpdateAddress(List<ContactAddressModel> AddressModel)
        {
            long contactID = 0;
            if (AddressModel.Count > 0)
                contactID = AddressModel.Single().ContactID;
            return new HttpResult<Response<ContactAddressModel>>(addressProvider.UpdateAddresses(contactID, AddressModel), Request);
        }


        /// <summary>
        /// To remove contact Address
        /// </summary>
        /// <param name="contactAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAddress(long contactAddressID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactAddressModel>>(addressProvider.DeleteAddress(contactAddressID, modifiedOn), Request);
        }
    }
}
