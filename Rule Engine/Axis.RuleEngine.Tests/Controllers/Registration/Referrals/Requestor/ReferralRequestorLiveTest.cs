using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Registration.Referral;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;
using System.Globalization;
using Axis.Model.Registration;

namespace Axis.RuleEngine.Tests.Controllers.Registration.Referrals.Requestor
{
    [TestClass]
    public class ReferralRequestorLiveTest
    {
        private CommunicationManager communicationManager;

        private const string baseRouteDemographics = "Registration/";
        private const string baseRouteHeader = "ReferralHeader/";
        private const string baseRouteAddress = "ContactAddress/";
        private const string baseRouteEmail = "ContactEmail/";
        private const string baseRoutePhone = "ContactPhones/";

        private const long referralHeaderID = 1;

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetReferralRequestor_Success()
        {
            //Header

            // Arrange
            var urlHeader = baseRouteHeader + "GetReferralHeader";

            var paramHeader = new NameValueCollection();
            paramHeader.Add("referralHeaderID", referralHeaderID.ToString());

            // Act
            var headerResponse = communicationManager.Get<Response<ReferralHeaderModel>>(paramHeader, urlHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(headerResponse.DataItems.Count > 0, "Atleast one referral Header must exist.");


            var contactId = headerResponse.DataItems[0].ContactID;

            //Demographics

            // Arrange
            var urlDemographics = baseRouteDemographics + "getContactDemographics";

            var paramDemographics = new NameValueCollection();
            paramDemographics.Add("contactId", contactId.ToString());

            // Act
            var demographicsResponse = communicationManager.Get<Response<ContactDemographicsModel>>(paramDemographics, urlDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(demographicsResponse.DataItems.Count > 0, "Atleast one referral demographics must exist.");


            //Address

            // Arrange
            var urlAddress = baseRouteAddress + "GetAddresses";

            var paramAddress = new NameValueCollection();
            paramAddress.Add("contactID", contactId.ToString());
            paramAddress.Add("contactTypeID", "7");

            // Act
            var addressResponse = communicationManager.Get<Response<ContactAddressModel>>(paramAddress, urlAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(addressResponse.DataItems.Count > 0, "Atleast one referral address must exist.");


            //Email

            // Arrange
            var urlEmail = baseRouteEmail + "GetEmails";

            var paramEmail = new NameValueCollection();
            paramEmail.Add("contactID", contactId.ToString());
            paramEmail.Add("contactTypeID", "7");

            // Act
            var emailResponse = communicationManager.Get<Response<ContactEmailModel>>(paramEmail, urlEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(emailResponse.DataItems.Count > 0, "Atleast one referral email must exist.");


            //Phone

            // Arrange

            var urlPhone = baseRoutePhone + "GetContactPhones";

            var paramPhone = new NameValueCollection();
            paramPhone.Add("contactID", contactId.ToString());
            paramPhone.Add("contactTypeID", "7");

            // Act
            var phoneResponse = communicationManager.Get<Response<ContactPhoneModel>>(paramPhone, urlPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(phoneResponse.DataItems.Count > 0, "Atleast one referral phone must exist.");
        }

        [TestMethod]
        public void GetReferralRequestor_Failed()
        {
            //Header

            // Arrange
            var urlHeader = baseRouteHeader + "GetReferralHeader";

            var paramHeader = new NameValueCollection();
            paramHeader.Add("referralHeaderID", "-1");

            // Act
            var headerResponse = communicationManager.Get<Response<ReferralHeaderModel>>(paramHeader, urlHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(headerResponse.DataItems.Count == 0, "Atleast one referral Header must exist.");


            var contactID = -1;

            //Demographics

            // Arrange
            var urlDemographics = baseRouteDemographics + "GetContactDemographics";

            var paramDemographics = new NameValueCollection();
            paramDemographics.Add("contactID", contactID.ToString());

            // Act
            var demographicsResponse = communicationManager.Get<Response<ContactDemographicsModel>>(paramDemographics, urlDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(demographicsResponse.DataItems.Count == 0, "Atleast one referral demographics must exist.");


            //Address

            // Arrange
            var urlAddress = baseRouteAddress + "GetAddresses";

            var paramAddress = new NameValueCollection();
            paramAddress.Add("contactID", contactID.ToString());
            paramAddress.Add("contactTypeID", "7");

            // Act
            var addressResponse = communicationManager.Get<Response<ContactAddressModel>>(paramAddress, urlAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(addressResponse.DataItems.Count == 0, "Atleast one referral address must exist.");


            //Email

            // Arrange
            var urlEmail = baseRouteEmail + "GetEmails";

            var paramEmail = new NameValueCollection();
            paramEmail.Add("contactID", contactID.ToString());
            paramEmail.Add("contactTypeID", "7");

            // Act
            var emailResponse = communicationManager.Get<Response<ContactEmailModel>>(paramEmail, urlEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(emailResponse.DataItems.Count == 0, "Atleast one referral email must exist.");


            //Phone

            // Arrange
            var urlPhone = baseRoutePhone + "GetContactPhones";

            var paramPhone = new NameValueCollection();
            paramPhone.Add("contactID", contactID.ToString());
            paramPhone.Add("contactTypeID", "7");

            // Act
            var phoneResponse = communicationManager.Get<Response<ContactPhoneModel>>(paramPhone, urlPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(phoneResponse.DataItems.Count == 0, "Atleast one referral phone must exist.");
        }

        [TestMethod]
        public void AddReferralRequestor_Success()
        {
            //Demographics

            // Arrange
            var urlDemographics = baseRouteDemographics + "AddContactDemographics";

            var referralDemographics = new ContactDemographicsModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                SuffixID = 1,
                TitleID = 1,
                Middle = "M",
                ForceRollback = true
            };

            // Act
            var demographicsResponse = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(referralDemographics, urlDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.RowAffected >= 3, "Referral demographics could not be created.");

            //Header

            // Arrange

            var urlHeader = baseRouteHeader + "AddReferralHeader";

            var referralHeader = new ReferralHeaderModel
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
            var headerResponse = communicationManager.Post<ReferralHeaderModel, Response<ReferralHeaderModel>>(referralHeader, urlHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.RowAffected >= 3, "Referral header could not be created.");

            //Address

            // Arrange
            var urlAddress = baseRouteAddress + "AddUpdateAddress";

            var referralAddress = new List<ContactAddressModel>();
            referralAddress.Add(new ContactAddressModel
            {
                ContactID = 1,
                MailPermissionID = 1,
                AddressTypeID = 1,
                Line1 = "Line1",
                Line2 = "Line2",
                City = "City",
                StateProvince = 1,
                County = 1,
                Zip = "12321-3212",
                ForceRollback = true
            });
            // Act
            var addressResponse = communicationManager.Post<List<ContactAddressModel>, Response<List<ContactAddressModel>>>(referralAddress, urlAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.RowAffected >= 3, "Referral address could not be created.");

            //Email

            // Arrange
            var urlEmail = baseRouteEmail + "AddUpdateEmail";

            var referralEmail = new List<ContactEmailModel>();
            referralEmail.Add(new ContactEmailModel
            {
                ContactID = 1,
                EmailPermissionID = 1,
                Email = "Email@Email.Email",
                ForceRollback = true
            });

            // Act
            var emailResponse = communicationManager.Post<List<ContactEmailModel>, Response<List<ContactEmailModel>>>(referralEmail, urlEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.RowAffected >= 3, "Referral email could not be created.");

            //Phone

            // Arrange
            var urlPhone = baseRoutePhone + "AddUpdateContactPhones";

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
            var phoneResponse = communicationManager.Post<ContactPhoneModel, Response<ContactPhoneModel>>(referralPhone, urlPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.RowAffected >= 3, "Referral phone could not be created.");
        }

        [TestMethod]
        public void AddReferralRequestor_Failed()
        {
            //Demographics

            // Arrange
            var urlDemographics = baseRouteDemographics + "AddContactDemographics";

            var referralDemographics = new ContactDemographicsModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                SuffixID = -1,
                TitleID = -1,
                Middle = "M",
                ForceRollback = true
            };

            // Act
            var demographicsResponse = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(referralDemographics, urlDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.RowAffected < 3, "Invalid referral demographics has been added.");

            //Header

            // Arrange

            var urlHeader = baseRouteHeader + "AddReferralHeader";

            var referralHeader = new ReferralHeaderModel
            {
                ContactID = -1,
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
            var headerResponse = communicationManager.Post<ReferralHeaderModel, Response<ReferralHeaderModel>>(referralHeader, urlHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.RowAffected < 3, "Invalid referral header has been added.");

            //address

            // Arrange
            var urlAddress = baseRouteAddress + "AddUpdateAddress";

            var referralAddress = new List<ContactAddressModel>();
            referralAddress.Add(new ContactAddressModel
            {
                ContactID = -1,
                MailPermissionID = 1,
                AddressTypeID = 1,
                Line1 = "Line1",
                Line2 = "Line2",
                City = "City",
                StateProvince = 1,
                County = 1,
                Zip = "12321-3212",
                ForceRollback = true
            });
            // Act
            var addressResponse = communicationManager.Post<List<ContactAddressModel>, Response<List<ContactAddressModel>>>(referralAddress, urlAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.RowAffected < 3, "Invalid referral address has been added.");

            //Email

            // Arrange
            var urlEmail = baseRouteEmail + "AddUpdateEmail";

            var referralEmail = new List<ContactEmailModel>();
            referralEmail.Add(new ContactEmailModel
            {
                ContactID = -1,
                EmailPermissionID = 1,
                Email = "Email@Email.Email",
                ForceRollback = true
            });

            // Act
            var emailResponse = communicationManager.Post<List<ContactEmailModel>, Response<List<ContactEmailModel>>>(referralEmail, urlEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.RowAffected < 3, "Invalid referral email has been added.");

            //Phone

            // Arrange
            var urlPhone = baseRoutePhone + "AddUpdateContactPhones";

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
            var phoneResponse = communicationManager.Post<ContactPhoneModel, Response<ContactPhoneModel>>(referralPhone, urlPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.RowAffected < 3, "Invalid referral phone has been added.");
        }

        [TestMethod]
        public void UpdateReferralRequestor_Success()
        {
            //Demographics

            // Arrange
            var urlDemographics = baseRouteDemographics + "UpdateContactDemographics";
            var referralDemographics = new ContactDemographicsModel
            {
                ContactID = 4,
                FirstName = "FirstName",
                LastName = "LastName",
                SuffixID = 1,
                TitleID = 1,
                Middle = "M",
                ForceRollback = true
            };

            // Act
            var demographicsResponse = communicationManager.Post<ContactDemographicsModel, Response<ContactDemographicsModel>>(referralDemographics, urlDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.RowAffected == 3, "Referral demographics could not be updated.");

            //Header

            // Arrange
            var urlHeader = baseRouteHeader + "UpdateReferralHeader";
            var referralHeader = new ReferralHeaderModel
            {
                ContactID = 4,
                ReferralHeaderID = 1,
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
            var headerResponse = communicationManager.Put<ReferralHeaderModel, Response<ReferralHeaderModel>>(referralHeader, urlHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.RowAffected == 3, "Referral header could not be updated.");

            //Address

            // Arrange
            var urlAddress = baseRouteAddress + "AddUpdateAddress";
            var referralAddress = new List<ContactAddressModel>();
            referralAddress.Add(new ContactAddressModel
            {
                ContactID = 4,
                AddressID = 2,
                MailPermissionID = 1,
                AddressTypeID = 1,
                Line1 = "Line1",
                Line2 = "Line2",
                City = "City",
                StateProvince = 1,
                County = 1,
                Zip = "12321-3212",
                ForceRollback = true
            });

            // Act
            var addressResponse = communicationManager.Post<List<ContactAddressModel>, Response<List<ContactAddressModel>>>(referralAddress, urlAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.RowAffected == 2, "Referral address could not be updated.");

            //Email

            // Arrange
            var urlEmail = baseRouteEmail + "AddUpdateEmail";
            var referralEmail = new List<ContactEmailModel>();
            referralEmail.Add(new ContactEmailModel
            {
                ContactID = 4,
                EmailID = 1,
                EmailPermissionID = 1,
                Email = "Email@Email.Email",
                ForceRollback = true
            });

            // Act
            var emailResponse = communicationManager.Post<List<ContactEmailModel>, Response<List<ContactEmailModel>>>(referralEmail, urlEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.RowAffected == 2, "Referral email could not be updated.");

            //Phone

            // Arrange
            var urlPhone = baseRoutePhone + "AddUpdateContactPhones";
            var referralPhone = new ContactPhoneModel
            {
                ContactID = 4,
                PhoneID = 1,
                PhonePermissionID = 1,
                PhoneTypeID = 1,
                Number = "2342342342",
                Extension = "Extension",
                ForceRollback = true
            };

            // Act
            var phoneResponse = communicationManager.Post<ContactPhoneModel, Response<ContactPhoneModel>>(referralPhone, urlPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.RowAffected == 3, "Referral phone could not be updated.");
        }

        [TestMethod]
        public void UpdateReferralRequestor_Failed()
        {
            //Demographics

            // Arrange
            var urlDemographics = baseRouteDemographics + "UpdateContactDemographics";
            var referralDemographics = new ReferralDemographicsModel
            {
                ReferralID = -1,
                FirstName = "FirstName",
                LastName = "LastName",
                SuffixID = -1,
                TitleID = -1,
                Middle = "M",
                ForceRollback = true
            };

            // Act
            var demographicsResponse = communicationManager.Post<ReferralDemographicsModel, Response<ReferralDemographicsModel>>(referralDemographics, urlDemographics);

            // Assert
            Assert.IsTrue(demographicsResponse != null, "Response can't be null");
            Assert.IsTrue(demographicsResponse.RowAffected < 3, "Invalid referral demographics has been updated.");

            //Header

            // Arrange
            var urlHeader = baseRouteHeader + "UpdateReferralHeader";
            var referralHeader = new ReferralHeaderModel
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
            var headerResponse = communicationManager.Put<ReferralHeaderModel, Response<ReferralHeaderModel>>(referralHeader, urlHeader);

            // Assert
            Assert.IsTrue(headerResponse != null, "Response can't be null");
            Assert.IsTrue(headerResponse.RowAffected < 3, "Invalid referral header has been updated.");

            //Address

            // Arrange
            var urlAddress = baseRouteAddress + "AddUpdateAddress";
            var referralAddress = new List<ContactAddressModel>();
            referralAddress.Add(new ContactAddressModel
            {
                ContactID = -1,
                AddressID = -1,
                MailPermissionID = 1,
                AddressTypeID = 1,
                Line1 = "Line1",
                Line2 = "Line2",
                City = "City",
                StateProvince = 1,
                County = 1,
                Zip = "12321-3212",
                ForceRollback = true
            });

            // Act
            var addressResponse = communicationManager.Post<List<ContactAddressModel>, Response<List<ContactAddressModel>>>(referralAddress, urlAddress);

            // Assert
            Assert.IsTrue(addressResponse != null, "Response can't be null");
            Assert.IsTrue(addressResponse.RowAffected < 3, "Invalid referral address has been updated.");

            //Email

            // Arrange
            var urlEmail = baseRouteEmail + "AddUpdateEmail";
            var referralEmail = new List<ContactEmailModel>();
            referralEmail.Add(new ContactEmailModel
            {
                ContactID = -1,
                EmailID = -1,
                EmailPermissionID = 1,
                Email = "Email@Email.Email",
                ForceRollback = true
            });

            // Act
            var emailResponse = communicationManager.Post<List<ContactEmailModel>, Response<List<ContactEmailModel>>>(referralEmail, urlEmail);

            // Assert
            Assert.IsTrue(emailResponse != null, "Response can't be null");
            Assert.IsTrue(emailResponse.RowAffected < 3, "Invalid referral email has been updated.");

            //Phone

            // Arrange
            var urlPhone = baseRoutePhone + "AddUpdateContactPhones";
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
            var phoneResponse = communicationManager.Post<ContactPhoneModel, Response<ContactPhoneModel>>(referralPhone, urlPhone);

            // Assert
            Assert.IsTrue(phoneResponse != null, "Response can't be null");
            Assert.IsTrue(phoneResponse.RowAffected < 2, "Invalid referral phone has been updated.");
        }
    }
}
