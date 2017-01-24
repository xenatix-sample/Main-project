using Axis.PresentationEngine.Areas.Security.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Security.Controllers
{
    public class CredentialSecurityController : BaseController
    {
        private readonly ISecurityRepository securityRepository;

        public CredentialSecurityController(
            ISecurityRepository securityRepository)
        {
            this.securityRepository = securityRepository;
        }

      

        [HttpGet]
        public JsonResult GetUserCredentialSecurity()
        {
            return Json(securityRepository.GetUserCredentialSecurity(), JsonRequestBehavior.AllowGet);
        }

    }
}