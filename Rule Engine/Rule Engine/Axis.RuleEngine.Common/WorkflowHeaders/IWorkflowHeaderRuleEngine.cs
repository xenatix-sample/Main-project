using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;


namespace Axis.RuleEngine.Common.WorkflowHeader
{
    public interface IWorkflowHeaderRuleEngine
    {
        Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel workflowHeader);
        Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey, long headerID);
    }
}
