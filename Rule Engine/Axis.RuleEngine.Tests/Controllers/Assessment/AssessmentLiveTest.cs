using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Collections.Specialized;
using Axis.Model.Common.Assessment;
using Axis.Model.Common;
using System.Configuration;
using System.Collections.Generic;

namespace Axis.RuleEngine.Tests.Controllers.Assessment
{
    [TestClass]
    public class AssessmentLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "assessment/";
        private long assessmentID = 1;
        private long sectionID = 1;
        private long contactID = 1;
        private long responseID = 1;

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetAssessment_Success()
        {
            // Arrange
            var url = baseRoute + "getAssessment";

            var param = new NameValueCollection();
            param.Add("assessmentID", assessmentID.ToString());

            // Act
            var response = communicationManager.Get<Response<AssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment must exists.");
        }

        [TestMethod]
        public void GetAssessment_Failed()
        {
            // Arrange
            var url = baseRoute + "getAssessment";

            var param = new NameValueCollection();
            param.Add("assessmentID", "-1");

            // Act
            var response = communicationManager.Get<Response<AssessmentModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentSections_Success()
        {
            var url = baseRoute + "getAssessmentSections";

            var param = new NameValueCollection();
            param.Add("assessmentID", assessmentID.ToString());

            var response = communicationManager.Get<Response<AssessmentSectionsModel>>(param, url);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment section must exists.");
        }

        [TestMethod]
        public void GetAssessmentSections_Failed()
        {
            // Arrange
            var url = baseRoute + "getAssessmentSections";

            var param = new NameValueCollection();
            param.Add("assessmentID", "0");

            // Act
            var response = communicationManager.Get<Response<AssessmentSectionsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment section should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentQuestions_Success()
        {
            // Arrange
            var url = baseRoute + "getAssessmentQuestions";

            var param = new NameValueCollection();
            param.Add("sectionID", sectionID.ToString());

            // Act
            var response = communicationManager.Get<Response<AssessmentQuestionModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment question must exists.");
        }

        [TestMethod]
        public void GetAssessmentQuestions_Failed()
        {
            // Arrange
            var url = baseRoute + "getAssessmentQuestions";

            var param = new NameValueCollection();
            param.Add("sectionID", "0");

            // Act
            var response = communicationManager.Get<Response<AssessmentSectionsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment question should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentResponses_Success()
        {
            // Arrange
            var url = baseRoute + "getAssessmentResponses";

            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            param.Add("assessmentID", assessmentID.ToString());

            // Act
            var response = communicationManager.Get<Response<AssessmentResponseModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment responses must exists.");
        }

        [TestMethod]
        public void GetAssessmentResponses_Failed()
        {
            // Arrange
            var url = baseRoute + "getAssessmentResponses";

            var param = new NameValueCollection();
            param.Add("contactID", "0");
            param.Add("assessmentID", "0");

            // Act
            var response = communicationManager.Get<Response<AssessmentResponseModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment responses should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentResponse_Success()
        {
            // Arrange
            var url = baseRoute + "getAssessmentResponse";

            var param = new NameValueCollection();
            param.Add("responseID", responseID.ToString());

            // Act
            var response = communicationManager.Get<Response<AssessmentResponseModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment response must exists.");
        }

        [TestMethod]
        public void GetAssessmentResponse_Failed()
        {
            // Arrange
            var url = baseRoute + "getAssessmentResponse";

            var param = new NameValueCollection();
            param.Add("responseID", "0");

            // Act
            var response = communicationManager.Get<Response<AssessmentResponseModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment response should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentResponseDetails_Success()
        {
            // Arrange
            var url = baseRoute + "getAssessmentResponseDetails";

            var param = new NameValueCollection();
            param.Add("responseID", responseID.ToString());
            param.Add("sectionID", sectionID.ToString());

            // Act
            var response = communicationManager.Get<Response<AssessmentResponseDetailsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment response details must exists.");
        }

        [TestMethod]
        public void GetAssessmentResponseDetails_Failed()
        {
            // Arrange
            var url = baseRoute + "getAssessmentResponseDetails";

            var param = new NameValueCollection();
            param.Add("responseID", "0");
            param.Add("sectionID", "0");

            // Act
            var response = communicationManager.Get<Response<AssessmentResponseDetailsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment response details should not exists for this test case.");
        }

        [TestMethod]
        public void AddAssessmentResponse_Success()
        {
            // Arrange
            var url = baseRoute + "addAssessmentResponse";

            var assessmentResponse = new AssessmentResponseModel
            {
                ResponseID = 0,
                AssessmentID = 2,
                ContactID = 2,
                EnterDate = DateTime.Now,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<AssessmentResponseModel, Response<AssessmentResponseModel>>(assessmentResponse, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Assessment Response could not be created.");
        }

        [TestMethod]
        public void AddAssessmentResponse_Failed()
        {
            // Arrange
            var url = baseRoute + "addAssessmentResponse";

            //Add Assessment Response
            var assessmentResponse = new AssessmentResponseModel
            {
                ResponseID = -1,
                AssessmentID = 0,
                ContactID = 0,
                EnterDate = DateTime.Now,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<AssessmentResponseModel, Response<AssessmentResponseModel>>(assessmentResponse, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add assessment response expected to be failed.");
        }

        [TestMethod]
        public void AddAssessmentResponseDetails_Success()
        {
            // Arrange
            var url = baseRoute + "saveAssessmentResponseDetails";

            //Add Assessment Response Details
            var assessmentResponseDetails = new List<AssessmentResponseDetailsModel>();
            assessmentResponseDetails.Add(new AssessmentResponseDetailsModel
            {
                ResponseDetailsID = 0,
                ResponseID = 1,
                QuestionID = 10,
                OptionsID = 2,
                Rating = 1
            });

            // Act
            var response = communicationManager.Post<List<AssessmentResponseDetailsModel>, Response<AssessmentResponseDetailsModel>>(assessmentResponseDetails, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Assessment Response Details could not be created.");
        }

        [TestMethod]
        public void AddAssessmentResponseDetails_Failed()
        {
            // Arrange
            var url = baseRoute + "saveAssessmentResponseDetails";

            //Add Assessment Response Details
            var assessmentResponseDetails = new List<AssessmentResponseDetailsModel>();
            assessmentResponseDetails.Add(new AssessmentResponseDetailsModel
            {
                ResponseDetailsID = -1,
                ResponseID = -1,
                QuestionID = 10,
                OptionsID = 2,
                Rating = 1
            });

            // Act
            var response = communicationManager.Post<List<AssessmentResponseDetailsModel>, Response<AssessmentResponseDetailsModel>>(assessmentResponseDetails, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add assessment response details expected to be failed.");
        }

        [TestMethod]
        public void UpdateAssessmentResponse_Success()
        {
            // Arrange
            var url = baseRoute + "updateAssessmentResponse";

            var assessmentResponse = new AssessmentResponseModel
            {
                ResponseID = 1,
                AssessmentID = 2,
                ContactID = 2,
                EnterDate = DateTime.Now,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<AssessmentResponseModel, Response<AssessmentResponseModel>>(assessmentResponse, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Assessment response could not updated.");
        }

        [TestMethod]
        public void UpdateAssessmentResponse_Failed()
        {
            // Arrange
            var url = baseRoute + "updateAssessmentResponse";

            var assessmentResponse = new AssessmentResponseModel
            {
                ResponseID = -1,
                AssessmentID = 0,
                ContactID = 0,
                EnterDate = DateTime.Now,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<AssessmentResponseModel, Response<AssessmentResponseModel>>(assessmentResponse, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Update assessment response expected to be failed.");
        }

        [TestMethod]
        public void UpdateAssessmentResponseDetails_Success()
        {
            // Arrange
            var url = baseRoute + "saveAssessmentResponseDetails";

            var assessmentResponseDetails = new List<AssessmentResponseDetailsModel>();
            assessmentResponseDetails.Add(new AssessmentResponseDetailsModel
            {
                ResponseDetailsID = 1,
                ResponseID = 1,
                QuestionID = 10,
                OptionsID = 2,
                Rating = 1
            });

            // Act
            var response = communicationManager.Post<List<AssessmentResponseDetailsModel>, Response<AssessmentResponseDetailsModel>>(assessmentResponseDetails, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Assessment response details could not updated.");
        }

        [TestMethod]
        public void UpdateAssessmentResponseDetails_Failed()
        {
            // Arrange
            var url = baseRoute + "saveAssessmentResponseDetails";

            var assessmentResponseDetails = new List<AssessmentResponseDetailsModel>();
            assessmentResponseDetails.Add(new AssessmentResponseDetailsModel
            {
                ResponseDetailsID = -1,
                ResponseID = -1,
                QuestionID = 10,
                OptionsID = 2,
                Rating = 1
            });

            // Act
            var response = communicationManager.Post<List<AssessmentResponseDetailsModel>, Response<AssessmentResponseDetailsModel>>(assessmentResponseDetails, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Update assessment response details expected to be failed.");
        }
    }
}
