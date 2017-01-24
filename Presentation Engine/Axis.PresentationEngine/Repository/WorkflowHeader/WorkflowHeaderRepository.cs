using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;
using Axis.PresentationEngine.Helpers.Model;
using Axis.Service;
using Axis.PresentationEngine.Helpers.Translator;
using System.Globalization;
using System.Collections.Specialized;
using Axis.Model.Registration;

namespace Axis.PresentationEngine.Repository
{
    public class WorkflowHeaderRepository : IWorkflowHeaderRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "WorkflowHeader/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditRepository"/> class.
        /// </summary>
        public WorkflowHeaderRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        
        public WorkflowHeaderRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

       
        public Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel workflowHeader)
        {
            string apiUrl = baseRoute + "AddWorkflowHeader";
            return communicationManager.Post<WorkflowHeaderModel, Response<WorkflowHeaderModel>>(workflowHeader, apiUrl);
        }

        public Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey, long headerID)
        {
            string apiUrl = baseRoute + "GetWorkflowHeader";
            var param = new NameValueCollection();
            param.Add("workflowKey", workflowKey.ToString());
            param.Add("headerID", headerID.ToString());
            return communicationManager.Get<Response<WorkflowHeaderModel>>(param, apiUrl);
        }


       
    }
}