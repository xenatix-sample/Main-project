﻿using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.Controllers
{
    public class PresentIllnessController : BaseController
    {
        #region Action Results

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
