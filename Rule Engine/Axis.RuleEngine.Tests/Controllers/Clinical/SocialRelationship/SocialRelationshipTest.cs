using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Common;
using Axis.Model.Clinical.SocialRelationship;
using Axis.RuleEngine.Clinical.SocialRelationship;
using Axis.RuleEngine.Plugins.Clinical;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers.Clinical.SocialRelationship
{
    /// <summary>
    /// Mock test for SocialRelationship
    /// </summary>
    [TestClass]
    public class SocialRelationshipTest
    {
        private ISocialRelationshipRuleEngine socialRelationhshipRuleEngine;

        private long _defaultContactId = 1;
        private long _defaultDeleteId = 1;
        private SocialRelationshipController socialRelationshipController;
        List<SocialRelationshipModel> socialRelationships;

        [TestInitialize]
        public void Initialize()
        {
        }

        /// <summary>
        /// The test method for GetSocialRelationshipsByContact success
        /// </summary>
        public void Mock_SocialRelationship()
        {
            var mock = new Mock<ISocialRelationshipRuleEngine>();
            socialRelationhshipRuleEngine = mock.Object;

            socialRelationshipController = new SocialRelationshipController(socialRelationhshipRuleEngine);

            socialRelationships = new List<SocialRelationshipModel>();
            socialRelationships.Add(new SocialRelationshipModel()
            {
                SocialRelationshipID = 1,
                ContactID = 1,
                EncounterID = null,
                ReviewedNoChanges = false,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            });

            var socialRelationshipResponse = new Response<SocialRelationshipModel>()
            {
                DataItems = socialRelationships
            };

            //Get Social Relationship
            mock.Setup(r => r.GetSocialRelationshipsByContact(It.IsAny<long>()))
                .Callback((long id) => { socialRelationshipResponse.DataItems = socialRelationships.Where(sr => sr.ContactID == id).ToList(); })
                .Returns(socialRelationshipResponse);

            //Add Social Relationship
            mock.Setup(r => r.AddSocialRelationship(It.IsAny<SocialRelationshipModel>()))
                .Callback((SocialRelationshipModel socialRelationModel) => { if (socialRelationModel.ContactID > 0) socialRelationships.Add(socialRelationModel); })
                .Returns(socialRelationshipResponse);

            //Update Social Relationship
            mock.Setup(r => r.UpdateSocialRelationship(It.IsAny<SocialRelationshipModel>()))
                .Callback((SocialRelationshipModel socialRelationModel) => { if (socialRelationModel.SocialRelationshipID > 0) { 
                                            socialRelationships.Remove(socialRelationships.Find(sr => sr.SocialRelationshipID == socialRelationModel.SocialRelationshipID)); socialRelationships.Add(socialRelationModel); } })
                .Returns(socialRelationshipResponse);

            //Delete Social Relationship
            var deleteResponse = new Response<SocialRelationshipModel>();
            mock.Setup(r => r.DeleteSocialRelationship(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => socialRelationships.Remove(socialRelationships.Find(sr => sr.SocialRelationshipID == id)))
                .Returns(deleteResponse);
        }

        /// <summary>
        /// The test method for GetSocialRelationshipsByContact success
        /// </summary>
        [TestMethod]
        public void GetSocialRelationshipsByContact_Success()
        {
            //Arrange
            Mock_SocialRelationship();

            //Act
            var getSocialRelationshipResult = socialRelationshipController.GetSocialRelationshipsByContact(_defaultContactId);
            var response = getSocialRelationshipResult as HttpResult<Response<SocialRelationshipModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one Social Relationship must exist.");
        }

        /// <summary>
        /// The test method for GetSocialRelationshipsByContact failure
        /// </summary>
        [TestMethod]
        public void GetSocialRelationshipsByContact_Failure()
        {
            //Arrange
            Mock_SocialRelationship();
            //Act
            var getSocialRelationshipResult = socialRelationshipController.GetSocialRelationshipsByContact(-1);
            var response = getSocialRelationshipResult as HttpResult<Response<SocialRelationshipModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response Value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() == 0, "Social Relationship exists for invalid data.");
        }

        /// <summary>
        /// The test method for AddSocialRelationship success
        /// </summary>
        [TestMethod]
        public void AddSocialRelationship_Success()
        {
            //Arrange
            Mock_SocialRelationship();
            var addSocialRelationship = new SocialRelationshipModel
            {
                SocialRelationshipID = 2,
                ContactID = 1,
                EncounterID = null,
                ReviewedNoChanges = false,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            //Act
            var addSocialRelationshipResult = socialRelationshipController.AddSocialRelationship(addSocialRelationship);
            var response = addSocialRelationshipResult as HttpResult<Response<SocialRelationshipModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 2, "Social Relationship could not be created.");
        }

        /// <summary>
        /// The test method for AddSocialRelationship failure
        /// </summary>
        [TestMethod]
        public void AddSocialRelationship_Failure()
        {
            //Arrange
            Mock_SocialRelationship();

            var addSocialRelationshipFailure = new SocialRelationshipModel
            {
                SocialRelationshipID = -1,
                ContactID = -1,
                EncounterID = null,
                ReviewedNoChanges = false,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            //Act
            var addSocialRelationshipResult = socialRelationshipController.AddSocialRelationship(addSocialRelationshipFailure);
            var response = addSocialRelationshipResult as HttpResult<Response<SocialRelationshipModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 1, "Social Relationship created for invalid record.");
        }

        /// <summary>
        /// The test method for UpdateSocialRelationship success
        /// </summary>
        [TestMethod]
        public void UpdateSocialRelationship_Success()
        {
            //Arrange
            Mock_SocialRelationship();
            var updateSocialRelationship = new SocialRelationshipModel
            {
                SocialRelationshipID = 1,
                ContactID = 1,
                EncounterID = null,
                ReviewedNoChanges = true,       //Updated
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            //Act
            var updateSocialRelationshipResult = socialRelationshipController.UpdateSocialRelationship(updateSocialRelationship);
            var response = updateSocialRelationshipResult as HttpResult<Response<SocialRelationshipModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(response.Value.DataItems[0].ReviewedNoChanges, "Social Relationship could not be updated.");
        }

        /// <summary>
        /// The test method for UpdateSocialRelationship failure
        /// </summary>
        [TestMethod]
        public void UpdateSocialRelationship_Failure()
        {
            //Arrange
            Mock_SocialRelationship();
            var updateSocialRelationship = new SocialRelationshipModel
            {
                SocialRelationshipID = -1,
                ContactID = 1,
                EncounterID = null,
                ReviewedNoChanges = true,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };
            //Act
            var updateSocialRelationshipResult = socialRelationshipController.UpdateSocialRelationship(updateSocialRelationship);
            var response = updateSocialRelationshipResult as HttpResult<Response<SocialRelationshipModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response value can't be null");
            Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
            Assert.IsTrue(!response.Value.DataItems[0].ReviewedNoChanges, "Social Relationship updated for invalid data.");
        }

        /// <summary>
        /// The test method for DeleteSocialRelationship success
        /// </summary>
        [TestMethod]
        public void DeleteSocialRelationship_Success()
        {
            //Arrange
            Mock_SocialRelationship();
            //Act
            var deleteSocialRelationshipResult = socialRelationshipController.DeleteSocialRelationship(_defaultDeleteId, DateTime.UtcNow);
            var response = deleteSocialRelationshipResult as HttpResult<Response<SocialRelationshipModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(socialRelationships.Count() == 0, "Social Relationship could not be deleted.");
        }

        /// <summary>
        /// The test method for DeleteSocialRelationship failure
        /// </summary>
        [TestMethod]
        public void DeleteSocialRelationship_Failure()
        {
            //Arrange
            Mock_SocialRelationship();
            //Act
            var deleteSocialRelationshipResult = socialRelationshipController.DeleteSocialRelationship(-1, DateTime.UtcNow);
            var response = deleteSocialRelationshipResult as HttpResult<Response<SocialRelationshipModel>>;

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.Value, "Response can't be null");
            Assert.IsTrue(socialRelationships.Count() > 0, "Social Relationship deleted for invalid record.");
        }
    }
}
