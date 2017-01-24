using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    public class SelfPayController : BaseApiController
    {
        /// <summary>
        /// The selfPay data provider
        /// </summary>
        readonly ISelfPayDataProvider _selfPayDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfPayController"/> class.
        /// </summary>
        /// <param name="selfPayDataProvider">The self pay data provider.</param>
        public SelfPayController(ISelfPayDataProvider selfPayDataProvider)
        {
            _selfPayDataProvider = selfPayDataProvider;
        }

        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetSelfPayDetails(long selfPayID)
        {
            return new HttpResult<Response<SelfPayModel>>(_selfPayDataProvider.GetSelfPayDetails(selfPayID), Request);
        }


        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public IHttpActionResult AddSelfPay(SelfPayModel selfPay)
        {
            return new HttpResult<Response<SelfPayModel>>(_selfPayDataProvider.AddSelfPay(selfPay), Request);
        }

        /// <summary>
        /// Adds the self pay header
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public IHttpActionResult AddSelfPayHeader(SelfPayModel selfPay)
        {
            return new HttpResult<Response<SelfPayModel>>(_selfPayDataProvider.AddSelfPayHeader(selfPay), Request);
        }

        /// <summary>
        /// Updates the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateSelfPay(SelfPayModel selfPay)
        {
            return new HttpResult<Response<SelfPayModel>>(_selfPayDataProvider.UpdateSelfPay(selfPay), Request);
        }


        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        public IHttpActionResult DeleteSelfPay(long selfPayID, DateTime modifiedOn)
        {
            return new HttpResult<Response<SelfPayModel>>(_selfPayDataProvider.DeleteSelfPay(selfPayID, modifiedOn), Request);
        }
    }
}
