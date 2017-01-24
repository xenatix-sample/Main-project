using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers.Registration.FinancialAssessment
{
    /// <summary>
    /// Live test method for financial assessment
    /// </summary>
    [TestClass]
    public class FinancialAssessmentLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "FinancialAssessment/";

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;
        private long financialAssessmentID = 1;
        private long financialAssessmentDetailID = 1;
        private long financialAssessmentDetailFailedID = 0;
        /// <summary>
        /// The request model
        /// </summary>
        private FinancialAssessmentModel requestModel = null;
        private FinancialAssessmentDetailsModel requestDetailsModel = null;

        #endregion Class Variables

        #region Test Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];

            requestModel = new FinancialAssessmentModel()
            {
                ContactID = 1,
                FinancialAssessmentID = 1,
                AssessmentDate = DateTime.Now,
                TotalIncome = 300000,
                TotalExpenses = 30000,
                AdjustedGrossIncome = 5000,
                TotalExtraOrdinaryExpenses=100,
                TotalOther=100,
                ExpirationDate=DateTime.Now,
                ExpirationReasonID=0,
                FamilySize = 2
            };

            requestDetailsModel = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 1,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 17,
                CategoryID = 1
            };
        }

        /// <summary>
        /// Gets the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void GetFinancialAssessment_Success()
        {
            //Arrenge
            var url = baseRoute + "getFinancialAssessment";
            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());
            param.Add("financialAssessmentID", financialAssessmentID.ToString());

            //Act
            var response = communicationManager.Get<Response<FinancialAssessmentModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one financial assessment record must exists.");
        }

        /// <summary>
        /// Gets the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void GetFinancialAssessment_Failed()
        {
            //Arrenge
            var url = baseRoute + "getFinancialAssessment";
            var param = new NameValueCollection();
            param.Add("contactId", "0");
            param.Add("financialAssessmentID", "0");

            //Act
            var response = communicationManager.Get<Response<FinancialAssessmentModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems.Count == 0);
        }

        /// <summary>
        /// Adds the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void AddFinancialAssessment_Success()
        {
            //Arrenge
            var url = baseRoute + "AddFinancialAssessment";

            //Act
            var response = communicationManager.Post<FinancialAssessmentModel, Response<FinancialAssessmentModel>>(requestModel, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Financial assessment could not be created.");
            Assert.IsTrue(response.DataItems != null, "Data Items can't be null");
            Assert.IsTrue(response.DataItems.Count == 1, "Data Items must have exactly one item in it");
        }

        /// <summary>
        /// Adds the financial assessment details success.
        /// </summary>
        [TestMethod]
        public void AddFinancialAssessmentDetails_Success()
        {
            //Arrenge
            var url = baseRoute + "AddFinancialAssessmentDetails";

            //Act
            var response = communicationManager.Post<FinancialAssessmentDetailsModel, Response<FinancialAssessmentDetailsModel>>(requestDetailsModel, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Financial assessment could not be created.");
            Assert.IsTrue(response.DataItems != null, "Data Items can't be null");
            Assert.IsTrue(response.DataItems.Count == 1, "Data Items must have exactly one item in it");
        }

        /// <summary>
        /// Adds the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void AddFinancialAssessment_Failed()
        {
            //Arrenge
            var url = baseRoute + "AddFinancialAssessment";

            
            requestModel = new FinancialAssessmentModel()
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

            //Act
            var response = communicationManager.Post<FinancialAssessmentModel, Response<FinancialAssessmentModel>>(requestModel, url);

            //Assert
            Assert.IsNull(response.DataItems);
        }

        /// <summary>
        /// Adds the financial assessment details failed.
        /// </summary>
        [TestMethod]
        public void AddFinancialAssessmentDetails_Failed()
        {
            //Arrenge
            var url = baseRoute + "AddFinancialAssessmentDetails";
            
            requestDetailsModel = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 0,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 17,
                CategoryID = 1
            };

            //Act
            var response = communicationManager.Post<FinancialAssessmentDetailsModel, Response<FinancialAssessmentDetailsModel>>(requestDetailsModel, url);

            //Assert
            Assert.IsNull(response.DataItems);
        }
        
        /// <summary>
        /// Updates the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void UpdateFinancialAssessment_Success()
        {
            //Arrange
            var url = baseRoute + "UpdateFinancialAssessment";
            requestModel = new FinancialAssessmentModel()
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

            //Act
            var response = communicationManager.Post<FinancialAssessmentModel, Response<FinancialAssessmentModel>>(requestModel, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Financial assessment could not be updated.");
        }

        /// <summary>
        /// Updates the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void UpdateFinancialAssessmentDetails_Success()
        {
            //Arrange
            var url = baseRoute + "UpdateFinancialAssessmentDetails";
            requestDetailsModel = new FinancialAssessmentDetailsModel()
            {
                FinancialAssessmentDetailsID = 1,
                FinancialAssessmentID = 1,
                CategoryTypeID = 1,
                Amount = 100,
                FinanceFrequencyID = 1,
                CategoryID = 1
            };

            //Act
            var response = communicationManager.Post<FinancialAssessmentDetailsModel, Response<FinancialAssessmentDetailsModel>>(requestDetailsModel, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Financial assessment could not be updated.");
        }
        
        /// <summary>
        /// Updates the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void UpdateFinancialAssessment_Failed()
        {
            //Arrenge
            var url = baseRoute + "UpdateFinancialAssessment";
            requestModel = new FinancialAssessmentModel()
            {
                ContactID = 0,
                FinancialAssessmentID = 0,
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
            //Act
            var response = communicationManager.Post<FinancialAssessmentModel, Response<FinancialAssessmentModel>>(requestModel, url);

            //Assert
            Assert.IsTrue(response.RowAffected == 0);

        }

        /// <summary>
        /// Updates the financial assessment details failed.
        /// </summary>
        [TestMethod]
        public void UpdateFinancialAssessmentDetails_Failed()
        {
            //Arrenge
            var url = baseRoute + "UpdateFinancialAssessmentDetails";

            //Act
            var response = communicationManager.Post<FinancialAssessmentDetailsModel, Response<FinancialAssessmentDetailsModel>>(requestDetailsModel, url);

            //Assert
            Assert.IsNull(response.DataItems);
            Assert.IsTrue(response.RowAffected == 0);

        }


        /// <summary>
        /// Delete the financial assessment_ success.
        /// </summary>
        [TestMethod]
        public void DeleteFinancialAssessment_Success()
        {
            //Arrange
            var url = baseRoute + "DeleteFinancialAssessmentDetail";
            var param = new NameValueCollection();
            param.Add("financialAssessmentDetailID", financialAssessmentDetailID.ToString());
            //Act
            var response = communicationManager.Delete<Response<FinancialAssessmentModel>>(param, url);
            
            //Assert
            Assert.IsTrue(response.RowAffected == 0, "Financial assessment could not be deleted.");
        }

        /// <summary>
        /// Delete the financial assessment_ failed.
        /// </summary>
        [TestMethod]
        public void DeleteFinancialAssessment_Failed()
        {
            //Arrenge
            var url = baseRoute + "DeleteFinancialAssessmentDetail";
            var param = new NameValueCollection();
            param.Add("financialAssessmentDetailID", financialAssessmentDetailFailedID.ToString());
            //Act
            var response = communicationManager.Delete<Response<FinancialAssessmentModel>>(param, url);

            //Assert
            Assert.IsTrue(response.RowAffected == 0, "Financial assessment could not be deleted.");

        }


        #endregion Test Methods
    }
}