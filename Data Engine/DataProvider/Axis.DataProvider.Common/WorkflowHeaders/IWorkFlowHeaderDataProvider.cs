using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;


namespace Axis.DataProvider.Common.WorkFlowHeader
{
    public interface IWorkFlowHeaderDataProvider
    {
        
        Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel workflowHeader);
        Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey,long headerID);

       
    }
}
