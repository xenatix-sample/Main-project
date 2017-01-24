using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.ApiControllers;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Linq;

namespace Axis.PresentationEngine.Tests.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class RegisterDemographicsTest
    {
        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;

        /// <summary>
        /// The token
        /// </summary>
        private string token = ConfigurationManager.AppSettings["UnitTestToken"];

        /// <summary>
        /// The demographics view model
        /// </summary>
        private ContactDemographicsViewModel demographicsViewModel = null;

        /// <summary>
        /// Gets the demographic_ success.
        /// </summary>
        [TestMethod]
        public void GetDemographic_Success()
        {
            // Arrange
            var controller = new RegistrationController(new RegistrationRepository(token));

            // Act
            var result = controller.GetContactDemographics(contactId);
            var modelResponse = result.Result;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// Gets the demographic_ failed.
        /// </summary>
        [TestMethod]
        public void GetDemographic_Failed()
        {
            // Arrange
            var controller = new RegistrationController(new RegistrationRepository(token));

            // Act
            var result = controller.GetContactDemographics(0);
            var modelResponse = result.Result;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count == 0);
        }

        /// <summary>
        /// Demographics_s the success_data.
        /// </summary>
        private void Demographics_Success_data()
        {
            var demographicsViewModel = new ContactDemographicsViewModel
            {
                ContactTypeID = 1,
                ClientTypeID = 1,
                FirstName = "FirstName",
                Middle = "MiddleName",
                LastName = "LastName",
                SuffixID = 1,
                GenderID = 1,
                TitleID = 1,
                DOB = DateTime.Now,
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "8765477",
                IsPregnant = false,
                PreferredName = "PreferredName",
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                Age = 1,
                ReferralSourceID = 1,
                ContactPresentingProblem =
                     new ContactPresentingProblemModel()
                        {
                            EffectiveDate = DateTime.Now,
                            ExpirationDate = DateTime.Now,
                            PresentingProblemTypeID = 1
                        },
                Addresses =
                    new List<ContactAddressModel>
                    {
                        new ContactAddressModel()
                        {
                            AddressTypeID = 1,
                            Line1 = "C40",
                            City = "Noida",
                            StateProvince = 1,
                            County = 1,
                            EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2),
                            MailPermissionID=1
                        }
                    },
            };
        }

        /// <summary>
        /// Demographics_s the failed_data.
        /// </summary>
        private void Demographics_Failed_data()
        {
            demographicsViewModel = new ContactDemographicsViewModel
            {
                ContactTypeID = 1,
                ClientTypeID = 1,
                FirstName = null,
                Middle = "MiddleName",
                LastName = null,
                SuffixID = 1,
                GenderID = 1,
                TitleID = 1,
                DOB = DateTime.Now,
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "8765477",
                IsPregnant = false,
                PreferredName = "PreferredName",
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                Age = 1,
                ReferralSourceID = 1,
                ContactPresentingProblem =
                    new ContactPresentingProblemModel()
                    {
                        EffectiveDate = DateTime.Now,
                        ExpirationDate = DateTime.Now,
                        PresentingProblemTypeID = 0
                    },
                Addresses =
                    new List<ContactAddressModel>
                    {
                        new ContactAddressModel()
                        {
                            AddressTypeID = 1,
                            Line1 = "C40",
                            City = "Noida",
                            StateProvince = 1,
                            County = 1,
                            EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2),
                            MailPermissionID=1
                        }
                    },
            };
        }

        /// <summary>
        /// Saves the demographic_ success.
        /// </summary>
        [TestMethod]
        public void SaveDemographic_Success()
        {
            // Arrange
            Demographics_Success_data();
            var controller = new RegistrationController(new RegistrationRepository(token));

            // Act

            var modelResponse = controller.AddContactDemographics(demographicsViewModel);
            var rowAffected = modelResponse.RowAffected;

            // Assert
            Assert.IsTrue(rowAffected > 0);
        }

        /// <summary>
        /// Saves the demographic_ failed.
        /// </summary>
        [TestMethod]
        public void SaveDemographic_Failed()
        {
            // Arrange
            Demographics_Failed_data();
            var controller = new RegistrationController(new RegistrationRepository(token));

            // Act

            var modelResponse = controller.AddContactDemographics(demographicsViewModel);
            var rowAffected = modelResponse.RowAffected;

            // Assert
            Assert.IsTrue(rowAffected == 0);
        }

        /// <summary>
        /// Updates the demographic_ success.
        /// </summary>
        [TestMethod]
        public void updateDemographic_Success()
        {
            // Arrange
            var controller = new RegistrationController(new RegistrationRepository(token));

            // Act
            demographicsViewModel = new ContactDemographicsViewModel
            {
                ContactID = 8,
                ContactTypeID = 1,
                ClientTypeID = 1,
                FirstName = "FirstName",
                Middle = "MiddleName",
                LastName = "LastName",
                SuffixID = 1,
                GenderID = 1,
                TitleID = 1,
                DOB = DateTime.Now,
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "8765477",
                IsPregnant = false,
                PreferredName = "PreferredName",
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                Age = 1,
                ReferralSourceID = 1,
                ContactPresentingProblem =
                    new ContactPresentingProblemModel()
                    {
                        ContactPresentingProblemID = 1,
                        EffectiveDate = DateTime.Now,
                        ExpirationDate = DateTime.Now,
                        PresentingProblemTypeID = 1
                    },
                Addresses =
                    new List<ContactAddressModel>
                    {
                        new ContactAddressModel()
                        {
                            AddressTypeID = 1,
                            Line1 = "C40",
                            City = "Noida",
                            StateProvince = 1,
                            County = 1,
                            EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2),
                            MailPermissionID=1
                        }
                    }
            };
            var modelResponse = controller.UpdateContactDemographics(demographicsViewModel);
            var rowAffected = modelResponse.RowAffected;

            // Assert
            Assert.IsTrue(rowAffected > 0);
        }

        /// <summary>
        /// Updates the demographic_ failed.
        /// </summary>
        [TestMethod]
        public void updateDemographic_Failed()
        {
            // Arrange
            var controller = new RegistrationController(new RegistrationRepository(token));

            // Act
            var demographicsViewModel = new ContactDemographicsViewModel
            {
                ContactID = 0,
                ContactTypeID = 1,
                ClientTypeID = 1,
                FirstName = "FirstName",
                Middle = "MiddleName",
                LastName = "LastName",
                SuffixID = 1,
                GenderID = 1,
                TitleID = 1,
                DOB = DateTime.Now,
                DOBStatusID = 1,
                SSN = "123451234",
                SSNStatusID = 1,
                DriverLicense = "8765477",
                IsPregnant = false,
                PreferredName = "PreferredName",
                DeceasedDate = DateTime.Now,
                ContactMethodID = 1,
                Age = 1,
                ReferralSourceID = 1,
                ContactPresentingProblem =
                   new ContactPresentingProblemModel()
                   {
                       ContactPresentingProblemID = 0,
                       EffectiveDate = DateTime.Now,
                       ExpirationDate = DateTime.Now,
                       PresentingProblemTypeID = 1
                   },
                Addresses =
                    new List<ContactAddressModel>
                    {
                        new ContactAddressModel()
                        {
                            AddressTypeID = 1,
                            Line1 = "C40",
                            City = "Noida",
                            StateProvince = 1,
                            County = 1,
                            EffectiveDate=DateTime.Now,
                            ExpirationDate=DateTime.Now.AddDays(2),
                            MailPermissionID=1
                        }
                    }
            };
            var modelResponse = controller.UpdateContactDemographics(demographicsViewModel);
            var rowAffected = modelResponse.RowAffected;

            // Assert
            Assert.IsTrue(rowAffected == 0);
        }

        /// <summary>
        /// Verifies the duplicate contacts_ success.
        /// </summary>
        [TestMethod]
        public void VerifyDuplicateContacts_Success()
        {
            // Arrange
            Demographics_Success_data();
            var controller = new RegistrationController(new RegistrationRepository(token));

            // Act

            var modelResponse = controller.VerifyDuplicateContacts(demographicsViewModel);

            //Assert
            Assert.IsTrue(modelResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(modelResponse.DataItems.Count > 0, "Atleast one contact demography must exists.");
        }

        /// <summary>
        /// Verifies the duplicate contacts_ failed.
        /// </summary>
        [TestMethod]
        public void VerifyDuplicateContacts_Failed()
        {
            // Arrange
            Demographics_Failed_data();
            var controller = new RegistrationController(new RegistrationRepository(token));

            // Act

            var modelResponse = controller.VerifyDuplicateContacts(demographicsViewModel);

            //Assert
            Assert.IsTrue(modelResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(modelResponse.DataItems.Count > 0, "Contact demography exists for invalid data.");
        }
    }
}
