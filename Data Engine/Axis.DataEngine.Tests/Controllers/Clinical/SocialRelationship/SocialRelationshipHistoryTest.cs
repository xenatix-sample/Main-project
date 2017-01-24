using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Clinical.SocialRelationship;
using System.Globalization;
using Axis.DataEngine.Plugins.Clinical;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.SocialRelationship;
using Axis.DataProvider.Clinical;
using Axis.Model.Clinical;

namespace Axis.DataEngine.Tests.Controllers.Clinical.SocialRelationship
{
    /// <summary>
    /// Mock test for SocialRelationship
    /// </summary>
    [TestClass]
    public class SocialRelationshipHistoryTest
    {
        //private ISocialRelationshipHistoryDataProvider socialRelationhshipRuleEngine;

        //private long _defaultContactId = 1;
        //private long _defaultDeleteId = 1;
        //private SocialRelationshipHistoryController socialRelationshipController;
        //List<SocialRelationshipHistoryModel> socialRelationships;

        [TestInitialize]
        public void Initialize()
        {
        }

        ///// <summary>
        ///// Mock initialization
        ///// </summary>
        //public void Mock_SocialRelationshipHistory()
        //{
        //    var mock = new Mock<ISocialRelationshipHistoryDataProvider>();
        //    socialRelationhshipRuleEngine = mock.Object;

        //    socialRelationshipController = new SocialRelationshipHistoryController(socialRelationhshipRuleEngine);

        //    socialRelationships = new List<SocialRelationshipHistoryModel>();
        //    socialRelationships.Add(new SocialRelationshipHistoryModel()
        //    {
        //        SocialRelationshipDetailID = 1,
        //        SocialRelationShipID = 10003,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "No Children",
        //        RelationShipHistory = "No RelationShip",
        //        FamilyHistory = "Living with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = 1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    });

        //    var socialRelationshipResponse = new Response<SocialRelationshipHistoryModel>()
        //    {
        //        DataItems = socialRelationships
        //    };

        //    //Get Social Relationship
        //    mock.Setup(r => r.GetSocialRelationHistoryByContact(It.IsAny<long>()))
        //        .Callback((long id) => { socialRelationshipResponse.DataItems = socialRelationships.Where(sr => sr.ContactID == id).ToList(); })
        //        .Returns(socialRelationshipResponse);

        //    //Add Social Relationship
        //    mock.Setup(r => r.AddSocialRelationHistory(It.IsAny<SocialRelationshipHistoryModel>()))
        //        .Callback((SocialRelationshipHistoryModel socialRelationModel) => { if (socialRelationModel.ContactID > 0) socialRelationships.Add(socialRelationModel); })
        //        .Returns(socialRelationshipResponse);

        //    //Update Social Relationship
        //    mock.Setup(r => r.UpdateSocialRelationHistory(It.IsAny<SocialRelationshipHistoryModel>()))
        //        .Callback((SocialRelationshipHistoryModel socialRelationModel) => { if (socialRelationModel.SocialRelationshipDetailID > 0) { 
        //                                    socialRelationships.Remove(socialRelationships.Find(sr => sr.SocialRelationshipDetailID == socialRelationModel.SocialRelationshipDetailID)); socialRelationships.Add(socialRelationModel); } })
        //        .Returns(socialRelationshipResponse);

        //    //Delete Social Relationship
        //    var deleteResponse = new Response<SocialRelationshipHistoryModel>();
        //    mock.Setup(r => r.DeleteSocialRelationHistory(It.IsAny<long>()))
        //        .Callback((long id) => socialRelationships.Remove(socialRelationships.Find(sr => sr.SocialRelationshipDetailID == id)))
        //        .Returns(deleteResponse);

        //    //Get Social Relationship details
        //    mock.Setup(r => r.GetSocialRelationshipDetail(It.IsAny<long>()))
        //        .Callback((long id) => { socialRelationshipResponse.DataItems = socialRelationships.Where(sr => sr.ContactID == id).ToList(); })
        //        .Returns(socialRelationshipResponse);
        //}

        ///// <summary>
        ///// The test method for GetSocialRelationHistoryByContact success
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationHistoryByContact_Success()
        //{
        //    //Arrange
        //    Mock_SocialRelationshipHistory();

        //    //Act
        //    var getSocialRelationshipResult = socialRelationshipController.GetSocialRelationHistoryByContact(_defaultContactId);
        //    var response = getSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response Value can't be null");
        //    Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
        //    Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one record must exist.");
        //}

        ///// <summary>
        ///// The test method for GetSocialRelationHistoryByContact failure
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationHistoryByContact_Failure()
        //{
        //    //Arrange
        //    Mock_SocialRelationshipHistory();
        //    //Act
        //    var getSocialRelationshipResult = socialRelationshipController.GetSocialRelationHistoryByContact(-1);
        //    var response = getSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response Value can't be null");
        //    Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
        //    Assert.IsTrue(response.Value.DataItems.Count() == 0, "Record exist for invalid data.");
        //}

        ///// <summary>
        ///// The test method for AddSocialRelationHistory success
        ///// </summary>
        //[TestMethod]
        //public void AddSocialRelationHistory_Success()
        //{
        //    //Arrange
        //    Mock_SocialRelationshipHistory();
        //    var addSocialRelationship = new SocialRelationshipHistoryModel
        //    {
        //        SocialRelationshipDetailID = 2,
        //        SocialRelationShipID = 10003,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "First Children",
        //        RelationShipHistory = "First RelationShip",
        //        FamilyHistory = "Enjoying with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = 1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    };
        //    //Act
        //    var addSocialRelationshipResult = socialRelationshipController.AddSocialRelationHistory(addSocialRelationship);
        //    var response = addSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response value can't be null");
        //    Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
        //    Assert.IsTrue(response.Value.DataItems.Count == 2, "Social Relationship Details could not be saved.");
        //}

        ///// <summary>
        ///// The test method for AddSocialRelationHistory failure
        ///// </summary>
        //[TestMethod]
        //public void AddSocialRelationHistory_Failure()
        //{
        //    //Arrange
        //    Mock_SocialRelationshipHistory();

        //    var addSocialRelationshipFailure = new SocialRelationshipHistoryModel
        //    {
        //        SocialRelationshipDetailID = -1,
        //        SocialRelationShipID = 1,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "First Children",
        //        RelationShipHistory = "First RelationShip",
        //        FamilyHistory = "Enjoying with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = -1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    };
        //    //Act
        //    var addSocialRelationshipResult = socialRelationshipController.AddSocialRelationHistory(addSocialRelationshipFailure);
        //    var response = addSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response value can't be null");
        //    Assert.IsNotNull(response.Value.DataItems, "Response value can't be null");
        //    Assert.IsTrue(response.Value.DataItems.Count == 1, "Social Relationship details saved for invalid record.");
        //}

        ///// <summary>
        ///// The test method for UpdateSocialRelationHistory success
        ///// </summary>
        //[TestMethod]
        //public void UpdateSocialRelationHistory_Success()
        //{
        //    //Arrange
        //    Mock_SocialRelationshipHistory();
        //    var updateSocialRelationship = new SocialRelationshipHistoryModel
        //    {
        //        SocialRelationshipDetailID = 1,
        //        SocialRelationShipID = 10003,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "Children History Updated",
        //        RelationShipHistory = "First RelationShip",
        //        FamilyHistory = "Enjoying with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = 1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    };
        //    //Act
        //    var updateSocialRelationshipResult = socialRelationshipController.UpdateSocialRelationHistory(updateSocialRelationship);
        //    var response = updateSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response value can't be null");
        //    Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
        //    Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
        //    Assert.IsTrue(response.Value.DataItems[0].ChildhoodHistory == "Children History Updated", "Social Relationship details could not be updated.");
        //}

        ///// <summary>
        ///// The test method for UpdateSocialRelationHistory failure
        ///// </summary>
        //[TestMethod]
        //public void UpdateSocialRelationHistory_Failure()
        //{
        //    //Arrange
        //    Mock_SocialRelationshipHistory();
        //    var updateSocialRelationship = new SocialRelationshipHistoryModel
        //    {
        //        SocialRelationshipDetailID = -1,
        //        SocialRelationShipID = 10003,
        //        FamilyRelationshipID = 1,
        //        ChildhoodHistory = "Children History Updated",
        //        RelationShipHistory = "First RelationShip",
        //        FamilyHistory = "Enjoying with Family.",
        //        IsDeceased = false,
        //        IsInvolved = true,
        //        FirstName = "Peter",
        //        LastName = "Parker",
        //        RelationshipTypeID = 1,
        //        ReviewedNoChanges = false,
        //        ContactID = -1,
        //        EncounterID = null,
        //        TakenBy = 1,
        //        TakenTime = DateTime.Now,
        //        ForceRollback = true
        //    };
        //    //Act
        //    var updateSocialRelationshipResult = socialRelationshipController.UpdateSocialRelationHistory(updateSocialRelationship);
        //    var response = updateSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response value can't be null");
        //    Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
        //    Assert.IsTrue(response.Value.DataItems.Count > 0, "Response must return data items");
        //    Assert.IsTrue(response.Value.DataItems[0].ChildhoodHistory != "Children History Updated", "Social Relationship updated for invalid data.");
        //}

        ///// <summary>
        ///// The test method for DeleteSocialRelationHistory success
        ///// </summary>
        //[TestMethod]
        //public void DeleteSocialRelationHistory_Success()
        //{
        //    //Arrange
        //    Mock_SocialRelationshipHistory();
        //    //Act
        //    var deleteSocialRelationshipResult = socialRelationshipController.DeleteSocialRelationHistory(_defaultDeleteId);
        //    var response = deleteSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsTrue(socialRelationships.Count() == 0, "Social Relationship details could not be deleted.");
        //}

        ///// <summary>
        ///// The test method for DeleteSocialRelationHistory failure
        ///// </summary>
        //[TestMethod]
        //public void DeleteSocialRelationHistory_Failure()
        //{
        //    //Arrange
        //    Mock_SocialRelationshipHistory();
        //    //Act
        //    var deleteSocialRelationshipResult = socialRelationshipController.DeleteSocialRelationHistory(-1);
        //    var response = deleteSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response can't be null");
        //    Assert.IsTrue(socialRelationships.Count() > 0, "Social Relationship details deleted for invalid record.");
        //}

        ///// <summary>
        ///// The test method for GetSocialRelationshipDetail success
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationshipDetail_Success()
        //{
        //    //Arrange
        //    var socialRelationshipID = 1;
        //    Mock_SocialRelationshipHistory();

        //    //Act
        //    var getSocialRelationshipResult = socialRelationshipController.GetSocialRelationshipDetail(socialRelationshipID);
        //    var response = getSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response Value can't be null");
        //    Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
        //    Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one record must exist.");
        //}

        ///// <summary>
        ///// The test method for GetSocialRelationshipDetail failure
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationshipDetail_Failure()
        //{
        //    //Arrange
        //    var socialRelationshipID = -1;
        //    Mock_SocialRelationshipHistory();
        //    //Act
        //    var getSocialRelationshipResult = socialRelationshipController.GetSocialRelationshipDetail(socialRelationshipID);
        //    var response = getSocialRelationshipResult as HttpResult<Response<SocialRelationshipHistoryModel>>;

        //    //Assert
        //    Assert.IsNotNull(response, "Response can't be null");
        //    Assert.IsNotNull(response.Value, "Response Value can't be null");
        //    Assert.IsNotNull(response.Value.DataItems, "DataItems can't be null");
        //    Assert.IsTrue(response.Value.DataItems.Count() == 0, "Record exist for invalid data.");
        //}
    }
}
