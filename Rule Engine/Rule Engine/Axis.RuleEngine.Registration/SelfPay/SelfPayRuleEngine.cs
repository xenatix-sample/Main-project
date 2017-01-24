using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;

namespace Axis.RuleEngine.Registration
{
    public class SelfPayRuleEngine : ISelfPayRuleEngine
    {
              /// <summary>
        /// The self pay service
        /// </summary>
        private readonly ISelfPayService selfPayService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfPayRuleEngine" /> class.
        /// </summary>
        /// <param name="selfPayService">The self pay service.</param>
        public SelfPayRuleEngine(ISelfPayService selfPayService)
        {
            this.selfPayService = selfPayService;
        }

        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <returns></returns>
        public Response<SelfPayModel> GetSelfPayDetails(long selfPayID)
        {
            return selfPayService.GetSelfPayDetails(selfPayID);
        }

        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> AddSelfPay(SelfPayModel selfPay)
        {
            return selfPayService.AddSelfPay(selfPay);
        }

        /// <summary>
        /// Adds the self pay header
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> AddSelfPayHeader(SelfPayModel selfPay)
        {
            return selfPayService.AddSelfPayHeader(selfPay);
        }

        /// <summary>
        /// Updates the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> UpdateSelfPay(SelfPayModel selfPay)
        {
            return selfPayService.UpdateSelfPay(selfPay);
        }


        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        public Response<SelfPayModel> DeleteSelfPay(long selfPayID, DateTime modifiedOn)
        {
            return selfPayService.DeleteSelfPay(selfPayID, modifiedOn);
        }
    }
}
