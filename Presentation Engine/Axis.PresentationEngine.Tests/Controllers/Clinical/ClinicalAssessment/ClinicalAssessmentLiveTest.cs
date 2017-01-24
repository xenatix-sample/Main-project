using Axis.Model.Common;
using Axis.Plugins.Clinical.ApiControllers;
using Axis.Plugins.Clinical.Models;
using Axis.Plugins.Clinical.Models.Assessment;
using Axis.Plugins.Clinical.Repository.Assessment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers.Clinical
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ClinicalAssessmentLiveTest
    {

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactID = 1;
        /// <summary>
        /// The controller
        /// </summary>
        private ClinicalAssessmentController controller = null;

        [TestInitialize]
        public void Initialize() {

            //Arrange
            controller = new ClinicalAssessmentController(new ClinicalAssessmentRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        
        }

        
        /// <summary>
        /// Success test case  of GetClinicalAssessments
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void GetClinicalAssessments_Success()
        {
            //Arrange
            long clinicalAssessmentID = 10;

            
            // Act
            var response = controller.GetClinicalAssessments(clinicalAssessmentID).Result;
            var count = response.DataItems.Count;
            // Assert
            Assert.IsTrue(count > 0, "Atleast one clinical assessment must exist.");

        }

        /// <summary>
        /// Failed test case  of GetClinicalAssessments
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void GetClinicalAssessments_Failed()
        {
            //Arrange
            long clinicalAssessmentID = -1;

            // Act
            var response = controller.GetClinicalAssessments(clinicalAssessmentID).Result;
            var count = response.DataItems.Count;

            // Assert
            Assert.IsTrue(count == 0, "Atleast one clinical assessment exists.");

        }

        /// <summary>
        /// Success test case of GetClinicalAssessmentsByContact
        /// </summary>
        [TestMethod]
        public void GetClinicalAssessmentsByContact_Success()
        {           

            //Act
            var response = controller.GetClinicalAssessmentsByContact(contactID).Result;
            var count = response.DataItems.Count;

            //Assert
            Assert.IsTrue(count > 0, "Atleast one clinical assessment must exist");
        }

        /// <summary>
        /// Failed test case of GetClinicalAssessmentsByContact
        /// </summary>
        [TestMethod]
        public void GetClinicalAssessmentsByContact_Failed()
        {
            //Arrange
            contactID = -1;
            

            //Act
            var response = controller.GetClinicalAssessmentsByContact(contactID).Result;
            var count = response.DataItems.Count;

            //Assert
            Assert.IsTrue(count ==0, "Atleast one clinical assessment exist");
        }


        /// <summary>
        /// Success test case for AddAssessment
       /// </summary>
       [TestMethod]
        public void AddAssessment_Success()
        {
           //Arrange
            var assessment = new ClinicalAssessmentViewModel{
            ClinicalAssessmentID=0,
            AssessmentID=4,
            ContactID=1,
            TakenTime = DateTime.UtcNow,
            TakenBy=1,
            ResponseID=1     
            };

            // Act
            var response = controller.AddAssessment(assessment);
           
            // Assert
            Assert.IsTrue(response != null, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Atleast one clinical assessment must exists.");

            
        }

       /// <summary>
       /// Failure test case for AddAssessment
       /// </summary>
       [TestMethod]
       public void AddAssessment_Failed()
       {
           //Arrange
           var assessment = new ClinicalAssessmentViewModel
           {
               AssessmentID = 0,
               ContactID = -1,
               TakenTime = DateTime.UtcNow,
               TakenBy = 1,
               ResponseID = 1
           };

           // Act
           var response = controller.AddAssessment(assessment);

           //Assert
           Assert.IsTrue(response != null, "Response can't be null");
           Assert.IsTrue(response.ResultCode != 0, "Add assessment expected to be failed.");


       }

       /// <summary>
       /// Success test case for UpdateAssessment
       /// </summary>
       [TestMethod]
       public void UpdateAssessment_Success()
       {
           // Arrange
           var assessment = new ClinicalAssessmentViewModel
           {
               ClinicalAssessmentID=2,
               AssessmentID = 4,
               ContactID = 1,
               TakenTime = DateTime.UtcNow,
               TakenBy = 1,
               ResponseID = 1
           };

           // Act
           var response = controller.UpdateAssessment(assessment);

           // Assert
           Assert.IsTrue(response != null, "Response can't be null");
           Assert.IsTrue(response.ResultCode == 0, "Assessment could not be updated.");
       }


       /// <summary>
       /// Failed test case for UpdateAssessment
       /// </summary>
       [TestMethod]
       public void UpdateAssessment_Failed()
       {
           // Arrange
           var assessment = new ClinicalAssessmentViewModel
           {
               ClinicalAssessmentID = -1,
               AssessmentID = 4,
               ContactID = -1,
               TakenTime = DateTime.UtcNow,
               TakenBy = 1,
               ResponseID = 1
           };

           // Act
           var response = controller.UpdateAssessment(assessment);

           //Assert
           Assert.IsTrue(response != null, "Response can't be null");
           Assert.IsTrue(response.ResultCode != 0, "Update assessment expected to be failed.");

       }

        /// <summary>
        /// Success test case for DeleteAssessment
        /// </summary>
       [TestMethod]
       public void DeleteAssessment_Success()
       {
           //Arrange
           var clinicalAssessmentId = 2;

           //Act
           var response = controller.DeleteAssessment(clinicalAssessmentId, DateTime.UtcNow);

           //Assert
           Assert.IsTrue(response.RowAffected > 2, "Clinical assessment could not be deleted.");

       }

       /// <summary>
       /// Failed test case for DeleteAssessment
       /// </summary>
       [TestMethod]
       public void DeleteAssessment_Failed()
       {

           //Arrange
           var clinicalAssessmentId = 1;

           //Act
           var response = controller.DeleteAssessment(clinicalAssessmentId, DateTime.UtcNow);

           //Assert
           Assert.IsTrue(response.RowAffected == 2, "Clinical assessment deleted.");

       }
    }
}
