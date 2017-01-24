using System.Web.Mvc;

namespace Axis.PresentationEngine.Controllers
{
    public class DigitalSignatureController : Controller
    {
        // GET: Signature
        public ActionResult DigitalSignature()
        {
            return View("../Shared/_DigitalSignature");
        }
    }
}