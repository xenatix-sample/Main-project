using System;
using Axis.Plugins.Registration.Models.Referrals.Common;
using Axis.Plugins.Registration.Repository.Referrals.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals.Common
{
    public class ReferralAddressController : BaseApiController
    {
        /// <summary>
        /// The referral address repository
        /// </summary>
        private readonly IReferralAddressRepository referralAddressRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressController"/> class.
        /// </summary>
        /// <param name="referralAddressRepository">The referral address repository.</param>
        public ReferralAddressController(IReferralAddressRepository referralAddressRepository)
        {
            this.referralAddressRepository = referralAddressRepository;
        }

        /// <summary>
        /// Get Address
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralAddressViewModel> GetAddresses(long referralID)
        {
            int contactTypeID = 1;// TODO :- Remove contactTypeID from all layers
            return referralAddressRepository.GetAddresses(referralID, contactTypeID);
        }

        /// <summary>
        /// Add update address.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JsonResult
        /// </returns>
        [HttpPost]
        public Response<ReferralAddressViewModel> AddUpdateAddresses(ReferralAddressViewModel model)
        {
            List<ReferralAddressViewModel> viewModel = new List<ReferralAddressViewModel>();
            viewModel.Add(model);
            return referralAddressRepository.AddUpdateAddresses(viewModel);
        }

        /// <summary>
        /// Delete Address
        /// </summary>
        /// <param name="referralAddressID">The referral address identifier.</param>
        /// <returns>
        /// JsonResult
        /// </returns>
        [HttpDelete]
        public string DeleteAddress(long referralAddressID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            referralAddressRepository.DeleteAddress(referralAddressID, modifiedOn);
            return string.Empty;
        }
    }
}