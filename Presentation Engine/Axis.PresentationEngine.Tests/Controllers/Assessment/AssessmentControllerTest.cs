using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.PresentationEngine.Areas.Assessment.ApiControllers;
using Axis.PresentationEngine.Areas.Assessment.Repository;
using System.Configuration;
using System.Web.Mvc;
using Axis.PresentationEngine.Areas.Assessment.Models;
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Tests.Controllers.Assessment
{
    [TestClass]
    public class AssessmentControllerTest
    {
        private long assessmentID = 1;
        private long sectionID = 1;
        private long contactID = 1;
        private long responseID = 1;
        private AssessmentController controller = null;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            controller = new AssessmentController(new AssessmentRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetAssessment_Success()
        {
            // Act
            var response = controller.GetAssessment(assessmentID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment must exists.");
        }

        [TestMethod]
        public void GetAssessment_Failed()
        {
            // Act
            var response = controller.GetAssessment(0);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentSections_Success()
        {
            // Act
            var response = controller.GetAssessmentSections(assessmentID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment section must exists.");
        }

        [TestMethod]
        public void GetAssessmentSections_Failed()
        {
            // Act
            var response = controller.GetAssessmentSections(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment section should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentQuestions_Success()
        {
            // Act
            var response = controller.GetAssessmentQuestions(sectionID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment question must exists.");
        }

        [TestMethod]
        public void GetAssessmentQuestions_Failed()
        {
            // Act
            var response = controller.GetAssessmentQuestions(0);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment question should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentResponses_Success()
        {
            // Act
            var response = controller.GetAssessmentResponses(contactID, assessmentID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment responses must exists.");
        }

        [TestMethod]
        public void GetAssessmentResponses_Failed()
        {
            // Act
            var response = controller.GetAssessmentResponses(-1, -1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment responses should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentResponse_Success()
        {
            // Act
            var response = controller.GetAssessmentResponse(responseID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment response must exists.");
        }

        [TestMethod]
        public void GetAssessmentResponse_Failed()
        {
            // Act
            var response = controller.GetAssessmentResponse(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment response should not exists for this test case.");
        }

        [TestMethod]
        public void GetAssessmentResponseDetails_Success()
        {
            // Act
            var response = controller.GetAssessmentResponseDetails(responseID, sectionID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one assessment response details must exists.");
        }

        [TestMethod]
        public void GetAssessmentResponseDetails_Failed()
        {
            // Act
            var response = controller.GetAssessmentResponseDetails(-1, 0);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Assessment response details should not exists for this test case.");
        }

        [TestMethod]
        public void AddAssessmentResponse_Success()
        {
            // Act
            var assessmentResponse = new AssessmentResponseViewModel
            {
                ResponseID = 0,
                AssessmentID = 2,
                ContactID = 2,
                EnterDate = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddAssessmentResponse(assessmentResponse);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Assessment Response could not be created.");
        }

        [TestMethod]
        public void AddAssessmentResponse_Failed()
        {
            // Act
            var assessmentResponse = new AssessmentResponseViewModel
            {
                AssessmentID = -1,
                ResponseID = -1,
                ContactID = -1,
                EnterDate = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddAssessmentResponse(assessmentResponse);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add assessment response expected to be failed.");
        }

        [TestMethod]
        public void AddAssessmentResponseDetails_Success()
        {
            // Act
            //var assessmentResponseDetails = new List<AssessmentResponseDetailsViewModel>();
            //assessmentResponseDetails.Add(new AssessmentResponseDetailsViewModel
            //{
            //    ResponseDetailsID = 0,
            //    ResponseID = 1,
            //    QuestionID = 10,
            //    OptionsID =  2,
            //    Rating = 1
            //});

            //var response = controller.SaveAssessmentResponseDetails(Newtonsoft.Json.Linq.JObject.FromObject(
            //    new
            //    {
            //        ResponseID = 1,
            //        assessmentResponseDetails = assessmentResponseDetails.ToArray()
            //    }
            //    ));

            //// Assert
            //Assert.IsTrue(response != null, "Response can't be null");
            //Assert.IsTrue(response.ResultCode == 0, "Assessment Response Details could not be created.");
        }

        [TestMethod]
        public void AddAssessmentResponseDetails_Failed()
        {
            // Act
            //var assessmentResponseDetails = new List<AssessmentResponseDetailsViewModel>();
            //assessmentResponseDetails.Add(new AssessmentResponseDetailsViewModel
            //{
            //    ResponseDetailsID = -1,
            //    ResponseID = -1,
            //    QuestionID = 10,
            //    OptionsID = 2,
            //    Rating = 1
            //});

            //var response = controller.SaveAssessmentResponseDetails(
            //            Newtonsoft.Json.Linq.JObject.FromObject(new
            //            {
            //                ResponseID = -1,
            //                assessmentResponseDetails = assessmentResponseDetails.ToArray()
            //            }
            //        ));

            //// Assert
            //Assert.IsTrue(response != null, "Response can't be null");
            //Assert.IsTrue(response.ResultCode != 0, "Add assessment response details expected to be failed.");
        }

        [TestMethod]
        public void UpdateAssessmentResponse_Success()
        {
            // Act
            var assessmentResponse = new AssessmentResponseViewModel
            {
                ResponseID = 1,
                AssessmentID = 2,
                ContactID = 2,
                EnterDate = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateAssessmentResponse(assessmentResponse);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Assessment response could not updated.");
        }

        [TestMethod]
        public void UpdateAssessmentResponse_Failed()
        {
            // Act
            var assessmentResponse = new AssessmentResponseViewModel
            {
                ResponseID = -1,
                AssessmentID = 0,
                ContactID = 0,
                EnterDate = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateAssessmentResponse(assessmentResponse);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Update assessment response expected to be failed.");
        }

        [TestMethod]
        public void UpdateAssessmentResponseDetails_Success()
        {
            // Act
            //var assessmentResponseDetails = new List<AssessmentResponseDetailsViewModel>();
            //assessmentResponseDetails.Add(new AssessmentResponseDetailsViewModel
            //{
            //    ResponseDetailsID=1,
            //    ResponseID=1,
            //    QuestionID = 10,
            //    OptionsID = 2,
            //    Rating = 1
            //});

            //var response = controller.SaveAssessmentResponseDetails(Newtonsoft.Json.Linq.JObject.FromObject(new
            //        {
            //            ResponseID = 1,
            //            assessmentResponseDetails = assessmentResponseDetails.ToArray()
            //        }
            //    ));

            //// Assert
            //Assert.IsTrue(response != null, "Response can't be null");
            //Assert.IsTrue(response.ResultCode == 0, "Assessment response details could not updated.");
        }

        [TestMethod]
        public void UpdateAssessmentResponseDetails_Failed()
        {
            // Act
            //var assessmentResponseDetails = new List<AssessmentResponseDetailsViewModel>();
            //assessmentResponseDetails.Add(new AssessmentResponseDetailsViewModel
            //{
            //    ResponseDetailsID = -1,
            //    ResponseID = -1,
            //    QuestionID = 10,
            //    OptionsID = 2,
            //    Rating = 1
            //});

            //var response = controller.SaveAssessmentResponseDetails(Newtonsoft.Json.Linq.JObject.FromObject(new
            //        {
            //            ResponseID = -1,
            //            assessmentResponseDetails = assessmentResponseDetails.ToArray()
            //        }
            //    ));

            //// Assert
            //Assert.IsTrue(response != null, "Response can't be null");
            //Assert.IsTrue(response.ResultCode != 0, "Update assessment response details expected to be failed.");
        }
    }
}
