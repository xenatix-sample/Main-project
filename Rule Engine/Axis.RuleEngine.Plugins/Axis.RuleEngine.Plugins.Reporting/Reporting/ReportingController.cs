using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Service.Controllers
{
    public class ReportingController : ApiController
    {
        public IHttpActionResult GetReportsByType(string reportTypeName)
        {
            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(new Response<Dictionary<string, List<dynamic>>>(), new HttpRequestMessage());
        }

    }
}
