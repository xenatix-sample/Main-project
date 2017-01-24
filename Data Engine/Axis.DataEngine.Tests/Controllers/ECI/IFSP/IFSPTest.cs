using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Axis.DataProvider.ECI.IFSP;
using Axis.DataEngine.Plugins.ECI;
using Axis.Model.ECI;
using System.Collections.Generic;
using Axis.Model.Common;
using System.Linq;
using Axis.DataEngine.Helpers.Results;

namespace Axis.DataEngine.Tests.Controllers.ECI.IFSP
{
    [TestClass]
    public class IFSPTest
    {
        /// <summary>
        /// The IFSP data provider
        /// </summary>
        private IIFSPDataProvider ifspDataProvider;

        /// <summary>
        /// The default contact identifier
        /// </summary>
        private long defaultContactId = 1;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteContactId = 1;

        /// <summary>
        /// The ifsp controller
        /// </summary>
        private IFSPController ifspController;

        /// <summary>
        /// The IFSP data for success
        /// </summary>
        private IFSPDetailModel ifspDataForSuccess;

        /// <summary>
        /// The ifsp response
        /// </summary>
        private Response<IFSPDetailModel> ifspResponse;

        /// <summary>
        /// The IFSP list
        /// </summary>
        private List<IFSPDetailModel> ifsps;

        /// <summary>
        /// The mock
        /// </summary>
        private Mock<IIFSPDataProvider> mock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            InitializeMock();

            ifspDataForSuccess = new IFSPDetailModel()
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
            };
        }

        /// <summary>
        /// Initializes the mock.
        /// </summary>
        private void InitializeMock()
        {
            mock = new Mock<IIFSPDataProvider>();
            ifspDataProvider = mock.Object;
            ifspController = new IFSPController(ifspDataProvider);
        }

        /// <summary>
        /// Prepares the response.
        /// </summary>
        /// <param name="ifspData">The ifsp data.</param>
        private void PrepareResponse(IFSPDetailModel ifspData)
        {
            ifsps = new List<IFSPDetailModel>();
            if (ifspData.ContactID > 0)
                ifsps.Add(ifspData);

            ifspResponse = new Response<IFSPDetailModel>()
            {
                DataItems = ifsps
            };
        }

        /// <summary>
        /// Gets ifsps - success test case.
        /// </summary>
        [TestMethod]
        public void GetIFSPList_Success()
        {
            // Arrange
            PrepareResponse(ifspDataForSuccess);
            
            mock.Setup(r => r.GetIFSPList(It.IsAny<long>()))
                .Callback((long id) => { ifspResponse.DataItems = ifsps.Where(ifsp => ifsp.ContactID == id).ToList(); })
                .Returns(ifspResponse);

            //Act
            var getIFSPResult = ifspController.GetIFSPList(defaultContactId);
            var response = getIFSPResult as HttpResult<Response<IFSPDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one IFSP must exists.");
        }

        /// <summary>
        /// Gets ifsps - failure test case.
        /// </summary>
        [TestMethod]
        public void GetIFSPList_Failure()
        {
            // Arrange
            PrepareResponse(ifspDataForSuccess);
            
            mock.Setup(r => r.GetIFSPList(It.IsAny<long>()))
                .Callback((long id) => { ifspResponse.DataItems = ifsps.Where(ifsp => ifsp.ContactID == id).ToList(); })
                .Returns(ifspResponse);

            //Act
            var getIFSPResult = ifspController.GetIFSPList(-1);
            var response = getIFSPResult as HttpResult<Response<IFSPDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "IFSP exists for invalid data.");
        }

        /// <summary>
        /// Add ifsp - success test case
        /// </summary>
        [TestMethod]
        public void AddIFSP_Success()
        {
            // Arrange
            //PrepareResponse(ifspDataForSuccess);
            ifsps = new List<IFSPDetailModel>();
            ifspResponse = new Response<IFSPDetailModel>()
            {
                DataItems = ifsps
            };

            mock.Setup(r => r.AddIFSP(It.IsAny<IFSPDetailModel>()))
                .Callback((IFSPDetailModel ifspModel) => { if (ifspModel.IFSPID > 0) ifsps.Add(ifspModel); })
                .Returns(ifspResponse);

            //Act
            var addIFSP = new IFSPDetailModel()
            {
                IFSPID = 2,
                ContactID = 1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Add Success test case",
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
            Assert.IsTrue(response.Value.DataItems.Count == 1, "IFSP could not be created.");
        }

        /// <summary>
        /// Add ifsp - failure test case
        /// </summary>
        [TestMethod]
        public void AddIFSP_Failure()
        {
            // Arrange
            ifsps = new List<IFSPDetailModel>();
            ifspResponse = new Response<IFSPDetailModel>()
            {
                DataItems = ifsps
            };

            mock.Setup(r => r.AddIFSP(It.IsAny<IFSPDetailModel>()))
                .Callback((IFSPDetailModel ifspModel) => { if (ifspModel.IFSPID > 0) ifsps.Add(ifspModel); })
                .Returns(ifspResponse);

            //Act
            var addIFSP = new IFSPDetailModel()
            {
                IFSPID = -1,
                ContactID = -1,
                IFSPTypeID = -1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Add failure test case",
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
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 0, "IFSP created for invalid data.");
        }

        /// <summary>
        /// Update ifsp - success test case.
        /// </summary>
        [TestMethod]
        public void UpdateIFSP_Success()
        {
            // Arrange
            PrepareResponse(ifspDataForSuccess);

            mock.Setup(r => r.UpdateIFSP(It.IsAny<IFSPDetailModel>()))
                .Callback((IFSPDetailModel ifspModel) => { if (ifspModel.IFSPID > 0) { ifsps.Remove(ifsps.Find(ifsp => ifsp.IFSPID == ifspModel.IFSPID)); ifsps.Add(ifspModel); } })
                .Returns(ifspResponse);

            //Act
            var updateIfsp = new IFSPDetailModel()
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

        /// <summary>
        /// Update ifsp - failure test case.
        /// </summary>
        [TestMethod]
        public void UpdateIFSP_Failure()
        {
            // Arrange
            PrepareResponse(ifspDataForSuccess);

            mock.Setup(r => r.UpdateIFSP(It.IsAny<IFSPDetailModel>()))
                .Callback((IFSPDetailModel ifspModel) => { if (ifspModel.IFSPID > 0) { ifsps.Remove(ifsps.Find(ifsp => ifsp.IFSPID == ifspModel.IFSPID)); ifsps.Add(ifspModel); } })
                .Returns(ifspResponse);

            //Act
            var updateIfsp = new IFSPDetailModel()
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
            var updateIfspResult = ifspController.UpdateIFSP(updateIfsp);
            var response = updateIfspResult as HttpResult<Response<IFSPDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].Comments != "Update failure test case", "IFSP updated for invalid data.");
        }

        /// <summary>
        /// Delete ifsp - success test case.
        /// </summary>
        [TestMethod]
        public void RemoveIFSP_Success()
        {
            // Arrange
            ifsps = new List<IFSPDetailModel>();
            ifsps.Add(ifspDataForSuccess);
            var ifspResp = new Response<bool>();

            mock.Setup(r => r.RemoveIFSP(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => ifsps.Remove(ifsps.Find(ifsp => ifsp.IFSPID == id)))
                .Returns(ifspResp);

            //Act
            var deleteIfspResult = ifspController.RemoveIFSP(defaultDeleteContactId, DateTime.UtcNow);
            var response = deleteIfspResult as HttpResult<Response<bool>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(ifsps.Count() == 0, "IFSP could not be deleted.");
        }

        /// <summary>
        /// Delete IFSP - failure test case.
        /// </summary>
        [TestMethod]
        public void RemoveIFSP_Failure()
        {
            // Arrange
            ifsps = new List<IFSPDetailModel>();
            ifsps.Add(ifspDataForSuccess);
            var ifspResp = new Response<bool>();

            mock.Setup(r => r.RemoveIFSP(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => ifsps.Remove(ifsps.Find(ifsp => ifsp.IFSPID == id)))
                .Returns(ifspResp);

            //Act
            var deleteIfspResult = ifspController.RemoveIFSP(-1, DateTime.UtcNow);
            var response = deleteIfspResult as HttpResult<Response<bool>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response can't be null");
            Assert.IsTrue(ifsps.Count() > 0, "IFSP deleted for invalid record.");
        }
        
    }
}
