using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;
using Axis.Model.Registration;
using Axis.RuleEngine.Common.WorkflowHeader;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.Security;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class WorkflowHeaderController : BaseApiController
    {
       
        private readonly IWorkflowHeaderRuleEngine workflowHeaderRuleEngine = null;

       
        public WorkflowHeaderController(IWorkflowHeaderRuleEngine workflowHeaderRuleEngine)
        {
            this.workflowHeaderRuleEngine = workflowHeaderRuleEngine;
        }

        
        [HttpPost]
        public IHttpActionResult AddWorkflowHeader(WorkflowHeaderModel workflowHeader)
        {
            return new HttpResult<Response<WorkflowHeaderModel>>(workflowHeaderRuleEngine.AddWorkflowHeader(workflowHeader), Request);
        }

        [HttpGet]
        public IHttpActionResult GetWorkflowHeader(string workflowKey, long headerID)
        {
            return new HttpResult<Response<WorkflowHeaderModel>>(workflowHeaderRuleEngine.GetWorkflowHeader(workflowKey, headerID), Request);
        }

       
    }
}