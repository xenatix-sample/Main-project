using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Filters;
using Axis.PresentationEngine.Areas.Cache.Repository;
using Axis.Model.Common;
using Axis.Model.Cache;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Cache.Controllers
{
    public class CacheController : BaseController
    {
        private ICacheRepository cacheRepository;

        public CacheController(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }

        public CacheController()
        {

        }

        [HttpGet]
        [Authorization(AllowAnonymous = false)]
        public ActionResult Manifest()
        {
            Response.ContentType = "text/cache-manifest";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(
                System.Web.HttpCacheability.NoCache);

            Response<ManifestModel> response = cacheRepository.GetFilesToCache();
            
            return PartialView("Manifest", response.DataItems);
        }


    }
}