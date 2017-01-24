using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;
using Axis.Model.Registration;
using Axis.Service.Common.WorkFlowHeader;

namespace Axis.RuleEngine.Common.WorkflowHeader
{
    public class WorkflowHeaderRuleEngine : IWorkflowHeaderRuleEngine
    {
        
        private IWorkFlowHeaderService workFlowHeaderService;

        
        public WorkflowHeaderRuleEngine(IWorkFlowHeaderService workFlowHeaderService)
        {
            this.workFlowHeaderService = workFlowHeaderService;
        }
        public Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel workflowHeaderModel)
        {
            return workFlowHeaderService.AddWorkflowHeader(workflowHeaderModel);
        }

        public Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey, long headerID)
        {
            return workFlowHeaderService.GetWorkflowHeader(workflowKey, headerID);
        }
       
    }
}
