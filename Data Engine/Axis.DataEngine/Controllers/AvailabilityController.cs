using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.Model.Common;

namespace Axis.DataEngine.Service.Controllers
{
    public class AvailabilityController : BaseApiController
    {
        [HttpGet]
        public IHttpActionResult GetAvailability()
        {
            var availability = new AvailabilityModel() { IsAvailable = true, Message = "Data engine is available"};
            return new HttpResult<AvailabilityModel>(availability, Request);
        }
    }
}