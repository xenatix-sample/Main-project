using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    public class SelfPayController : BaseApiController
    {
        #region Private Variable

        private readonly ISelfPayRepository selfPayRepository;

        #endregion

        public SelfPayController(ISelfPayRepository selfPayRepository)
        {
            this.selfPayRepository = selfPayRepository;
        }

        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayHeaderID">The self pay identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<SelfPayViewModel> GetSelfPayDetails(long selfPayID)
        {
            return selfPayRepository.GetSelfPayDetails(selfPayID);
        }


        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<SelfPayViewModel> AddSelfPay(SelfPayViewModel selfPay)
        {
            //selfPay.EffectiveDate = selfPay.EffectiveDate.Value.ToUniversalTime();
            //selfPay.ExpirationDate = selfPay.ExpirationDate.Value.ToUniversalTime();
            return selfPayRepository.AddSelfPay(selfPay);
        }

        /// <summary>
        /// Adds the self pay header
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<SelfPayViewModel> AddSelfPayHeader(SelfPayViewModel selfPay)
        {
            return selfPayRepository.AddSelfPayHeader(selfPay);
        }
        /// <summary>
        /// Updates the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<SelfPayViewModel> UpdateSelfPay(SelfPayViewModel selfPay)
        {
            return selfPayRepository.UpdateSelfPay(selfPay);
        }

        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<SelfPayViewModel> DeleteSelfPay(long selfPayID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return selfPayRepository.DeleteSelfPay(selfPayID, modifiedOn);
        }
    }
}
