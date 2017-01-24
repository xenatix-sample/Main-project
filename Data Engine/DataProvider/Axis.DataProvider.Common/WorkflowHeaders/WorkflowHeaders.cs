using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.WorkflowHeader;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Axis.Security;


namespace Axis.DataProvider.Common.WorkFlowHeader
{
    public class WorkFlowHeaderDataProvider : IWorkFlowHeaderDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkFlowHeaderDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public WorkFlowHeaderDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion initializations

       
       
        public Response<WorkflowHeaderModel> AddWorkflowHeader(WorkflowHeaderModel workflowHeader)
        {
            SqlParameter workflowKeyParam = new SqlParameter("WorkflowDataKey", workflowHeader.WorkflowDataKey);
            SqlParameter headerIDParam = new SqlParameter("RecordPrimaryKeyValue", workflowHeader.RecordHeaderID);
            SqlParameter contactIDParam = new SqlParameter("ContactID", workflowHeader.ContactID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { workflowKeyParam, headerIDParam, contactIDParam, modifiedOnParam };
            var repository = _unitOfWork.GetRepository<WorkflowHeaderModel>(SchemaName.Core);
            return _unitOfWork.EnsureInTransaction(
                    repository.ExecuteNQStoredProc,
                    "usp_SaveRecordHeader",
                    procParams
                );
        }

        public Response<WorkflowHeaderModel> GetWorkflowHeader(string workflowKey, long headerID)
        {
            SqlParameter workflowKeyParam = new SqlParameter("WorkflowDataKey", workflowKey);
            SqlParameter headerIDParam = new SqlParameter("RecordPrimaryKeyValue", headerID);

            List<SqlParameter> procParams = new List<SqlParameter>() { workflowKeyParam, headerIDParam };

            var repository = _unitOfWork.GetRepository<WorkflowHeaderModel>(SchemaName.Core);
            return repository.ExecuteStoredProc("usp_GetRecordHeaderDetails", procParams);
           
        }
    }
}
