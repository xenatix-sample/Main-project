using Axis.PresentationEngine.Areas.Configuration.Models;
using Axis.PresentationEngine.Areas.Configuration.Repository;
using Axis.PresentationEngine.Areas.Configuration.Translator;
using System.Web.Mvc;
using Axis.Helpers.Caching;
using System.Collections.Generic;
using System.Linq;
using Axis.Model.Setting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Configuration.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class LocationsController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}