using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Common;
using Axis.Model.Clinical.PresentIllness;
using Axis.RuleEngine.Clinical.PresentIllness;
using Axis.RuleEngine.Plugins.Clinical;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers.Clinical.PresentIllness
{
    /// <summary>
    /// Mock test for PresentIllness
    /// </summary>
    [TestClass]
    public class PresentIllnessTest
    {
        private IPresentIllnessRuleEngine PresentIllnessRuleEngine;

        private long _defaultContactId = 1;
        private long _defaultDeleteId = 1;
        private PresentIllnessController PresentIllnessController;
        List<PresentIllnessModel> HPIs;

        [TestInitialize]
        public void Initialize()
        {
        }

        /// <summary>
        /// Mock Details
        /// </summary>
        public void Mock_PresentIllness()
        {
            var mock = new Mock<IPresentIllnessRuleEngine>();
            PresentIllnessRuleEngine = mock.Object;

            PresentIllnessController = new PresentIllnessController(PresentIllnessRuleEngine);

            HPIs = new List<PresentIllnessModel>();
            HPIs.Add(new PresentIllnessModel()
            {

                ContactID = 1,
                EncounterID = null,
                HPIID = 1,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            });

            var PresentIllnessResponse = new Response<PresentIllnessModel>()
            {
                DataItems = HPIs
            };

            //Get HPI
            mock.Setup(r => r.GetHPI(It.IsAny<long>()))
                .Callback((long id) => { PresentIllnessResponse.DataItems = HPIs.Where(pi => pi.ContactID == id).ToList(); })
                .Returns(PresentIllnessResponse);

            //Add PresentIllness
            mock.Setup(r => r.AddHPI(It.IsAny<PresentIllnessModel>()))
                .Callback((PresentIllnessModel PresentIllnessModel) => { if (PresentIllnessModel.ContactID > 0) HPIs.Add(PresentIllnessModel); })
                .Returns(PresentIllnessResponse);

            //Update PresentIllness
            mock.Setup(r => r.UpdateHPI(It.IsAny<PresentIllnessModel>()))
                .Callback((PresentIllnessModel PresentIllnessModel) =>
                {
                    if (PresentIllnessModel.HPIID > 0)
                    {
                        HPIs.Remove(HPIs.Find(pi => pi.HPIID == PresentIllnessModel.HPIID)); HPIs.Add(PresentIllnessModel);
                    }
                })
                .Returns(PresentIllnessResponse);

            //Delete PresentIllness
            var deleteResponse = new Response<PresentIllnessModel>();
            mock.Setup(r => r.DeleteHPI(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => HPIs.Remove(HPIs.Find(pi => pi.HPIID == id)))
                .Returns(deleteResponse);
        }

        /// <summary>
        /// The test method for Gethpi success
        /// </summary>
        [TestMethod]
        public void GetHPI_Success()
        {
            //Arrange
            Mock_PresentIllness();

            //Act
            var getPresentIllnessResult = PresentIllnessController.GetHPI(_defaultContactId);
            var response = getPresentIllnessResult as HttpResult<Response<PresentIllnessModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one HPI must exist.");
        }

        /// <summary>
        /// The test method for GetHPI failure
        /// </summary>
        [TestMethod]
        public void GetHPI_Failure()
        {
            //Act
            Mock_PresentIllness();

            var getPresentIllnessResult = PresentIllnessController.GetHPI(-1);
            var response = getPresentIllnessResult as HttpResult<Response<PresentIllnessModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Atleast one HPI must exist.");
        }

        /// <summary>
        /// The test method for AddHPIsuccess
        /// </summary>
        [TestMethod]
        public void AddHPI_Success()
        {
            //Act
            Mock_PresentIllness();
            var addHPI = new PresentIllnessModel
            {
                HPIID = 2,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addHPIResult = PresentIllnessController.AddHPI(addHPI);
            var response = addHPIResult as HttpResult<Response<PresentIllnessModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 2, "HPI could not be created.");
        }

        /// <summary>
        /// The test method for AddHPI failure
        /// </summary>
        [TestMethod]
        public void AddHPI_Failure()
        {
            //Act
            Mock_PresentIllness();

            var addHPIFailure = new PresentIllnessModel
            {
                HPIID = -1,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var addHPIResult = PresentIllnessController.AddHPI(addHPIFailure);
            var response = addHPIResult as HttpResult<Response<PresentIllnessModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 1, "HPI created for invalid record.");
        }

        /// <summary>
        /// The test method for UpdateHPI success
        /// </summary>
        [TestMethod]
        public void UpdateHPI_Success()
        {
            //Act
            Mock_PresentIllness();
            var updateHPI = new PresentIllnessModel
            {
                HPIID = 1,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateHPIResult = PresentIllnessController.UpdateHPI(updateHPI);
            var response = updateHPIResult as HttpResult<Response<PresentIllnessModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");

        }

        /// <summary>
        /// The test method for UpdateHPI failure
        /// </summary>
        [TestMethod]
        public void UpdateHPI_Failure()
        {
            //Act
            Mock_PresentIllness();
            var updateHPI = new PresentIllnessModel
            {
                HPIID = -1,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            var updateHPIResult = PresentIllnessController.UpdateHPI(updateHPI);
            var response = updateHPIResult as HttpResult<Response<PresentIllnessModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");

        }

        /// <summary>
        /// The test method for DeleteHPI success
        /// </summary>
        [TestMethod]
        public void DeleteHPI_Success()
        {
            //Act
            Mock_PresentIllness();
            var deleteHPIResult = PresentIllnessController.DeleteHPI(_defaultDeleteId, DateTime.UtcNow);
            var response = deleteHPIResult as HttpResult<Response<PresentIllnessModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(HPIs.Count() == 0, "HPI could not be deleted.");
        }

        /// <summary>
        /// The test method for DeleteHPI failure
        /// </summary>
        [TestMethod]
        public void DeleteHPI_Failure()
        {
            //Act
            Mock_PresentIllness();
            var deleteHPIResult = PresentIllnessController.DeleteHPI(-1, DateTime.UtcNow);
            var response = deleteHPIResult as HttpResult<Response<PresentIllnessModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response can't be null");
            Assert.IsTrue(HPIs.Count() > 0, "HPI deleted for invalid record.");
        }
    }
}
