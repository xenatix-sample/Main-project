using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Collections.Generic;

namespace Axis.RuleEngine.Tests.Controllers.Registration.Collateral
{
    [TestClass]
    public class CollateralLiveTest
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "collateral/";
        private long _defaultContactID = 50;
        private int _defaultContactTypeID = 4;
        private long _defaultDeleteContactID = 51;

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetCollaterals_Success()
        {
            //Arrange
            var url = baseRoute + "getCollaterals";
            var param = new NameValueCollection();
            param.Add("contactID", _defaultContactID.ToString());
            param.Add("contactTypeId", _defaultContactTypeID.ToString());

            //Act
            var response = communicationManager.Get<Response<CollateralModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Collateral must exists.");
        }

        [TestMethod]
        public void GetCollaterals_Failure()
        {
            //Arrange
            var url = baseRoute + "getCollaterals";
            var param = new NameValueCollection();
            param.Add("contactID", "-1");
            param.Add("contactTypeId", "-1");

            //Act
            var response = communicationManager.Get<Response<CollateralModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems.Count == 0, "Atleast one Collateral exists.");
        }

        [TestMethod]
        public void AddCollateral_Success()
        {
            //Arrange
            var url = baseRoute + "addCollateral";

            //Add Emergency Contact
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addCollateral = new CollateralModel
            {
                ParentContactID = 50,
                ContactID = 0,
                ContactTypeID = 4,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 10,
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
                IsActive = true,
                ModifiedBy = 5,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            //Act
            var response = communicationManager.Post<CollateralModel, Response<CollateralModel>>(addCollateral, url);

            //Assert
            Assert.IsTrue(response.RowAffected > 0, "Collateral could not be created.");
        }

        [TestMethod]
        public void AddCollateral_Failure()
        {
            //Arrange
            var url = baseRoute + "addCollateral";

            //Add Emergency Contact
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel { AddressID = 1, AddressTypeID = 2, Line1 = "Address Line1", Line2 = "AddressLine2", City = "Colorado", County = 1, StateProvince = 2, Zip = "zipCode" });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel { ContactPhoneID = 1, PhoneID = 1, PhoneTypeID = 2, PhonePermissionID = 3, Number = "9876458125" });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel { Email = "abc@rsystems.com" });

            var addCollateralFailure = new CollateralModel
            {
                ParentContactID = -1,
                ContactID = -1,
                ContactTypeID = -1,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 10,
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

            //Act
            var response = communicationManager.Post<CollateralModel, Response<CollateralModel>>(addCollateralFailure, url);

            //Assert
            Assert.IsTrue(response.ResultCode != 0);
            Assert.IsTrue(response.RowAffected == 0, "Collateral created.");
        }

        [TestMethod]
        public void UpdateCollateral_Success()
        {
            //Arrange
            var url = baseRoute + "updateCollateral";

            //Update Additional Demographic
            var updateCollateral = new CollateralModel
            {
                ParentContactID = 0,
                ContactID = 51,
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

            //Act
            var response = communicationManager.Put<CollateralModel, Response<CollateralModel>>(updateCollateral, url);

            //Assert
            Assert.IsTrue(response.ResultCode == 0, "Collateral could not be updated.");
        }

        [TestMethod]
        public void UpdateCollateral_Failure()
        {
            //Arrange
            var url = baseRoute + "updateCollateral";

            //Update Additional Demographic
            var updateCollateralFailure = new CollateralModel
            {
                ParentContactID = 0,
                ContactID = -1,
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

            //Act
            var response = communicationManager.Put<CollateralModel, Response<CollateralModel>>(updateCollateralFailure, url);

            //Assert
            Assert.IsTrue(response.ResultCode != 0, "Collateral updated.");
        }

        [TestMethod]
        public void DeleteCollateral_Success()
        {
            //Arrange
            var url = baseRoute + "deleteCollateral";
            var param = new NameValueCollection();
            param.Add("Id", _defaultDeleteContactID.ToString());

            //Act
            var response = communicationManager.Delete<Response<CollateralModel>>(param, url);

            //Assert
            Assert.IsTrue(response.ResultCode == 0, "Collateral could not be deleted.");
        }

        [TestMethod]
        public void DeleteCollateral_Failure()
        {
            //Arrange
            var url = baseRoute + "deleteCollateral";
            var param = new NameValueCollection();
            param.Add("Id", "-1");

            //Act
            var response = communicationManager.Delete<Response<CollateralModel>>(param, url);

            //Assert
            Assert.IsTrue(response.ResultCode != 0, "Collateral deleted.");
        }
    }
}
