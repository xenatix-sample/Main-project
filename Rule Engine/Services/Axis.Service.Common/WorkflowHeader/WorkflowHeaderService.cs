using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;

using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.Common.WorkFlowHeader
{
    public class WorkFlowHeaderService : IWorkFlowHeaderService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "WorkFlowHeader/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditService"/> class.
        /// </summary>
        public WorkFlowHeaderService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

       
        public Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel workflowHeader)
        {
            var apiUrl = baseRoute + "AddWorkflowHeader";           
            return communicationManager.Post<WorkflowHeaderModel, Response<WorkflowHeaderModel>>(workflowHeader, apiUrl);
        }

        public Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey, long headerID)
        {
            var apiUrl = baseRoute + "GetWorkflowHeader";
            var param = new NameValueCollection();
            param.Add("workflowKey", workflowKey.ToString());
            param.Add("headerID", headerID.ToString());
            return communicationManager.Get<Response<WorkflowHeaderModel>>(param, apiUrl);
        }

       
    }
}
