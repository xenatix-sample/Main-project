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
using System.Dynamic;

namespace Axis.PresentationEngine.Areas.Configuration.Controllers
{
    public class SettingsController : BaseController
    {
        #region Class Variables

        private IConfigurationRepository configurationRepository;

        #endregion

        #region Constructors

        public SettingsController(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        #endregion

        #region Action Results

        // GET: Configuration/Settings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetSettingsToCache(bool forceServerCacheReset = false)
        {
            var response = configurationRepository.GetSettingsToCache(forceServerCacheReset);

            var settingsList = response.ToModel().DataItems;

            //Potentialy cache the settings retrieved from the database if needed
            SettingsCacheManager settingsCache = new SettingsCacheManager(settingsList, forceServerCacheReset);

            //filter all of the settings based on priority user-plugin-application with user being the highest
            var filteredSettings = settingsList.GroupBy(g => g.SettingsID).Select(o => o.OrderByDescending(c => c.SettingsTypeID).First()).ToList();

            //Serialize and convert to js for local caching
            dynamic settings = new ExpandoObject();
            settings.SettingsList = filteredSettings;

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented, serializerSettings);

            var settingsViewModel = new GetSettingsToCacheViewModel()
            {
                SettingsJson = settingsJson
            };

            if(Response != null)
                Response.ContentType = "text/javascript";

            return View(settingsViewModel);
        }

        #endregion

        #region JsonResults

        [HttpGet]
        public JsonResult GetNonUserSettings()
        {
            return Json(configurationRepository.GetNonUserSettings(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSetting(SettingsViewModel setting)
        {
            var response = configurationRepository.UpdateSetting(setting);

            if(response.ResultCode == 0)
            {
                //Update the in-memory cache
                GetSettingsToCache(true);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }        

        #endregion
    }
}