using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.ApiControllers.Referrals;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository.Referrals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Axis.PresentationEngine.Tests.Controllers.Registration.Referrals.Search
{
    [TestClass]
    public class ReferralSearchLiveTest
    {
        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteReferralHeaderID = 2;

        /// <summary>
        /// The controller
        /// </summary>
        private ReferralSearchController controller;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new ReferralSearchController(new ReferralSearchRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Gets the Referral_success.
        /// </summary>
        [TestMethod]
        public void GetReferrals_Success()
        {
            // Act
            var response = controller.GetReferrals("", 1,1).Result;

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Referral must exists.");
        }

        /// <summary>
        /// Gets the Referral_failure.
        /// </summary>
        [TestMethod]
        public void GetReferrals_Failure()
        {
            // Act
            var response = controller.GetReferrals("Invalid Data",-1,0).Result;

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral exists for invalid data.");
        }

        /// <summary>
        /// Deletes the Referral_ success.
        /// </summary>
        [TestMethod]
        public void DeleteReferral_Success()
        {
            // Act
            var response = controller.DeleteReferral(defaultDeleteReferralHeaderID, "Test case delete success", DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 3, "Referral could not be deleted.");
        }

        /// <summary>
        /// Deletes the collateral_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteReferral_Failure()
        {
            // Act
            var response = controller.DeleteReferral(9999, "Test case delete Failure", DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected <= 3, "Referral deleted for invalid data.");
        }
    }
}
