using System;
using Axis.Plugins.Registration.Models.Referrals.Common;
using Axis.Plugins.Registration.Repository.Referrals.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals.Common
{
    public class ReferralPhoneController : BaseApiController
    {
        /// <summary>
        /// The referral phone repository
        /// </summary>
        private readonly IReferralPhoneRepository referralPhoneRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhoneController"/> class.
        /// </summary>
        /// <param name="referralPhoneRepository">The referral phone repository.</param>
        public ReferralPhoneController(IReferralPhoneRepository referralPhoneRepository)
        {
            this.referralPhoneRepository = referralPhoneRepository;
        }

        /// <summary>
        /// Get Phone
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralPhoneViewModel> GetPhones(long referralID)
        {
            int contactTypeID = 1; // TODO :- Remove contactTypeID from all layers 
            return referralPhoneRepository.GetPhones(referralID, contactTypeID);
        }

        /// <summary>
        /// Add update Phone.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JsonResult
        /// </returns>
        [HttpPost]
        public Response<ReferralPhoneViewModel> AddUpdatePhones(ReferralPhoneViewModel model)
        {
            List<ReferralPhoneViewModel> viewModel = new List<ReferralPhoneViewModel>();
            viewModel.Add(model);
            return referralPhoneRepository.AddUpdatePhones(viewModel);
        }

        /// <summary>
        /// Delete Phone
        /// </summary>
        /// <param name="referralPhoneID">The referral phone identifier.</param>
        /// <returns>
        /// JsonResult
        /// </returns>
        [HttpDelete]
        public string DeleteReferralPhone(long referralPhoneID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            referralPhoneRepository.DeleteReferralPhone(referralPhoneID, modifiedOn);
            return string.Empty;
        }
    }
}