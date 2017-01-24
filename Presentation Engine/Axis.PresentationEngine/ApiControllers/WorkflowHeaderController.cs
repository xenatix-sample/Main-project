using System.Web.Http;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Repository;
using Axis.PresentationEngine.Helpers.Model;
using Axis.Model.Common.WorkflowHeader;
using Axis.Model.Registration;

namespace Axis.PresentationEngine.ApiControllers
{
    public class WorkflowHeaderController : BaseApiController
    {
       
        private readonly IWorkflowHeaderRepository workflowHeaderRepository;

       
        public WorkflowHeaderController(IWorkflowHeaderRepository workflowHeaderRepository) 
        {
            this.workflowHeaderRepository = workflowHeaderRepository;
        }

        
        [HttpPost]
        public Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel workflowHeader)
        {
            return workflowHeaderRepository.AddWorkflowHeader(workflowHeader);
        }

      
        [HttpGet]
        public Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey, long headerID)
        {
            return workflowHeaderRepository.GetWorkflowHeader(workflowKey, headerID);
        }

       

    }
}