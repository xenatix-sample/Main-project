using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Account;
using Axis.Model.Account;
using Axis.Model.Common;
using System.Net;
using System.Web.Http;
using Axis.DataProvider.RecordedServices.VoidServices;
using Axis.Model.RecordedServices;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class VoidServiceController : BaseApiController
    {
        /// <summary>
        /// initialise voidServiceDataProvider
        /// </summary>
        IVoidServiceDataProvider voidServiceDataProvider = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="voidServiceDataProvider"></param>
        public VoidServiceController(IVoidServiceDataProvider voidServiceDataProvider)
        {
            this.voidServiceDataProvider = voidServiceDataProvider;
        }

        /// <summary>
        /// To set service as void
        /// </summary>
        /// <param name="voidService">voidService</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VoidRecordedService(VoidServiceModel voidService)
        {
            var voidServiceResult = voidServiceDataProvider.VoidService(voidService);

            return new HttpResult<Response<VoidServiceModel>>(voidServiceResult, Request);
        }

        /// <summary>
        /// To set call center service as void
        /// </summary>
        /// <param name="voidService">voidService</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VoidServiceCallCenter(VoidServiceModel voidService)
        {
            var voidServiceResult = voidServiceDataProvider.VoidServiceCallCenter(voidService);

            return new HttpResult<Response<VoidServiceModel>>(voidServiceResult, Request);
        }

        /// <summary>
        /// To Get void service
        /// </summary>
        /// <param name="voidService">voidService</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetVoidService(int serviceRecordingID)
        {
            var voidServiceResult = voidServiceDataProvider.GetVoidService(serviceRecordingID);

            return new HttpResult<Response<VoidServiceModel>>(voidServiceResult, Request);
        }
    }
}
