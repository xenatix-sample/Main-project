using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Registration;
using Moq;
using System.Collections.Generic;
using Axis.Model.Common;
using System.Linq;
using Axis.RuleEngine.Registration;
using Axis.RuleEngine.Plugins.Registration;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.ECI;
using Axis.Model.ECI;
using Axis.RuleEngine.Plugins.ECI;

namespace Axis.RuleEngine.Tests.Controllers.ECI.IFSP
{
    /// <summary>
    /// Mock test for IFSP
    /// </summary>
    [TestClass]
    public class IFSPTest
    {
        private IIFSPRuleEngine ifspRuleEngine;

        private long _defaultContactId = 1;
        private long _defaultDeleteIFSPId = 1;
        private IFSPController ifspController;
        List<IFSPDetailModel> ifsps;

        [TestInitialize]
        public void Initialize()
        {
        }

        public void Mock_IFSP()
        {
            var mock = new Mock<IIFSPRuleEngine>();
            ifspRuleEngine = mock.Object;

            ifspController = new IFSPController(ifspRuleEngine);

            ifsps = new List<IFSPDetailModel>();
            ifsps.Add(new IFSPDetailModel()
            {
                IFSPID = 1,
                ContactID = 1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Success test case",
                AssessmentID = 1,
                ResponseID = 1,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            });

            var ifspResponse = new Response<IFSPDetailModel>()
            {
                DataItems = ifsps
            };
            
            //Response<IFSPDetailModel> ifspResponse = new Response<IFSPDetailModel>();

            //Get IFSP
            mock.Setup(r => r.GetIFSPList(It.IsAny<long>()))
                .Callback((long id) => { ifspResponse.DataItems = ifsps.Where(ifsp => ifsp.ContactID == id).ToList(); })
                .Returns(ifspResponse);

            //Add IFSP
            mock.Setup(r => r.AddIFSP(It.IsAny<IFSPDetailModel>()))
                .Callback((IFSPDetailModel ifspModel) => { if (ifspModel.ContactID > 0) ifsps.Add(ifspModel); })
                .Returns(ifspResponse);

            //Update IFSP
            mock.Setup(r => r.UpdateIFSP(It.IsAny<IFSPDetailModel>()))
                .Callback((IFSPDetailModel ifspModel) => { if (ifspModel.IFSPID > 0) { ifsps.Remove(ifsps.Find(ifsp => ifsp.IFSPID == ifspModel.IFSPID)); ifsps.Add(ifspModel); } })
                .Returns(ifspResponse);

            //Delete IFSP
            var deleteResponse = new Response<bool>();
            mock.Setup(r => r.RemoveIFSP(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => ifsps.Remove(ifsps.Find(ifsp => ifsp.IFSPID == id)))
                .Returns(deleteResponse);
        }

        [TestMethod]
        public void GetIFSPList_Success()
        {
            //Arrange
            Mock_IFSP();

            //Act
            var getIfspResult = ifspController.GetIFSPList(_defaultContactId);
            var response = getIfspResult as HttpResult<Response<IFSPDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one IFSP must exist.");
        }

        [TestMethod]
        public void GetIFSPList_Failure()
        {
            //Act
            Mock_IFSP();

            var getIFSPResult = ifspController.GetIFSPList(-1);
            var response = getIFSPResult as HttpResult<Response<IFSPDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Atleast one IFSP must exist.");
        }

        [TestMethod]
        public void AddIFSP_Success()
        {
            //Act
            Mock_IFSP();
            var addIFSP = new IFSPDetailModel
            {
                IFSPID = 2,
                ContactID = 1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Success test case",
                AssessmentID = 1,
                ResponseID = 1,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addIFSPResult = ifspController.AddIFSP(addIFSP);
            var response = addIFSPResult as HttpResult<Response<IFSPDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 2, "IFSP could not be created.");
        }

        [TestMethod]
        public void AddIFSP_Failure()
        {
            //Act
            Mock_IFSP();

            var addIFSPFailure = new IFSPDetailModel
            {
                IFSPID = -1,
                ContactID = -1,
                IFSPTypeID = -1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Failure test case",
                AssessmentID = 1,
                ResponseID = 1,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addIFSPResult = ifspController.AddIFSP(addIFSPFailure);
            var response = addIFSPResult as HttpResult<Response<IFSPDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 1, "IFSP created for invalid record.");
        }

        [TestMethod]
        public void UpdateIFSP_Success()
        {
            //Act
            Mock_IFSP();
            var updateIfsp = new IFSPDetailModel
            {
                IFSPID = 1,
                ContactID = 1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Update Success test case",
                AssessmentID = 1,
                ResponseID = 1,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateIFSPResult = ifspController.UpdateIFSP(updateIfsp);
            var response = updateIFSPResult as HttpResult<Response<IFSPDetailModel>>;
            
            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].Comments == "Update Success test case", "IFSP could not be updated.");
        }

        [TestMethod]
        public void UpdateIFSP_Failure()
        {
            //Act
            Mock_IFSP();
            var updateIfsp = new IFSPDetailModel
            {
                IFSPID = -1,
                ContactID = -1,
                IFSPTypeID = -1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Update failure test case",
                AssessmentID = 1,
                ResponseID = 1,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateIFSPResult = ifspController.UpdateIFSP(updateIfsp);
            var response = updateIFSPResult as HttpResult<Response<IFSPDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].Comments != "Update failure test case", "IFSP updated for invalid data.");
        }

        [TestMethod]
        public void RemoveIFSP_Success()
        {
            //Act
            Mock_IFSP();
            var deleteIFSPResult = ifspController.RemoveIFSP(_defaultDeleteIFSPId, DateTime.UtcNow);
            var response = deleteIFSPResult as HttpResult<Response<bool>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(ifsps.Count() == 0, "IFSP could not be deleted.");
        }

        [TestMethod]
        public void RemoveIFSP_Failure()
        {
            //Act
            Mock_IFSP();
            var deleteIFSPResult = ifspController.RemoveIFSP(-1, DateTime.UtcNow);
            var response = deleteIFSPResult as HttpResult<Response<bool>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response can't be null");
            Assert.IsTrue(ifsps.Count() > 0, "IFSP deleted for invalid record.");
        }
    }
}
