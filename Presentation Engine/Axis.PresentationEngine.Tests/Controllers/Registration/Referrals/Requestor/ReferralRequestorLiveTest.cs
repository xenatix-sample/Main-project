using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Common;
using System.Configuration;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Requestor;
using Axis.Plugins.Registration.Models.Referrals.Common;
using Axis.Plugins.Registration.Repository;
using Axis.Plugins.Registration.Repository.Common;
using Axis.Plugins.Registration.Model;
using System;
using Axis.Model.Registration;
using Axis.Plugins.Registration.ApiControllers;
using RegistrationController = Axis.Plugins.Registration.ApiControllers.RegistrationController;

namespace Axis.PresentationEngine.Tests.Controllers.Referrals.Requestor
{
    [TestClass]
    public class ReferralRequestorLiveTest
    {
        private const string baseRoute = "requestor/";

        private long referralHeaderID = 1;

        private RegistrationController demographicsController;
        private ReferralHeaderController headerController;
        private ContactAddressController addressController;
        private ContactEmailController emailController;
        private ContactPhoneController phoneController;

        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            demographicsController = new RegistrationController(new RegistrationRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
            headerController = new ReferralHeaderController(new ReferralHeaderRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
            addressController = new ContactAddressController(new ContactAddressRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
            emailController = new ContactEmailController(new ContactEmailRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
            phoneController = new ContactPhoneController(new ContactPhoneRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetReferralRequestor_Success()
        {
            //Header

            // Act
            var headerResponse = headerController.GetReferralHeader(referralHeaderID);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(headerResponse.DataItems.Count > 0, "Atleast one referral Header must exist.");


            var contactID = headerResponse.DataItems[0].ContactID;

            //Demographics

            // Act
            var demographicsResult = demographicsController.GetContactDemographics(contactID);
            var demographicsResponse = demographicsResult.Result;

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(demographicsResponse.DataItems.Count > 0, "Atleast one referral demographics must exist.");


            //Address

            // Act
            var addressResponse = addressController.GetAddresses(contactID, 7);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(addressResponse.DataItems.Count > 0, "Atleast one referral address must exist.");


            //Email

            // Act
            var emailResponse = emailController.GetEmails(Convert.ToInt32(contactID), 7);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(emailResponse.DataItems.Count > 0, "Atleast one referral email must exist.");


            //Phone

            // Act
            var phoneResponse = phoneController.GetContactPhones(contactID, 7);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(phoneResponse.DataItems.Count > 0, "Atleast one referral phone must exist.");
        }

        [TestMethod]
        public void GetReferralRequestor_Failed()
        {
            //Demographics

            // Act
            var demographicsResult = demographicsController.GetContactDemographics(-1);
            var demographicsResponse = demographicsResult.Result;

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(demographicsResponse.DataItems.Count == 0, "Referral demographics must not exist.");


            //Header

            // Act
            var headerResponse = headerController.GetReferralHeader(-1);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(headerResponse.DataItems.Count == 0, "Referral header must not exist.");


            //Address

            // Act
            var addressResponse = addressController.GetAddresses(-1, 7);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(addressResponse.DataItems.Count == 0, "Referral address must not exist.");


            //Email

            // Act
            var emailResponse = emailController.GetEmails(-1, 7);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(headerResponse.DataItems.Count == 0, "Referral email must not exist.");


            //Phone

            // Act
            var phoneResponse = phoneController.GetContactPhones(-1, 7);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(headerResponse.DataItems.Count == 0, "Referral phone must not exist.");
        }


        [TestMethod]
        public void AddReferralRequestor_Success()
        {
            //Demographics

            // Arrange
            var referralDemographics = new ContactDemographicsViewModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                SuffixID = 1,
                TitleID = 1,
                Middle = "M",
                ForceRollback = true
            };

            // Act
            var demographicsResponse = demographicsController.AddContactDemographics(referralDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.RowAffected == 3, "Referral demographics could not be created.");

            //Header

            // Arrange
            var referralHeader = new ReferralHeaderViewModel
            {
                ContactID = 1,
                ReferralStatusID = 1,
                ReferralTypeID = 1,
                ResourceTypeID = 1,
                //ReferralCategoryID = 1,
                ReferralSourceID = 1,
                ReferralOriginID = 1,
                OrganizationID = 1,
                ForceRollback = true
            };

            // Act
            var headerResponse = headerController.AddReferralHeader(referralHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.RowAffected > 2, "Referral header could not be created.");

            //address

            // Arrange
            var referralAddress = new ContactAddressViewModel
            {
                ContactID = 1,
                AddressTypeID = 1,
                Line1 = "Line1",
                Line2 = "Line2",
                City = "City",
                StateProvince = 1,
                County = 1,
                Zip = "12321-3212",
                ForceRollback = true
            };

            // Act
            var addressResponse = addressController.AddUpdateAddress(referralAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.RowAffected > 2, "Referral address could not be created.");

            //Email

            // Arrange
            var referralEmail = new ContactEmailViewModel
            {
                ContactID = 1,
                EmailPermissionID = 1,
                Email = "Email@Email.Email",
                ForceRollback = true
            };

            // Act
            var emailResponse = emailController.AddUpdateEmail(referralEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.RowAffected > 2, "Referral email could not be created.");

            //Phone

            // Arrange
            var referralPhone = new ContactPhoneModel
            {
                ContactID = 1,
                PhonePermissionID = 1,
                PhoneTypeID = 1,
                Number = "2342342342",
                Extension = "Extension",
                ForceRollback = true
            };

            // Act
            var phoneResponse = phoneController.AddUpdateContactPhone(referralPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.RowAffected > 2, "Referral phone could not be created.");
        }

        [TestMethod]
        public void AddReferralRequestor_Failed()
        {
            //Demographics

            // Arrange
            var referralDemographics = new ContactDemographicsViewModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                SuffixID = -1,
                TitleID = -1,
                Middle = "M",
                ForceRollback = true
            };

            // Act
            var demographicsResponse = demographicsController.AddContactDemographics(referralDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.RowAffected < 3, "Invalid referral demographics has been added.");

            //Header

            // Arrange
            var referralHeader = new ReferralHeaderViewModel
            {
                ContactID = -1,
                ReferralStatusID = 1,
                ReferralTypeID = 1,
                ResourceTypeID = 1,
                ReferralHeaderID = 1,
                ReferralCategorySourceID = 1,
                ReferralOriginID = 1,
                OrganizationID = 1,
                ForceRollback = true
            };

            // Act
            var headerResponse = headerController.AddReferralHeader(referralHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.RowAffected < 3, "Invalid referral header has been added.");

            //address

            // Arrange
            var referralAddress = new ContactAddressViewModel
            {
                ContactID = -1,
                AddressTypeID = 1,
                Line1 = "Line1",
                Line2 = "Line2",
                City = "City",
                StateProvince = 1,
                County = 1,
                Zip = "12321-3212",
                ForceRollback = true
            };

            // Act
            var addressResponse = addressController.AddUpdateAddress(referralAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.RowAffected < 5, "Invalid referral address has been added.");

            //Email

            // Arrange
            var referralEmail = new ContactEmailViewModel
            {
                ContactID = -1,
                EmailPermissionID = 1,
                Email = "Email@Email.Email",
                ForceRollback = true
            };

            // Act
            var emailResponse = emailController.AddUpdateEmail(referralEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.RowAffected < 3, "Invalid referral email has been added.");

            //Phone

            // Arrange
            var referralPhone = new ContactPhoneModel
            {
                ContactID = -1,
                PhonePermissionID = 1,
                PhoneTypeID = 1,
                Number = "2342342342",
                Extension = "Extension",
                ForceRollback = true
            };

            // Act
            var phoneResponse = phoneController.AddUpdateContactPhone(referralPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.RowAffected < 3, "Invalid referral phone has been added.");
        }
        
        [TestMethod]
        public void UpdateReferralRequestor_Success()
        {
            //Demographics

            // Arrange
            var referralDemographics = new ContactDemographicsViewModel
            {
                ContactID = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                SuffixID = 1,
                TitleID = 1,
                Middle = "M",
                ForceRollback = true
            };

            // Act
            var demographicsResponse = demographicsController.UpdateContactDemographics(referralDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.RowAffected > 2, "Referral demographics could not be updated.");

            //Header

            // Arrange
            var referralHeader = new ReferralHeaderViewModel
            {
                ContactID = 1,
                ReferralHeaderID = 2,
                ReferralStatusID = 1,
                ReferralTypeID = 1,
                ResourceTypeID = 1,
                //ReferralCategoryID = 1,
                ReferralSourceID = 1,
                ReferralOriginID = 1,
                OrganizationID = 1,
                ForceRollback = true
            };

            // Act
            var headerResponse = headerController.UpdateReferralHeader(referralHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.RowAffected > 1, "Referral header could not be updated.");

            //address

            // Arrange
            var referralAddress = new ContactAddressViewModel
            {
                ContactID = 2,
                AddressID = 2,
                AddressTypeID = 1,
                Line1 = "Line1",
                Line2 = "Line2",
                City = "City",
                StateProvince = 1,
                County = 1,
                Zip = "12321-3212",
                ForceRollback = true
            };

            // Act
            var addressResponse = addressController.AddUpdateAddress(referralAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.RowAffected >= 2, "Referral address could not be updated.");

            //Email

            // Arrange
            var referralEmail = new ContactEmailViewModel
            {
                ContactID = 1,
                EmailID = 1,
                EmailPermissionID = 1,
                Email = "Email@Email.Email",
                ForceRollback = true
            };

            // Act
            var emailResponse = emailController.AddUpdateEmail(referralEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.RowAffected > 1, "Referral email could not be updated.");

            //Phone

            // Arrange
            var referralPhone = new ContactPhoneModel
            {
                ContactID = 1,
                PhoneID = 1,
                PhonePermissionID = 1,
                PhoneTypeID = 1,
                Number = "2342342342",
                Extension = "Extension",
                ForceRollback = true
            };

            // Act
            var phoneResponse = phoneController.AddUpdateContactPhone(referralPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.RowAffected > 1, "Referral phone could not be updated.");
        }

        [TestMethod]
        public void UpdateReferralRequestor_Failed()
        {
            //Demographics

            // Arrange
            var referralDemographics = new ContactDemographicsViewModel
            {
                ContactID = -1,
                FirstName = "FirstName",
                LastName = "LastName",
                SuffixID = -1,
                TitleID = -1,
                Middle = "M",
                ForceRollback = true
            };

            // Act
            var demographicsResponse = demographicsController.UpdateContactDemographics(referralDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.RowAffected < 3, "Invalid referral demographics has been updated.");

            //Header

            // Arrange
            var referralHeader = new ReferralHeaderViewModel
            {
                ContactID = -1,
                ReferralHeaderID = -1,
                ReferralStatusID = 1,
                ReferralTypeID = 1,
                ResourceTypeID = 1,
                //ReferralCategoryID = 1,
                ReferralSourceID = 1,
                ReferralOriginID = 1,
                ForceRollback = true
            };

            // Act
            var headerResponse = headerController.AddReferralHeader(referralHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.RowAffected < 1, "Invalid referral header has been updated.");

            //address

            // Arrange
            var referralAddress = new ContactAddressViewModel
            {
                ContactID = -1,
                AddressID = -1,
                AddressTypeID = 1,
                Line1 = "Line1",
                Line2 = "Line2",
                City = "City",
                StateProvince = 1,
                County = 1,
                Zip = "12321-3212",
                ForceRollback = true
            };

            // Act
            var addressResponse = addressController.AddUpdateAddress(referralAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.RowAffected <= 3, "Invalid referral address has been updated.");

            //Email
            // Arrange
            var referralEmail = new ContactEmailViewModel
            {
                ContactID = -1,
                EmailID = -1,
                EmailPermissionID = 1,
                Email = "Email@Email.Email",
                ForceRollback = true
            };

            // Act
            var emailResponse = emailController.AddUpdateEmail(referralEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.RowAffected <= 2, "Invalid referral email has been updated.");

            //Phone

            // Arrange
            var referralPhone = new ContactPhoneModel
            {
                ContactID = -1,
                ContactPhoneID = -1,
                PhoneID = -1,
                PhonePermissionID = -1,
                PhoneTypeID = -1,
                Number = "0123456789",
                ForceRollback = true
            };

            // Act
            var phoneResponse = phoneController.AddUpdateContactPhone(referralPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.RowAffected <= 1, "Invalid referral phone has been updated.");
        }
    }
}
