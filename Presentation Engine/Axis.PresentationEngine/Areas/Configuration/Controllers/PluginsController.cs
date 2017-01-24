using Axis.Helpers;
using Axis.PluginsEngine;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Configuration.Controllers
{
    public class PluginsController : BaseController
    {

        private readonly IPluginFinder pluginFinder;
        private readonly IWebHelper webHelper;

        public PluginsController(IPluginFinder pluginFinder,
            IWebHelper webHelper)
        {
            this.pluginFinder = pluginFinder;
            this.webHelper = webHelper;
        
        }
        //
        // GET: /Configuration/Plugins/
        public ActionResult List()
        {
            var pluginDescriptors = pluginFinder.GetPluginDescriptors(PluginsType.All, null);
            return View(pluginDescriptors.ToList());
        }


        public ActionResult Install(string systemName)
        {
            try
            {
                var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName(systemName, PluginsType.All);
                if (pluginDescriptor == null)
                    //No plugin found with the specified id
                    return RedirectToAction("List");

                //check whether plugin is not installed
                if (pluginDescriptor.Installed)
                    return RedirectToAction("List");

                //install plugin
                pluginDescriptor.Instance().Install();
                
                //restart application
                webHelper.RestartAppDomain();
            }
            catch (Exception ex)
            {  
                //log the error in file
                _logger.Error(ex.Message, ex);
            }

            return RedirectToAction("List");
        }
        public ActionResult Uninstall(string systemName)
        {
            try
            {
                var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName(systemName, PluginsType.All);
                if (pluginDescriptor == null)
                    //No plugin found with the specified id
                    return RedirectToAction("List");

                //check whether plugin is installed
                if (!pluginDescriptor.Installed)
                    return RedirectToAction("List");

                //uninstall plugin
                pluginDescriptor.Instance().Uninstall();

                //restart application
                webHelper.RestartAppDomain();
            }
            catch (Exception ex)
            {
                //log the error in file
                _logger.Error(ex.Message, ex);
            }

            return RedirectToAction("List");
        }

	}
}