using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Common;
using Moq;
using System.Linq;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Common.WorkFlowHeader;
using Axis.DataEngine.Service.Controllers;
using Axis.Model.Common.WorkflowHeader;

namespace Axis.DataEngine.Tests.Controllers.WorkflowHeader
{
    [TestClass]
    public class WorkflowHeaderTest
    {
        
        private WorkFlowHeaderController workFlowHeaderController;
        private Mock<IWorkFlowHeaderDataProvider> mock;
        private WorkflowHeaderModel workflowHeaderModelForSuccess;
        private List<WorkflowHeaderModel> workflowHeaders ;
        

        private void InitializeMock()
        {
            mock = new Mock<IWorkFlowHeaderDataProvider>();
            workFlowHeaderController = new WorkFlowHeaderController(mock.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            InitializeMock();
            workflowHeaders = new List<WorkflowHeaderModel>();
            workflowHeaderModelForSuccess = new WorkflowHeaderModel()
            {
                RecordHeaderID = 1001,
                ContactID = 1,
                FirstName = "kishan",
                LastName = "mootha",               
                WorkflowDataKey = "Registration-Registration-Demographics",
               
            };
            workflowHeaders.Add(workflowHeaderModelForSuccess);

            //get Screen Audits
            Response<WorkflowHeaderModel> workflowHeaderReponse = new Response<WorkflowHeaderModel>();
            workflowHeaderReponse.DataItems = workflowHeaders;
            workflowHeaderReponse.RowAffected = 1;

            mock.Setup(r => r.AddWorkflowHeader(It.IsAny<WorkflowHeaderModel>()))
                .Callback((WorkflowHeaderModel workflowHeaderModel) => {
                    if(workflowHeaderModel.RecordHeaderID > 0)
                        workflowHeaders.Add(workflowHeaderModel);
                    })
               .Returns(workflowHeaderReponse);

        }
       
        [TestMethod]
        public void AddScreenAudit_Success()
        {
            //Act
            var workflowHeader = new WorkflowHeaderModel()
            {
                RecordHeaderID = 1002,
                ContactID = 2,
                FirstName = "kishantest",
                LastName = "moothatest",
                WorkflowDataKey = "Registration-Registration-Demographics_test",
            };

            var addWorkflowHeaderResult = workFlowHeaderController.AddWorkflowHeader(workflowHeader);
            var response = addWorkflowHeaderResult as HttpResult<Response<WorkflowHeaderModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 2, "Screen audit details are not added.");
        }
    }
}
