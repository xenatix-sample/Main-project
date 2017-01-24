using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Controllers
{
    public class TilesController : BaseController
    {
        // GET: Tile
        public ActionResult Index()
        {
            return View("../Shared/_TileInfo");
        }

    }
}