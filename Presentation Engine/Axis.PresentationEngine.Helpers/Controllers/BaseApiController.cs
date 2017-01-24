using System.Web.Http;
using Axis.Helpers.Caching;
using Axis.PresentationEngine.Helpers.Filters;
using Axis.Model.Common;
using Axis.Helpers.Infrastructure;

namespace Axis.PresentationEngine.Helpers.Controllers
{
    [HttpAuthorization]
    public class BaseApiController : ApiController
    {
        private const string keyPattern = "providers/";
        public void ClearCache<T>(Response<T> request)
        {
            ICacheManager cacheManager = EngineContext.Current.Resolve<ICacheManager>();

            if (request.ResultCode == 0 || request == null)
            {
                cacheManager.RemoveByPattern(keyPattern);
            }
        }
    }
}
