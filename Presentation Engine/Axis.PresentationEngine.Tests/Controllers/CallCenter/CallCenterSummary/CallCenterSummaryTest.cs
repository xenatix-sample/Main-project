using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.CallCenter.ApiControllers;
using Axis.Plugins.CallCenter.Repository.LawLiaisonSummary;
using Axis.Plugins.CallCenter.Repository.CrisisLineSummary;
using System.Configuration;
using System.Web.Mvc;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;

namespace Axis.PresentationEngine.Tests.Controllers.CallCenter.CallCenterSummary
{
    [TestClass]
    public class CallCenterSummaryTest
    {
        /// <summary>
        /// search text  
        /// </summary>
        private string searchText = "";

        /// <summary>
        /// search type filter
        /// </summary>
        private int searchTypeFilter = 1;

        /// <summary>
        /// userID
        /// </summary>
        private int userID = 1;

        /// <summary>
        /// search type filter
        /// </summary>
        private string callStatusID  = null;

        /// <summary>
        /// invalid search text  
        /// </summary>
        private string invalidSearchText = "invalid data";

        /// <summary>
        /// Law Liaison controller
        /// </summary>
        private LawLiaisonSummaryController lawLiaisonController = null;

        /// <summary>
        /// Crisis Line controller
        /// </summary>
        private CrisisLineSummaryController CrisisLineController = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            lawLiaisonController = new LawLiaisonSummaryController(new LawLiaisonSummaryRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
            CrisisLineController = new CrisisLineSummaryController(new CrisisLineSummaryRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Get Law Liaison Success unit test.
        /// </summary>
        [TestMethod]
        public void GetLawLiaisonCallCenterSummary_Success()
        {
            // Act
            var callCenterSearchResult = lawLiaisonController.GetLawLiaisonCallCenterSummary(searchText, userID, searchTypeFilter, callStatusID).Result;

            // Assert
            Assert.IsTrue(callCenterSearchResult != null, "Response can't be null");
            Assert.IsTrue(callCenterSearchResult.DataItems != null, "Data items can't be null");
            Assert.IsTrue(callCenterSearchResult.DataItems.Count > 0, "Atleast one Law Liaison must exist.");
        }

        /// <summary>
        /// Get Law Liaison failure unit test.
        /// </summary>
        [TestMethod]
        public void GetLawLiaisonCallCenterSummary_Failed()
        {
            // Act
            var callCenterSearchResult = lawLiaisonController.GetLawLiaisonCallCenterSummary(invalidSearchText, userID, searchTypeFilter, callStatusID).Result;

            // Assert
            Assert.IsTrue(callCenterSearchResult != null, "Response can't be null");
            Assert.IsTrue(callCenterSearchResult.DataItems != null, "Data items can't be null");
            Assert.IsTrue(callCenterSearchResult.DataItems.Count == 0, "Law Liaison exists for invalid data.");

        }

        /// <summary>
        /// Get Crisis Line Success unit test.
        /// </summary>
        [TestMethod]
        public void GetCrisisLineCallCenterSummary_Success()
        {
            // Act
            var callCenterSearchResult = CrisisLineController.GetCrisisLineCallCenterSummary(searchText, userID, searchTypeFilter, callStatusID).Result;

            // Assert
            Assert.IsTrue(callCenterSearchResult != null, "Response can't be null");
            Assert.IsTrue(callCenterSearchResult.DataItems != null, "Data items can't be null");
            Assert.IsTrue(callCenterSearchResult.DataItems.Count > 0, "Atleast one Crisis Line must exist.");
        }

        /// <summary>
        /// Get Crisis Line failure unit test.
        /// </summary>
        [TestMethod]
        public void GetCrisisLineCallCenterSummary_Failed()
        {
            // Act
            var callCenterSearchResult = CrisisLineController.GetCrisisLineCallCenterSummary(invalidSearchText, userID, searchTypeFilter, callStatusID).Result;

            // Assert
            Assert.IsTrue(callCenterSearchResult != null, "Response can't be null");
            Assert.IsTrue(callCenterSearchResult.DataItems != null, "Data items can't be null");
            Assert.IsTrue(callCenterSearchResult.DataItems.Count == 0, "Crisis Line exists for invalid data.");

        }

       
    }
}
