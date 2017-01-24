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

namespace Axis.RuleEngine.Tests.Controllers.Registration.FinancialAssessment
{
    /// <summary>
    /// Mock test method for financial assessment
    /// </summary>
    [TestClass]
    public class FinancialAssessmentTest
    {

        #region Variable
        
        /// <summary>
        /// The financial assessment rule engine
        /// </summary>
        private IFinancialAssessmentRuleEngine financialAssessmentRuleEngine;
        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;
        private long financialAssessmentID = 1;
        private long financialAssessmentDetailID = 1;
        /// <summary>
        /// The financial assessment model
        /// </summary>
        private FinancialAssessmentModel financialAssessmentModel = null;
        private FinancialAssessmentDetailsModel financialAssessmentDetail = null;
        private FinancialAssessmentController financialAssessmentController = null;

        /// <summary>
        /// The empty financial assessment model
        /// </summary>
        private FinancialAssessmentModel emptyFinancialAssessmentModel = null;
        private FinancialAssessmentDetailsModel emptyFinancialAssessmentDetailsModel = null;

        #endregion

        #region Private Method


        /// <summary>
        /// Financials the assessment_ success.
        /// </summary>
        private void FinancialAssessment_Success()
        {
            Mock<IFinancialAssessmentRuleEngine> mock = new Mock<IFinancialAssessmentRuleEngine>();
            financialAssessmentRuleEngine = mock.Object;

            var financialAssessmentDetails = new List<FinancialAssessmentDetailsModel>();
            financialAssessmentDetail = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 1,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 17,
                CategoryID = 1
            };
            financialAssessmentDetails.Add(financialAssessmentDetail);
            var financialAssessmentModels = new List<FinancialAssessmentModel>();
            financialAssessmentModel = new FinancialAssessmentModel()
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
                FamilySize = 2,
                FinancialAssessmentDetails = financialAssessmentDetails

            };
            financialAssessmentModels.Add(financialAssessmentModel);
            var financialAssessment = new Response<FinancialAssessmentModel>()
            {
                DataItems = financialAssessmentModels,
                RowAffected = 1
            };

            var financialAssessmentDetailsResult = new Response<FinancialAssessmentDetailsModel>()
            {
                DataItems = financialAssessmentDetails,
                RowAffected = 1
            };

            //Get FinancialAssessment
            Response<FinancialAssessmentModel> financialAssessmentResponse = new Response<FinancialAssessmentModel>();
            financialAssessmentResponse.DataItems = financialAssessmentModels.Where(contact => contact.ContactID == contactId && contact.FinancialAssessmentID == financialAssessmentID).ToList();

            mock.Setup(r => r.GetFinancialAssessment(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(financialAssessmentResponse);

            //Add FinancialAssessment
            mock.Setup(r => r.AddFinancialAssessment(It.IsAny<FinancialAssessmentModel>()))
                .Callback((FinancialAssessmentModel financialAssessmentsModel) => financialAssessmentModels.Add(financialAssessmentsModel))
                .Returns(financialAssessment);

            //Update FinancialAssessment
            mock.Setup(r => r.UpdateFinancialAssessment(It.IsAny<FinancialAssessmentModel>()))
                .Callback((FinancialAssessmentModel financialAssessmentsModel) => financialAssessmentModels.Add(financialAssessmentsModel))
                .Returns(financialAssessment);


            //Add FinancialAssessment Details
            mock.Setup(r => r.AddFinancialAssessmentDetails(It.IsAny<FinancialAssessmentDetailsModel>()))
                .Callback((FinancialAssessmentDetailsModel financialAssessmentsDetailsModel) => financialAssessmentDetails.Add(financialAssessmentsDetailsModel))
                .Returns(financialAssessmentDetailsResult);

            //Update FinancialAssessment Details
            mock.Setup(r => r.UpdateFinancialAssessmentDetails(It.IsAny<FinancialAssessmentDetailsModel>()))
                .Callback((FinancialAssessmentDetailsModel financialAssessmentsDetailsModel) => financialAssessmentDetails.Add(financialAssessmentsDetailsModel))
                .Returns(financialAssessmentDetailsResult);

            //Delete FinancialAssessment
            Response<bool> deleteResponse = new Response<bool>();
            List<bool> lstStatus = new List<bool>();
            lstStatus.Add(true);

            deleteResponse.RowAffected = 1;
            deleteResponse.ResultCode = 0;
            deleteResponse.DataItems = lstStatus;

            mock.Setup(r => r.DeleteFinancialAssessmentDetail(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => financialAssessmentModels.Remove(financialAssessmentModels.Find(x => x.FinancialAssessmentDetails[0].FinancialAssessmentDetailsID == id)))
                .Returns(deleteResponse);
        }

        /// <summary>
        /// Financials the assessment_ failed.
        /// </summary>
        private void FinancialAssessment_Failed()
        {
            Mock<IFinancialAssessmentRuleEngine> mock = new Mock<IFinancialAssessmentRuleEngine>();
            financialAssessmentRuleEngine = mock.Object;

            var financialAssessmentDetails = new List<FinancialAssessmentDetailsModel>();
            financialAssessmentDetail = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 0,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 17,
                CategoryID = 1
            };
            financialAssessmentDetails.Add(financialAssessmentDetail);
            var financialAssessmentModels = new List<FinancialAssessmentModel>();
            financialAssessmentModel = new FinancialAssessmentModel()
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
                FamilySize = 2,
                FinancialAssessmentDetails = financialAssessmentDetails
            };
            financialAssessmentModels.Add(financialAssessmentModel);
            var financialAssessment = new Response<FinancialAssessmentModel>()
            {
                DataItems = null,
                RowAffected = 0
            };

            var financialAssessmentDetailsResult = new Response<FinancialAssessmentDetailsModel>()
            {
                DataItems = financialAssessmentDetails,
                RowAffected = 0
            };

            //Get FinancialAssessment
            Response<FinancialAssessmentModel> financialAssessmentResponse = new Response<FinancialAssessmentModel>();
            financialAssessmentResponse.DataItems = financialAssessmentModels.Where(contact => contact.ContactID == contactId && contact.FinancialAssessmentID == financialAssessmentID).ToList();

            mock.Setup(r => r.GetFinancialAssessment(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(financialAssessmentResponse);

            //Add FinancialAssessment
            mock.Setup(r => r.AddFinancialAssessment(It.IsAny<FinancialAssessmentModel>()))
                .Callback((FinancialAssessmentModel financialAssessmentsModel) => financialAssessmentModels.Add(financialAssessmentsModel))
                .Returns(financialAssessment);

            //Update FinancialAssessment
            mock.Setup(r => r.UpdateFinancialAssessment(It.IsAny<FinancialAssessmentModel>()))
                .Callback((FinancialAssessmentModel financialAssessmentsModel) => financialAssessmentModels.Add(financialAssessmentsModel))
                .Returns(financialAssessment);


            //Add FinancialAssessment Details
            mock.Setup(r => r.AddFinancialAssessmentDetails(It.IsAny<FinancialAssessmentDetailsModel>()))
                .Callback((FinancialAssessmentDetailsModel financialAssessmentsDetailsModel) => financialAssessmentDetails.Add(financialAssessmentsDetailsModel))
                .Returns(financialAssessmentDetailsResult);

            //Update FinancialAssessment Details
            mock.Setup(r => r.UpdateFinancialAssessmentDetails(It.IsAny<FinancialAssessmentDetailsModel>()))
                .Callback((FinancialAssessmentDetailsModel financialAssessmentsDetailsModel) => financialAssessmentDetails.Add(financialAssessmentsDetailsModel))
                .Returns(financialAssessmentDetailsResult);


            //Delete FinancialAssessment
            Response<bool> deleteResponse = new Response<bool>();
            List<bool> lstStatus = new List<bool>();
            lstStatus.Add(true);

            deleteResponse.RowAffected = 0;
            deleteResponse.ResultCode = 1;
            deleteResponse.DataItems = lstStatus;

            mock.Setup(r => r.DeleteFinancialAssessmentDetail(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => financialAssessmentModels.Remove(financialAssessmentModels.Find(x => x.FinancialAssessmentDetails[0].FinancialAssessmentDetailsID == id)))
                .Returns(deleteResponse);
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
            FinancialAssessment_Success();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.GetFinancialAssessment(contactId, financialAssessmentID);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count > 0, "Atleast one Financial Assessment must exists.");
        }

        /// <summary>
        /// Gets the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void GetFinancialAssessment_Failed()
        {
            // Arrange
            FinancialAssessment_Failed();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.GetFinancialAssessment(0,0);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentModel>>;
            var financialAssessment = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count == 0);
        }

        /// <summary>
        /// Adds the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void AddFinancialAssessment_Success()
        {
            // Arrange
            FinancialAssessment_Success();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.AddFinancialAssessment(financialAssessmentModel);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);

        }

        /// <summary>
        /// Adds the financial assessment details success.
        /// </summary>
        [TestMethod]
        public void AddFinancialAssessmentDetails_Success()
        {
            // Arrange
            FinancialAssessment_Success();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.AddFinancialAssessmentDetails(financialAssessmentDetail);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentDetailsModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);

        }

        /// <summary>
        /// Adds the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void AddFinancialAssessment_Failed()
        {
            // Arrange
            FinancialAssessment_Failed();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.AddFinancialAssessment(emptyFinancialAssessmentModel);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNull(response.Value.DataItems);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

        /// <summary>
        /// Adds the financial assessment details failed.
        /// </summary>
        [TestMethod]
        public void AddFinancialAssessmentDetails_Failed()
        {
            // Arrange
            FinancialAssessment_Failed();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.AddFinancialAssessmentDetails(emptyFinancialAssessmentDetailsModel);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentDetailsModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

        /// <summary>
        /// Updates the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void UpdateFinancialAssessment_Success()
        {
            // Arrange
            FinancialAssessment_Success();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.UpdateFinancialAssessment(financialAssessmentModel);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

         /// <summary>
        /// Updates the financial assessment details success.
        /// </summary>
        [TestMethod]
        public void UpdateFinancialAssessmentDetails_Success()
        {
            // Arrange
            FinancialAssessment_Success();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.UpdateFinancialAssessmentDetails(financialAssessmentDetail);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentDetailsModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        /// <summary>
        /// Updates the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void UpdateFinancialAssessment_Failed()
        {
            // Arrange
            FinancialAssessment_Failed();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.UpdateFinancialAssessment(emptyFinancialAssessmentModel);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentModel>>;
            var financialAssessment = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNull(response.Value.DataItems);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

        /// <summary>
        /// Updates the financial assessment details failed.
        /// </summary>
        [TestMethod]
        public void UpdateFinancialAssessmentDetails_Failed()
        {
            // Arrange
            FinancialAssessment_Failed();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var getFinancialAssessmentResult = financialAssessmentController.UpdateFinancialAssessmentDetails(emptyFinancialAssessmentDetailsModel);
            var response = getFinancialAssessmentResult as HttpResult<Response<FinancialAssessmentDetailsModel>>;
            var financialAssessment = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsTrue(response.Value.RowAffected == 0);
        }

        /// <summary>
        /// Delete the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void DeleteFinancialAssessment_Success()
        {
            // Arrange
            FinancialAssessment_Success();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var deleteFinancialAssessmentResult = financialAssessmentController.DeleteFinancialAssessmentDetail(financialAssessmentDetailID, DateTime.UtcNow);
            var response = deleteFinancialAssessmentResult as HttpResult<Response<bool>>;

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Value.RowAffected, "Financial assessment details could not be deleted.");
        }

        /// <summary>
        /// Delete the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void DeleteFinancialAssessment_Failed()
        {
            // Arrange
            FinancialAssessment_Failed();
            financialAssessmentController = new FinancialAssessmentController(financialAssessmentRuleEngine);

            //Act
            var deleteFinancialAssessmentResult = financialAssessmentController.DeleteFinancialAssessmentDetail(financialAssessmentDetailID, DateTime.UtcNow);
            var response = deleteFinancialAssessmentResult as HttpResult<Response<bool>>;
           
            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.AreEqual(0, response.Value.RowAffected, "Financial assessment details deleted.");  
        }

        #endregion
    }
}
