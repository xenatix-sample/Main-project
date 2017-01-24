using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Manifest;
using Axis.Model.Cache;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{

    public class ManifestController : BaseApiController
    {
        IManifestDataProvider manifestDataProvider = null;

        public ManifestController(IManifestDataProvider manifestDataProvider)
        {
            this.manifestDataProvider = manifestDataProvider;
        }

        public IHttpActionResult GetFilesToCache()
        {
            return new HttpResult<Response<ManifestModel>>(manifestDataProvider.GetFilesToCache(), Request);
        }
    }
}