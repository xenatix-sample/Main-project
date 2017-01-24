using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.ApiControllers;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository.FinancialAssessment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers.Registration.FinancialAssessment
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class FinancialAssessmentTest
    {
        #region Variables

        
        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;
        private long financialAssessmentID = 1;
        private long financialAssessmentDetailID = 1;
        private long financialAssessmentDetailFailedID = 0;
        

        /// <summary>
        /// The token
        /// </summary>
        private string token = ConfigurationManager.AppSettings["UnitTestToken"];

        /// <summary>
        /// The financial assessment view model
        /// </summary>
        private FinancialAssessmentViewModel financialAssessmentViewModel = null;
        private FinancialAssessmentDetailsModel financialAssessmentDetailsModel = null;

        #endregion

        #region Private Methods

        /// <summary>
        /// Financials the assessment_ success_data.
        /// </summary>
        private void FinancialAssessment_Success_data()
        {
            var financialAssessmentDetails = new List<FinancialAssessmentDetailsModel>();

            financialAssessmentDetailsModel = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 1,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 17,
                CategoryID = 1
            };
            financialAssessmentDetails.Add(financialAssessmentDetailsModel);
            financialAssessmentViewModel = new FinancialAssessmentViewModel()
            {
                ContactID = 1,
                FinancialAssessmentID = 1,
                AssessmentDate = DateTime.Now,
                TotalIncome = 300000,
                TotalExpenses = 30000,
                AdjustedGrossIncome = 5000,
                TotalExtraOrdinaryExpenses = 100,
                TotalOther = 100,
                ExpirationDate = DateTime.Now,
                ExpirationReasonID = 0,
                FamilySize = 2
            };
        }

        /// <summary>
        /// Financials the assessment_ failed_data.
        /// </summary>
        private void FinancialAssessment_Failed_data()
        {
            var financialAssessmentDetails = new List<FinancialAssessmentDetailsModel>();
            financialAssessmentDetailsModel = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 0,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 17,
                CategoryID = 1
            };
            financialAssessmentDetails.Add(financialAssessmentDetailsModel);
            financialAssessmentViewModel = new FinancialAssessmentViewModel()
            {
                ContactID = 0,
                FinancialAssessmentID = 1,
                AssessmentDate = DateTime.Now,
                TotalIncome = 300000,
                TotalExpenses = 30000,
                AdjustedGrossIncome = 5000,
                TotalExtraOrdinaryExpenses = 100,
                TotalOther = 100,
                ExpirationDate = DateTime.Now,
                ExpirationReasonID = 0,
                FamilySize = 2
            };
        }

        #endregion

        #region Public Method


        /// <summary>
        /// Gets the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void GetFinancialAssessment_Success()
        {
            // Arrange
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act
            var modelResponse = controller.GetFinancialAssessment(contactId, financialAssessmentID).Result;

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsNotNull(modelResponse.DataItems);
            Assert.IsTrue(modelResponse.DataItems.Count > 0);
        }

        /// <summary>
        /// Gets the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void GetFinancialAssessment_Failed()
        {
            // Arrange
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act
            var modelResponse = controller.GetFinancialAssessment(0, 0).Result;
            
            // Assert
            Assert.IsNull(modelResponse);
        }

        /// <summary>
        /// Saves the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void SaveFinancialAssessment_Success()
        {
            // Arrange
            FinancialAssessment_Success_data();
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act

            var modelResponse = controller.AddFinancialAssessment(financialAssessmentViewModel);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected > 0);
        }

        /// <summary>
        /// Saves the financial assessment details success.
        /// </summary>
        [TestMethod]
        public void SaveFinancialAssessmentDetails_Success()
        {
            // Arrange
            FinancialAssessment_Success_data();
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act

            var modelResponse = controller.AddFinancialAssessmentDetails(financialAssessmentDetailsModel);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected > 0);
        }

        /// <summary>
        /// Saves the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void SaveFinancialAssessment_Failed()
        {
            // Arrange
            FinancialAssessment_Failed_data();
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act

            var modelResponse = controller.AddFinancialAssessment(financialAssessmentViewModel);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected == 0);
        }

        /// <summary>
        /// Saves the financial assessment details failed.
        /// </summary>
        [TestMethod]
        public void SaveFinancialAssessmentDetails_Failed()
        {
            // Arrange
            FinancialAssessment_Failed_data();
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act

            var modelResponse = controller.AddFinancialAssessmentDetails(financialAssessmentDetailsModel);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected == 0);
        }

        /// <summary>
        /// Updates the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void updateFinancialAssessment_Success()
        {
            // Arrange
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act
            financialAssessmentViewModel = new FinancialAssessmentViewModel()
            {
                ContactID = 1,
                FinancialAssessmentID = 1,
                AssessmentDate = DateTime.Now,
                TotalIncome = 300000,
                TotalExpenses = 30000,
                AdjustedGrossIncome = 5000,
                TotalExtraOrdinaryExpenses = 100,
                TotalOther = 100,
                ExpirationDate = DateTime.Now,
                ExpirationReasonID = 0,
                FamilySize = 2
            };

            var modelResponse = controller.UpdateFinancialAssessment(financialAssessmentViewModel);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected > 0);
        }

        /// <summary>
        /// Updates the financial assessment details success.
        /// </summary>
        [TestMethod]
        public void updateFinancialAssessmentDetails_Success()
        {
            // Arrange
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act
            financialAssessmentDetailsModel = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 1,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 17,
                CategoryID = 1
            };

            var modelResponse = controller.UpdateFinancialAssessmentDetails(financialAssessmentDetailsModel);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected > 0);
        }

        /// <summary>
        /// Updates the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void updateFinancialAssessment_Failed()
        {
            // Arrange
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act
            financialAssessmentViewModel = new FinancialAssessmentViewModel()
            {
                ContactID = 0,
                FinancialAssessmentID = 1,
                AssessmentDate = DateTime.Now,
                TotalIncome = 300000,
                TotalExpenses = 30000,
                AdjustedGrossIncome = 5000,
                TotalExtraOrdinaryExpenses = 100,
                TotalOther = 100,
                ExpirationDate = DateTime.Now,
                ExpirationReasonID = 0,
                FamilySize = 2
            };

            var modelResponse = controller.UpdateFinancialAssessment(financialAssessmentViewModel);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected == 0);

        }

        /// <summary>
        /// Updates the financial assessment details failed.
        /// </summary>
        [TestMethod]
        public void updateFinancialAssessmentDetails_Failed()
        {
            // Arrange
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act
            financialAssessmentDetailsModel = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 0,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 17,
                CategoryID = 1
            };


            var modelResponse = controller.UpdateFinancialAssessmentDetails(financialAssessmentDetailsModel);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected == 0);

        }

        /// <summary>
        /// Delete the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void DeleteFinancialAssessment_Success()
        {
            // Arrange
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));
           
            // Act

            var modelResponse = controller.DeleteFinancialAssessmentDetail(financialAssessmentDetailID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected > 0);
        }

        /// <summary>
        /// Delete the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void DeleteFinancialAssessment_Failed()
        {
            // Arrange
            var controller = new FinancialAssessmentController(new FinancialAssessmentRepository(token));

            // Act
            var modelResponse = controller.DeleteFinancialAssessmentDetail(financialAssessmentDetailFailedID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(modelResponse);
            Assert.IsTrue(modelResponse.RowAffected == 0);

        }

        #endregion
    }
}