using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals.Common;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals.Common
{
    public class ReferralAddressController : BaseApiController
    {
        /// <summary>
        /// The address provider
        /// </summary>
        IReferralAddressDataProvider addressProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressController"/> class.
        /// </summary>
        /// <param name="addressProvider">The address provider.</param>
        public ReferralAddressController(IReferralAddressDataProvider addressProvider)
        {
            this.addressProvider = addressProvider;
        }


        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAddresses(long referralID, int? contactTypeID)
        {
            return new HttpResult<Response<ReferralAddressModel>>(addressProvider.GetAddresses(referralID, contactTypeID), Request);
        }


        /// <summary>
        /// Adds the update address.
        /// </summary>
        /// <param name="addressModel">The address model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUpdateAddresses(List<ReferralAddressModel> addressModel)
        {        
            return new HttpResult<Response<ReferralAddressModel>>(addressProvider.AddUpdateAddresses(addressModel), Request);
        }



        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="referralAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAddress(long referralAddressID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralAddressModel>>(addressProvider.DeleteAddress(referralAddressID, modifiedOn), Request);
        }
    }
}
