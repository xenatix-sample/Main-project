﻿using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.CallCenter.Controllers
{
    public class CallCenterProgressNoteController : BaseController
    {
        /// <summary>
        /// Returns default view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
