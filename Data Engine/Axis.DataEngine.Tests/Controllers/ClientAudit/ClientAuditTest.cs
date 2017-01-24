using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Common;
using Moq;
using System.Linq;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Common.ClientAudit;
using Axis.DataEngine.Service.Controllers;
using Axis.Model.Common.ClientAudit;

namespace Axis.DataEngine.Tests.Controllers.ClientAudit
{
    [TestClass]
    public class ClientAuditTest
    {
        
        private ClientAuditController clientAuditController;
        private Mock<IClientAuditDataProvider> mock;
        private ScreenAuditModel screenAuditDataForSuccess;
        private List<ScreenAuditModel> screenAudits ;
        

        private void InitializeMock()
        {
            mock = new Mock<IClientAuditDataProvider>();
            clientAuditController = new ClientAuditController(mock.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            InitializeMock();
            screenAudits = new List<ScreenAuditModel>();
            screenAuditDataForSuccess = new ScreenAuditModel()
            {
                TransactionID = 1001,
                UserID = 1,
                ContactID = 1,
                DataKey = "Registration-Registration-Demographics",
                ActionTypeID = 1
            };
            screenAudits.Add(screenAuditDataForSuccess);

            //get Screen Audits
            Response<ScreenAuditModel> screemAuditesponse = new Response<ScreenAuditModel>();
            screemAuditesponse.DataItems = screenAudits;
            screemAuditesponse.RowAffected = 1;

            mock.Setup(r => r.AddScreenAudit(It.IsAny<ScreenAuditModel>()))
                .Callback((ScreenAuditModel screenAudit) => {
                    if(screenAudit.UserID > 0)
                    screenAudits.Add(screenAudit);
                    })
               .Returns(screemAuditesponse);

        }
       
        [TestMethod]
        public void AddScreenAudit_Success()
        {
            //Act
            var addScreenAudit= new ScreenAuditModel()
            {
                TransactionID = 1002,
                UserID = 2,
                ContactID = 2,
                DataKey = "Registration-Registration-AdditionalDemographics",
                ActionTypeID = 2
            };

            var addNoteResult = clientAuditController.AddScreenAudit(addScreenAudit);
            var response = addNoteResult as HttpResult<Response<ScreenAuditModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 2, "Screen audit details are not added.");
        }

        [TestMethod]
        public void AddScreenAudit_Failure()
        {
            //Act
            var addScreenAudit = new ScreenAuditModel()
            {
                TransactionID = -1001,
                UserID = -2,
                ContactID = -2,
                DataKey = "Registration-Registration-AdditionalDemographics",
                ActionTypeID = 2
            };

            var addNoteResult = clientAuditController.AddScreenAudit(addScreenAudit);
            var response = addNoteResult as HttpResult<Response<ScreenAuditModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 1, "Screen audit details are added for invalid data.");
        }
    }
}
