using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers;
using System.Configuration;
using Axis.Plugins.Registration.Repository;
using System.Web.Mvc;
using Axis.Model.Common;
using System.Collections.Generic;
using Axis.Plugins.Registration.Models;
using Axis.Model.Registration;

namespace Axis.PresentationEngine.Tests.Controllers.Registration.EmergencyContact
{
    [TestClass]
    public class EmergencyContactTest
    {
        private int defaultContactId = 3;
        private int defaultcontactTypeId = 3;
        private long defaultDeleteContactId = 7;
        private EmergencyContactController controller = null;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            controller = new EmergencyContactController(new EmergencyContactRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetEmergencyContacts_Success()
        {
            // Act
            var response = controller.GetEmergencyContacts(defaultContactId, defaultcontactTypeId).Result;
            var count=response.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0, "Atleast one emergency contact must exists.");
        }

        [TestMethod]
        public void GetEmergencyContacts_Failure()
        {
            // Act
            var response = controller.GetEmergencyContacts(-1, -1).Result;
            var count = response.DataItems.Count;

            // Assert
            Assert.IsTrue(count == 0, "Atleast one emergency contact exists.");
        }

        [TestMethod]
        public void AddEmergencyContact_Success()
        {
            // Act
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var addEmergencyContact = new EmergencyContactViewModel
            {
                ParentContactID=3,
                ContactID = 0,
                ContactTypeID = 3,
                FirstName="firstName11",
                LastName="lastName11",
                GenderID=1,
                DOB=DateTime.Now,
                SuffixID=2,
                ContactRelationshipID = 1,
                RelationshipTypeID = 2,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                IsActive=true,
                ModifiedBy=5,
                ModifiedOn=DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddEmergencyContact(addEmergencyContact);

            // Assert
            Assert.IsTrue(response.RowAffected > 0, "Emergency contact could not be created.");
        }

        [TestMethod]
        public void AddEmergencyContact_Failure()
        {
            // Act
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var addEmergencyContactFailure = new EmergencyContactViewModel
            {
                ParentContactID = -1,
                ContactID = -1,
                ContactTypeID = -1,
                FirstName = "firstName",
                LastName = "lastName",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                ContactRelationshipID = 1,
                RelationshipTypeID = 2,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddEmergencyContact(addEmergencyContactFailure);

            // Assert
            Assert.IsTrue(response.ResultCode != 0);
            Assert.IsTrue(response.RowAffected == 0, "Emergency contact could be created.");
        }

        [TestMethod]
        public void UpdateAdditionalDemographic_Success()
        {
            // Act
            var updateEmergencyContact = new EmergencyContactViewModel
            {
                ParentContactID = 0,
                ContactID = 20,
                ContactTypeID = 3,
                FirstName = "firstName1",
                LastName = "lastName1",
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateEmergencyContact(updateEmergencyContact);

            // Assert
            Assert.IsTrue(response.RowAffected > 0, "Emergency contact could not be updated.");
        }

        [TestMethod]
        public void UpdateAdditionalDemographic_Failure()
        {
            // Act
            var updateEmergencyContactFailure = new EmergencyContactViewModel
            {
                ParentContactID = 0,
                ContactID = -1,
                ContactTypeID = 3,
                FirstName = "firstName1",
                LastName = "lastName1",
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateEmergencyContact(updateEmergencyContactFailure);

            // Assert
            Assert.IsTrue(response.RowAffected == 0, "Emergency contact could be updated.");
        }

        [TestMethod]
        public void DeleteEmergencyContact_Success()
        {
            // Act
            var response = controller.DeleteEmergencyContact(defaultDeleteContactId, DateTime.UtcNow);

            // Assert
            Assert.IsTrue(response.RowAffected > 0, "Emergency contact could not be deleted.");
        }

        [TestMethod]
        public void DeleteEmergencyContact_Failure()
        {
            // Act
            var response = controller.DeleteEmergencyContact(-1, DateTime.UtcNow);

            // Assert
            Assert.IsTrue(response.RowAffected == 0, "Emergency contact could be deleted.");
        }
    }
}
