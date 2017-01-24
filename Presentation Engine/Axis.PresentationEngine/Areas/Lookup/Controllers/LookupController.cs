using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Helpers.Repositories;
using Axis.PresentationEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Lookup.Controllers
{
    public class LookupController : BaseController
    {
        #region Class Variables

        private ILookupRepository _lookupRepository;

        #endregion Class Variables

        #region Constructors

        public LookupController(ILookupRepository lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }

        #endregion Constructors

        #region Action Results

        [HttpGet]
        public ActionResult GetLookupsToCache()
        {
            var response = _lookupRepository.GetLookupsToCache();

            var lookups = response.DataItems.First();

            var lookupsViewModel = new JsonViewModel()
            {
                Json = JsonConvert.SerializeObject(lookups)
            };

            Response.ContentType = "text/javascript";
            return View(lookupsViewModel);
        }

        [HttpGet]
        public ActionResult GetReportsToCache()
        {
            var response = _lookupRepository.GetReportsToCache();

            var reportsKey = LookupType.Reports.ToString();
            var lookups = response.DataItems.First();
            var reports = new List<dynamic>();
            if (lookups.ContainsKey(reportsKey))
                reports = lookups[reportsKey];

            var lookupsViewModel = new JsonViewModel()
            {
                Json = String.Join(",", reports.Select(x => string.Format("'{0}': {1}", x.Name, x.Model)).ToArray())
            };

            Response.ContentType = "text/javascript";
            return View(lookupsViewModel);
        }

        [HttpGet]
        public ActionResult GetDrugsToCache()
        {
            var response = _lookupRepository.GetDrugsToCache();

            var lookups = response.DataItems.First();

            var lookupsViewModel = new JsonViewModel()
            {
                Json = JsonConvert.SerializeObject(lookups)
            };

            Response.ContentType = "text/javascript";
            return View(lookupsViewModel);
        }

        //TODO: Implement type-ahead search

        [HttpGet]
        public ActionResult GetLookupsByType(LookupType lookupType)
        {
            var response = _lookupRepository.GetLookupsByType(lookupType);

            var lookups = response.DataItems.First();

            return Content(JsonConvert.SerializeObject(lookups), "text/javascript");
        }

        /// <summary>
        /// Get all lookup Types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLookupItems()
        {
            var enumDataItems = new Response<string>();
            enumDataItems.DataItems = System.Enum.GetNames(typeof(LookupType)).ToList();
            return Json(enumDataItems.DataItems);
        }

        #endregion Action Results
    }
}