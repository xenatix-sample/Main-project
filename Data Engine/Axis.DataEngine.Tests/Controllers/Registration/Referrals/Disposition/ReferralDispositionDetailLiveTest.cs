﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.DataEngine.Tests.Controllers.Registration.Referrals.Disposition
{
    /// <summary>
    /// Referral disposition detail unit testing
    /// </summary>
    [TestClass]
    public class ReferralDispositionDetailLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "referralDisposition/";

        /// <summary>
        /// The referral header identifier
        /// </summary>
        private long referralHeaderID = 1;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Get Referral disposition detail's success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralDispositionDetail_Success()
        {
            // Arrange
            var url = baseRoute + "GetReferralDispositionDetail";

            var param = new NameValueCollection();
            param.Add("referralHeaderID", referralHeaderID.ToString());

            // Act
            var response = communicationManager.Get<Response<ReferralDispositionModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Referral Disposition Detail must exist.");
        }

        /// <summary>
        /// Get Referral disposition detail's failed unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralDispositionDetail_Failed()
        {
            // Arrange
            var url = baseRoute + "GetReferralDispositionDetail";

            var param = new NameValueCollection();
            param.Add("referralHeaderID", "-1");

            // Act
            var response = communicationManager.Get<Response<ReferralDispositionModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral disposition detail should not exist for this test case.");
        }

        /// <summary>
        /// Add Referral disposition detail's success unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralDispositionDetail_Success()
        {
            // Arrange
            var url = baseRoute + "AddReferralDisposition";
            var referralDispositionDetail = new ReferralDispositionModel
            {
                ReferralHeaderID = referralHeaderID,
                ReferralDispositionID = 2,
                ReferralDispositionOutcomeID = 1,
                UserID = 1,
                DispositionDate = DateTime.UtcNow,
                AdditionalNotes = "Additional Note Testing",
                ReasonforDenial = "Reason for denial Testing",
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<ReferralDispositionModel, Response<ReferralDispositionModel>>(referralDispositionDetail, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Referral Disposition Detail must exist.");
        }

        /// <summary>
        /// Add Referral disposition detail's failed unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralDispositionDetail_Failed()
        {
            // Arrange
            var url = baseRoute + "AddReferralDisposition";

            var referralDispositionDetail = new ReferralDispositionModel
            {
                ReferralHeaderID = -System.Math.Abs(referralHeaderID),
                ReferralDispositionID = 2,
                ReferralDispositionOutcomeID = 1,
                UserID = 1,
                DispositionDate = DateTime.UtcNow,
                AdditionalNotes = "Additional Note Testing",
                ReasonforDenial = "Reason for denial Testing",
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<ReferralDispositionModel, Response<ReferralDispositionModel>>(referralDispositionDetail, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral disposition detail should not exist for this test case.");
        }

        /// <summary>
        /// Update Referral disposition detail's success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralDispositionDetail_Success()
        {
            // Arrange
            var url = baseRoute + "UpdateReferralDisposition";

            var referralDispositionDetail = new ReferralDispositionModel
            {
                ReferralDispositionDetailID = 1,
                ReferralHeaderID = referralHeaderID,
                ReferralDispositionID = 2,
                ReferralDispositionOutcomeID = 1,
                UserID = 1,
                DispositionDate = DateTime.UtcNow,
                AdditionalNotes = "Additional Note update Testing",
                ReasonforDenial = "Reason for denial update Testing",
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<ReferralDispositionModel, Response<ReferralDispositionModel>>(referralDispositionDetail, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected > 2, "Referral disposition detail could not be updated.");
        }

        /// <summary>
        /// Update Referral disposition detail's failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateReferralDispositionDetail_Failed()
        {
            // Arrange
            var url = baseRoute + "UpdateReferralDisposition";

            var referralDispositionDetail = new ReferralDispositionModel
            {
                ReferralDispositionDetailID = 1,
                ReferralHeaderID = -System.Math.Abs(referralHeaderID),
                ReferralDispositionID = 2,
                ReferralDispositionOutcomeID = 1,
                UserID = 1,
                DispositionDate = DateTime.UtcNow,
                AdditionalNotes = "Additional Note update Testing",
                ReasonforDenial = "Reason for denial update Testing",
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<ReferralDispositionModel, Response<ReferralDispositionModel>>(referralDispositionDetail, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected <= 2, "Updated referral disposition detail with invalid data.");
        }
    }
}