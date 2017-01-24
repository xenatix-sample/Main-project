using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration
{
    public class QuickRegistrationController : BaseApiController
    {
        readonly IQuickRegistrationRuleEngine _quickRegistrationRuleEngine;

        public QuickRegistrationController(IQuickRegistrationRuleEngine quickRegistrationRuleEngine)
        {
            _quickRegistrationRuleEngine = quickRegistrationRuleEngine;
        }

        public IHttpActionResult GetQuickRegistration()
        {
            return new HttpResult<Response<QuickRegistrationModel>>(_quickRegistrationRuleEngine.GetQuickRegistration(), Request);
        }

        public IHttpActionResult AddQuickRegistration(QuickRegistrationModel contact)
        {
            return new HttpResult<Response<QuickRegistrationModel>>(_quickRegistrationRuleEngine.AddQuickRegistration(contact), Request);
        }

        public IHttpActionResult GetMrnMpi()
        {
            return new HttpResult<Response<QuickRegistrationModel>>(_quickRegistrationRuleEngine.GetMrnMpi(), Request);
        }

    }
}
