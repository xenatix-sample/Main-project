using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;
using Axis.Model.Registration;

namespace Axis.Service.Common.WorkFlowHeader
{
    public interface IWorkFlowHeaderService
    {
        Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel workflowHeader);
        Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey, long headerID);
    }
}
