using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Model;
using Axis.Plugins.Registration.ApiControllers;
using Axis.Plugins.Registration.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers.Registration.Collateral
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class CollateralTest
    {
        /// <summary>
        /// The default contact identifier
        /// </summary>
        private int defaultContactId = 1;

        /// <summary>
        /// The defaultcontact type identifier
        /// </summary>
        private int defaultcontactTypeId = 4;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteContactId = 7;

        /// <summary>
        /// The default delete parent contact identifier
        /// </summary>
        private long defaultDeleteParentContactId = 7;

        /// <summary>
        /// The controller
        /// </summary>
        private CollateralController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new CollateralController(new CollateralRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Gets the collaterals_ success.
        /// </summary>
        [TestMethod]
        public void GetCollaterals_Success()
        {
            // Act
            var response = controller.GetCollaterals(defaultContactId, defaultcontactTypeId, false).Result;
            var count = response.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0, "Atleast one Collateral must exists.");
        }

        /// <summary>
        /// Gets the collaterals_ failure.
        /// </summary>
        [TestMethod]
        public void GetCollaterals_Failure()
        {
            // Act
            var response = controller.GetCollaterals(-1, -1, false).Result;
            var count = response.DataItems.Count;

            // Assert
            Assert.IsTrue(count == 0, "Atleast one Collateral exists.");
        }

        /// <summary>
        /// Adds the collateral_ success.
        /// </summary>
        [TestMethod]
        public void AddCollateral_Success()
        {
            // Act
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "12345" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var addCollateral = new CollateralViewModel
            {
                ParentContactID = 1,
                ContactID = 0,
                ContactTypeID = 4,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 10,
                AlternateID = "alternateID",
                ClientIdentifierTypeID = 6,
                ContactRelationshipID = 1,
                LivingWithClientStatus = true,
                ReceiveCorrespondenceID = 1,
                FirstName = "firstName121",
                LastName = "lastName121",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                RelationshipTypeID = 2,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true,
            };

            var response = controller.AddCollateral(addCollateral);

            // Assert
            Assert.IsTrue(response.RowAffected > 0, "Collateral could not be created.");
        }

        /// <summary>
        /// Adds the collateral_ failure.
        /// </summary>
        [TestMethod]
        public void AddCollateral_Failure()
        {
            // Act
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addCollateralFailure = new CollateralViewModel
            {
                ParentContactID = -1,
                ContactID = -1,
                ContactTypeID = -1,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 10,
                AlternateID = "alternateID",
                ClientIdentifierTypeID = 6,
                ContactRelationshipID = 1,
                LivingWithClientStatus = true,
                ReceiveCorrespondenceID = 1,
                FirstName = "firstName11",
                LastName = "lastName11",
                GenderID = 1,
                DOB = DateTime.Now,
                SuffixID = 2,
                RelationshipTypeID = 2,
                Addresses = contactAddressModel,
                Phones = contactPhoneModel,
                Emails = contactEmailModel,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddCollateral(addCollateralFailure);

            // Assert
            Assert.IsTrue(response.ResultCode != 0);
            Assert.IsTrue(response.RowAffected == 0, "Collateral created.");
        }

        /// <summary>
        /// Updates the collateral_ success.
        /// </summary>
        [TestMethod]
        public void UpdateCollateral_Success()
        {
            // Act
            var updateCollateral = new CollateralViewModel
            {
                ParentContactID = 0,
                ContactID = 1,
                ContactTypeID = 4,
                FirstName = "firstName1",
                LastName = "lastName1",
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID = 2,
                ContactRelationshipID = 2,
                RelationshipTypeID = 3,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateCollateral(updateCollateral);

            // Assert
            Assert.IsTrue(response.RowAffected > 0, "Collateral could not be updated.");
        }

        /// <summary>
        /// Updates the collateral_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateCollateral_Failure()
        {
            // Act
            var updateCollateralFailure = new CollateralViewModel
            {
                ParentContactID = 0,
                ContactID = -1,
                ContactTypeID = 4,
                FirstName = "firstName1",
                LastName = "lastName1",
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID = 1,
                ContactRelationshipID = 2,
                RelationshipTypeID = 3,
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateCollateral(updateCollateralFailure);

            // Assert
            Assert.IsTrue(response.RowAffected == 0, "Collateral updated.");
        }

        /// <summary>
        /// Deletes the collateral_ success.
        /// </summary>
        [TestMethod]
        public void DeleteCollateral_Success()
        {
            // Act
            var response = controller.DeleteCollateral(defaultDeleteParentContactId,defaultDeleteContactId, DateTime.UtcNow);

            // Assert
            Assert.IsTrue(response.RowAffected > 0, "Collateral could not be deleted.");
        }

        /// <summary>
        /// Deletes the collateral_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteCollateral_Failure()
        {
            // Act
            var response = controller.DeleteCollateral(-1,-1, DateTime.UtcNow);

            // Assert
            Assert.IsTrue(response.RowAffected == 0, "Collateral deleted.");
        }
    }
}