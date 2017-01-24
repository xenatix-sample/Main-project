using System;
using Axis.Plugins.Registration.Models.Referrals.Common;
using Axis.Plugins.Registration.Repository.Referrals.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals.Common
{
    public class ReferralEmailController : BaseApiController
    {
        /// <summary>
        /// The referral email repository
        /// </summary>
        private readonly IReferralEmailRepository referralEmailRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailController"/> class.
        /// </summary>
        /// <param name="referralEmailRepository">The referral email repository.</param>
        public ReferralEmailController(IReferralEmailRepository referralEmailRepository)
        {
            this.referralEmailRepository = referralEmailRepository;
        }

        /// <summary>
        /// Get Email
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralEmailViewModel> GetEmails(long referralID)
        {
            int contactTypeID = 1; // TODO :- Remove contactTypeID from all layers 
            return referralEmailRepository.GetEmails(referralID, contactTypeID);
        }

        /// <summary>
        /// Add update Email.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JsonResult
        /// </returns>
        [HttpPost]
        public Response<ReferralEmailViewModel> AddUpdateEmails(ReferralEmailViewModel model)
        {
            List<ReferralEmailViewModel> viewModel = new List<ReferralEmailViewModel>();
            viewModel.Add(model);
            return referralEmailRepository.AddUpdateEmails(viewModel);
        }

        /// <summary>
        /// Delete Email
        /// </summary>
        /// <param name="referralEmailID">The referral email identifier.</param>
        /// <returns>
        /// JsonResult
        /// </returns>
        [HttpDelete]
        public string DeleteEmail(long referralEmailID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            referralEmailRepository.DeleteEmail(referralEmailID, modifiedOn);
            return string.Empty;
        }
    }
}
