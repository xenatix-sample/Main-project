using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Common.WorkFlowHeader;
using Axis.Model.Common.WorkflowHeader;
using System.Web.Http;
using Axis.Model.Registration;
using Axis.Model.Common;

namespace Axis.DataEngine.Service.Controllers
{
    public class WorkFlowHeaderController : BaseApiController
    {
      
        private readonly IWorkFlowHeaderDataProvider workflowHeaderDataProvider;

       
        public WorkFlowHeaderController(IWorkFlowHeaderDataProvider workflowHeaderDataProvider)
        {
            this.workflowHeaderDataProvider = workflowHeaderDataProvider;
        }

        
        [HttpPost]
        public IHttpActionResult AddWorkflowHeader(WorkflowHeaderModel workflowHeader)
        {
            return new HttpResult<Response<WorkflowHeaderModel>>(workflowHeaderDataProvider.AddWorkflowHeader(workflowHeader), Request);
        }

        [HttpGet]
        public IHttpActionResult GetWorkflowHeader(string workflowKey, long headerID)
        {
            return new HttpResult<Response<WorkflowHeaderModel>>(workflowHeaderDataProvider.GetWorkflowHeader(workflowKey, headerID), Request);
        }
   
    }
}