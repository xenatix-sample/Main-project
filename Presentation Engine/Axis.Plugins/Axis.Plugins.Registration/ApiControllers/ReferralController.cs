using System;
using System.Web.Http;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Models;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Controller for referral
    /// </summary>
    public class ReferralController : BaseApiController
    {
        private readonly IReferralRepository referralRepository;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="referralRepository"></param>
        public ReferralController(IReferralRepository referralRepository)
        {
            this.referralRepository = referralRepository;
        }

        /// <summary>
        /// Get referrals
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<Axis.Model.Registration.ReferralModel>> GetReferrals(int contactId)
        {
            var result = await referralRepository.GetReferralsAsync(contactId);
            return result;
        }

        /// <summary>
        /// Add referral
        /// </summary>
        /// <param name="referral">Referral ViewModel</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralViewModel> AddReferral(ReferralViewModel referral)
        {
            return referralRepository.AddReferral(referral);
        }

        /// <summary>
        /// Update referral
        /// </summary>
        /// <param name="referral">Referral ViewModel</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralViewModel> UpdateReferral(ReferralViewModel referral)
        {
            return referralRepository.UpdateReferral(referral);
        }

        /// <summary>
        /// Delete referral
        /// </summary>
        /// <param name="id">Referral Id</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ReferralViewModel> DeleteReferral(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return referralRepository.DeleteReferral(id, modifiedOn);
        }

        /// <summary>
        /// Update referral contact
        /// </summary>
        /// <param name="referralContact">Referral Contact ViewModel</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralContactViewModel> UpdateReferralContact(ReferralContactViewModel referralContact)
        {
            return referralRepository.UpdateReferralContact(referralContact);
        }

        /// <summary>
        /// Delete referral contact
        /// </summary>
        /// <param name="referralContactId">Referral Contact Id</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ReferralContactViewModel> DeleteReferalContact(long referralContactId, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return referralRepository.DeleteReferalContact(referralContactId, modifiedOn);
        }
    }
}