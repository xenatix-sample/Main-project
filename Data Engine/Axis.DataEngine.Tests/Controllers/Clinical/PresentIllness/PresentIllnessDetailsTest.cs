using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Clinical.PresentIllness;
using System.Globalization;
using Axis.DataEngine.Plugins.Clinical;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.PresentIllness;
using Axis.DataProvider.Clinical;
using Axis.Model.Clinical;

namespace Axis.DataEngine.Tests.Controllers.Clinical.PresentIllness
{
    /// <summary>
    /// Mock test for PresentIllness
    /// </summary>
    [TestClass]
    public class PresentIllnessDetailTest
    {
        private IPresentIllnessDataProvider PresentIllnessRuleEngine;

        private long _defaultContactId = 1;
        private long _defaultDeleteId = 1;
        private PresentIllnessController PresentIllnessController;
        List<PresentIllnessDetailModel> HPIs;

        [TestInitialize]
        public void Initialize()
        {
        }

        /// <summary>
        /// Mock initialization
        /// </summary>
        public void Mock_PresentIllness()
        {
            var mock = new Mock<IPresentIllnessDataProvider>();
            PresentIllnessRuleEngine = mock.Object;

            PresentIllnessController = new PresentIllnessController(PresentIllnessRuleEngine);

            HPIs = new List<PresentIllnessDetailModel>();
            HPIs.Add(new PresentIllnessDetailModel()
            {
                HPIDetailID = 57,
                HPIID = 1,
                Comment = "Some comment",
                Location = "SOme location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            });

            var HPIResponse = new Response<PresentIllnessDetailModel>()
            {
                DataItems = HPIs
            };

            //Get hpi detail
            mock.Setup(r => r.GetHPIDetail(It.IsAny<long>()))
                .Callback((long id) => { HPIResponse.DataItems = HPIs.Where(sr => sr.ContactID == id).ToList(); })
                .Returns(HPIResponse);

            //Add hpi detail
            mock.Setup(r => r.AddHPIDetail(It.IsAny<PresentIllnessDetailModel>()))
                .Callback((PresentIllnessDetailModel presentIllnessModel) => { if (presentIllnessModel.ContactID > 0) HPIs.Add(presentIllnessModel); })
                .Returns(HPIResponse);

            //Updatehpi detail
            mock.Setup(r => r.UpdateHPIDetail(It.IsAny<PresentIllnessDetailModel>()))
                .Callback((PresentIllnessDetailModel presentIllnessModel) =>
                {
                    if (presentIllnessModel.HPIDetailID > 0)
                    {
                        HPIs.Remove(HPIs.Find(sr => sr.HPIDetailID == presentIllnessModel.HPIDetailID)); HPIs.Add(presentIllnessModel);
                    }
                })
                .Returns(HPIResponse);

            //Delete HPI DETAIL
            var deleteResponse = new Response<PresentIllnessDetailModel>();
            mock.Setup(r => r.DeleteHPIDetail(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => HPIs.Remove(HPIs.Find(sr => sr.HPIDetailID == id)))
                .Returns(deleteResponse);

            //Get Social Relationship details
            mock.Setup(r => r.GetHPIDetail(It.IsAny<long>()))
                .Callback((long id) => { HPIResponse.DataItems = HPIs.Where(sr => sr.ContactID == id).ToList(); })
                .Returns(HPIResponse);
        }

        /// <summary>
        /// The test method for GetHPIDetail success
        /// </summary>
        [TestMethod]
        public void GetHPIDetail_Success()
        {
            //Arrange
            Mock_PresentIllness();

            //Act
            var getHPIResult = PresentIllnessController.GetHPIDetail(_defaultContactId);
            var response = getHPIResult as HttpResult<Response<PresentIllnessDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one record must exist.");
        }

        /// <summary>
        /// The test method for GetHPIDetail failure
        /// </summary>
        [TestMethod]
        public void GetHPIDetail_Failure()
        {
            //Arrange
            Mock_PresentIllness();
            //Act
            var getHPIResult = PresentIllnessController.GetHPIDetail(-1);
            var response = getHPIResult as HttpResult<Response<PresentIllnessDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Record exist for invalid data.");
        }

        /// <summary>
        /// The test method for AddHPIDetail success
        /// </summary>
        [TestMethod]
        public void AddHPIDetail_Success()
        {
            //Arrange
            Mock_PresentIllness();
            var addHPIDetail = new PresentIllnessDetailModel
            {

                HPIDetailID = 57,
                HPIID = 1,
                Comment = "Some comment",
                Location = "SOme location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            //Act
            var addHPIResult = PresentIllnessController.AddHPIDetail(addHPIDetail);
            var response = addHPIResult as HttpResult<Response<PresentIllnessDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 2, "HPI Details could not be saved.");
        }

        /// <summary>
        /// The test method for addHPIDetail failure
        /// </summary>
        [TestMethod]
        public void AddHPIDetail_Failure()
        {
            //Arrange
            Mock_PresentIllness();

            var addHPIDetailFailure = new PresentIllnessDetailModel
            {
                HPIDetailID = -57,
                HPIID = 1,
                Comment = "Some comment",
                Location = "SOme location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            //Act
            var addHPIResult = PresentIllnessController.AddHPIDetail(addHPIDetailFailure);
            var response = addHPIResult as HttpResult<Response<PresentIllnessDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 1, "HPI details saved for invalid record.");
        }

        /// <summary>
        /// The test method for UpdateHPI Detail success
        /// </summary>
        [TestMethod]
        public void UpdateHPIDetail_Success()
        {
            //Arrange
            Mock_PresentIllness();
            var updateHPIDetail = new PresentIllnessDetailModel
            {
                HPIDetailID = 57,
                HPIID = 1,
                Comment = "Some reasonable comment",
                Location = "SOme different location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            //Act
            var updateHPIResult = PresentIllnessController.UpdateHPIDetail(updateHPIDetail);
            var response = updateHPIResult as HttpResult<Response<PresentIllnessDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
           
        }

        /// <summary>
        /// The test method for UpdateSocialRelationHistory failure
        /// </summary>
        [TestMethod]
        public void UpdateHPIDetail_Failure()
        {
            //Arrange
            Mock_PresentIllness();
            var updateHPIDetail = new PresentIllnessDetailModel
            {
                HPIDetailID = -57,
                HPIID = 1,
                Comment = "Some reasonable comment",
                Location = "SOme different location",
                Quality = "SOme QUality Updated.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            //Act
            var updateHPIResult = PresentIllnessController.UpdateHPIDetail(updateHPIDetail);
            var response = updateHPIResult as HttpResult<Response<PresentIllnessDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].Quality != "Some quality Updated", "HPI updated for invalid data.");
        }

        /// <summary>
        /// The test method for DeleteHPIDetail success
        /// </summary>
        [TestMethod]
        public void DeleteHPIDetail_Success()
        {
            //Arrange
            Mock_PresentIllness();
            //Act
            var deleteHPIResult = PresentIllnessController.DeleteHPIDetail(_defaultDeleteId, DateTime.UtcNow);
            var response = deleteHPIResult as HttpResult<Response<PresentIllnessDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(HPIs.Count() == 0, "HPI details could not be deleted.");
        }

        /// <summary>
        /// The test method for DeleteSocialRelationHistory failure
        /// </summary>
        [TestMethod]
        public void DeleteHPIDetail_Failure()
        {
            //Arrange
            Mock_PresentIllness();
            //Act
            var deleteHPIResult = PresentIllnessController.DeleteHPIDetail(-1, DateTime.UtcNow);
            var response = deleteHPIResult as HttpResult<Response<PresentIllnessDetailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response can't be null");
            Assert.IsTrue(HPIs.Count() > 0, "HPI details deleted for invalid record.");
        }

       
    }
}
