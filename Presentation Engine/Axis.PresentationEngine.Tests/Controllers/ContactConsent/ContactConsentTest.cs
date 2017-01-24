using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.PresentationEngine.Areas.Consents.ApiControllers;
using Axis.PresentationEngine.Areas.Consents.Repository;
using System.Configuration;
using Axis.PresentationEngine.Areas.Consents.Models;

namespace Axis.PresentationEngine.Tests.Controllers.ContactConsent
{
    [TestClass]
    public class ContactConsentTest
    {
        /// <summary>
        /// The default contact identifier
        /// </summary>
        private int defaultContactId = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private ConsentsController controller;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new ConsentsController(new ConsentsRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }


        /// <summary>
        /// Gets the consents_ success.
        /// </summary>
        [TestMethod]
        public void GetConsents_Success()
        {
            // Act
            var response = controller.GetConsents(defaultContactId);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Consent must exists.");
        }


        /// <summary>
        /// Gets the consents_ failure.
        /// </summary>
        [TestMethod]
        public void GetConsents_Failure()
        {
            // Act
            var response = controller.GetConsents(-1);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Consent exists for invalid data.");
        }


        /// <summary>
        /// Adds the Consent_ success.
        /// </summary>
        [TestMethod]
        public void AddConsent_Success()
        {
            // Act
            var consentModel = new ConsentsViewModel
            {
                ModifiedOn = DateTime.Now,
                DateSigned = DateTime.Now,
                AssessmentSectionID = 57,
                SignatureStatusID = 2,
                EffectiveDate = DateTime.Now,
                AssessmentID = 30,
                ResponseID = 1,
                ContactID = 1,
                ForceRollback = true
            };

            var response = controller.AddConsent(consentModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Consent could not be created.");
        }

        /// <summary>
        /// Adds the Consent_ failure.
        /// </summary>
        [TestMethod]
        public void AddConsent_Failure()
        {
            // Act
            var consentModel = new ConsentsViewModel
            {
                DateSigned = DateTime.Now,
                AssessmentSectionID = -1,
                SignatureStatusID = -2,
                EffectiveDate = DateTime.Now,
                AssessmentID = -1,
                ResponseID = -1,
                ContactID = -1,
                ForceRollback = true
            };

            var response = controller.AddConsent(consentModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Consent created for invalid data.");
        }

        /// <summary>
        /// Updates the Consent_ success.
        /// </summary>
        [TestMethod]
        public void UpdateConsent_Success()
        {
            // Act
            var consentModel = new ConsentsViewModel
            {
                ContactConsentID = 1,
                DateSigned = DateTime.Now,
                ExpirationDate = DateTime.Now,
                ExpirationReasonID = 1,
                ExpiredResponseID = 1,
                ExpiredBy = 1,
                SignatureStatusID = 1,
                AssessmentID = 1,
                ResponseID = 1,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateConsent(consentModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Consent could not be updated.");
        }

        /// <summary>
        /// Updates the Consent_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateConsent_Failure()
        {
            // Act
            var consentModel = new ConsentsViewModel
            {
                ContactConsentID = -1,
                DateSigned = DateTime.Now,
                ExpirationDate = DateTime.Now,
                ExpirationReasonID = 1,
                ExpiredResponseID = 1,
                ExpiredBy = 1,
                SignatureStatusID = 1,
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            var response = controller.UpdateConsent(consentModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Consent updated for invalid data.");
        }

    }
}
