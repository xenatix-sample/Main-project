using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;


namespace Axis.PresentationEngine.Repository
{
    public interface IWorkflowHeaderRepository
    {
       
        Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel clientAudit);
        Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey, long headerID);

       
    }
}