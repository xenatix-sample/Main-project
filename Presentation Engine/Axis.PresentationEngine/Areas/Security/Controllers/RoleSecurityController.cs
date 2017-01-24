using Axis.PresentationEngine.Areas.Security.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Models;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Security.Controllers
{
    public class RoleSecurityController : BaseController
    {
        private readonly ISecurityRepository securityRepository;

        public RoleSecurityController(
            ISecurityRepository securityRepository)
        {
            this.securityRepository = securityRepository;
        }

        [HttpGet]
        public ActionResult GetUserRoleSecurity()
        {
            var response = securityRepository.GetUserRoleSecurity();

            Response.ContentType = "text/javascript";
            return View(new JsonViewModel()
            {
                Json = JsonConvert.SerializeObject(response.DataItems)
            });
        }
    }
}