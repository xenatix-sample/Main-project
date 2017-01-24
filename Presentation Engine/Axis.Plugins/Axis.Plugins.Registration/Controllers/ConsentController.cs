using System.Web.Mvc;
using Axis.Constant;
using Axis.PluginsEngine;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    public class ConsentController : BaseController
    {
        #region Action Results

        /// <summary>
        /// Index method that will attempt to load a signature based on the contactId provided
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public ActionResult Index()
        {
            //if (!IsPluginInstalled())
            //{
            //    //hide the esignature area since the plugin is not installed
            //}

            return View();
        }

        #endregion

        #region Private Methods

        private bool IsPluginInstalled()
        {
            string pluginSystemName = string.Format("{0}{1}", PluginPrefix.DefaultPluginPrefix, Axis.Constant.Plugins.ESignature);
            IPluginFinder pluginFinder = new PluginFinder();

            return pluginFinder.IsPluginInstalled(pluginSystemName);
        }

        #endregion
    }
}
