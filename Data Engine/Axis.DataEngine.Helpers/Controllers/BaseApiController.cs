using Axis.Helpers.Infrastructure;
using Axis.Logging;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Axis.DataEngine.Helpers.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
        protected Logger _logger = new Logger();

        public BaseApiController()
        {
        }
    }
}
