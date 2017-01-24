using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers.Registration.Collateral
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class CollateralLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;


        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "collateral/";


        /// <summary>
        /// The default contact identifier
        /// </summary>
        private long defaultContactID = 3;


        /// <summary>
        /// The default contact type identifier
        /// </summary>
        private int defaultContactTypeID = 4;


        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteContactID = 11;

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
        /// Gets collateral - success test case
        /// </summary>
        [TestMethod]
        public void GetCollaterals_Success()
        {
            //Arrange
            var url = baseRoute + "getCollaterals";
            var param = new NameValueCollection();
            param.Add("contactID", defaultContactID.ToString());
            param.Add("contactTypeId", defaultContactTypeID.ToString());

            //Act
            var response = communicationManager.Get<Response<CollateralModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Collateral must exists.");
        }

        /// <summary>
        /// Gets collateral - failure test case
        /// </summary>
        [TestMethod]
        public void GetCollaterals_Failure()
        {
            //Arrange
            var url = baseRoute + "getCollaterals";
            var param = new NameValueCollection();
            param.Add("contactID", "-1");
            param.Add("contactTypeId", defaultContactTypeID.ToString());

            //Act
            var response = communicationManager.Get<Response<CollateralModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Collateral exists.");
        }

        /// <summary>
        /// Add collateral - success test case
        /// </summary>
        [TestMethod]
        public void AddCollateral_Success()
        {
            //Arrange
            var url = baseRoute + "addCollateral";

            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel
            {
                AddressTypeID = 2,
                Line1 = "Address Line1",
                Line2 = "AddressLine2",
                City = "Colorado",
                County = 1,
                StateProvince = 2,
                Zip = "zipCode",
                MailPermissionID = 1
            });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel
            {
                PhoneTypeID = 2,
                PhonePermissionID = 3,
                Number = "9992225553"
            });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel
            {
                Email = "unittest@rsystems.com"
            });

            var addCollateral = new CollateralModel
            {
                ParentContactID = 3,
                ContactID = 0,
                ContactTypeID = 4,
                DriverLicense = "123456",
                DriverLicenseStateID = 10,
                ContactRelationshipID = 1,
                LivingWithClientStatus = true,
                ReceiveCorrespondenceID = 1,
                FirstName = "firstName",
                LastName = "lastName",
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
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Collateral could not be created.");
            Assert.IsTrue(response.RowAffected > 0, "Collateral could not be created.");
        }

        /// <summary>
        /// Add collateral - failure test case.
        /// </summary>
        [TestMethod]
        public void AddCollateral_Failure()
        {
            //Arrange
            var url = baseRoute + "addCollateral";

            //Add Emergency Contact
            var contactAddressModel = new List<ContactAddressModel>();
            contactAddressModel.Add(new ContactAddressModel
            {
                AddressTypeID = 2,
                Line1 = "Address Line1",
                Line2 = "AddressLine2",
                City = "Colorado",
                County = 1,
                StateProvince = 2,
                Zip = "zipCode",
                MailPermissionID = 1
            });

            var contactPhoneModel = new List<ContactPhoneModel>();
            contactPhoneModel.Add(new ContactPhoneModel
            {
                PhoneTypeID = 2,
                PhonePermissionID = 3,
                Number = "9992225553"
            });

            var contactEmailModel = new List<ContactEmailModel>();
            contactEmailModel.Add(new ContactEmailModel
            {
                Email = "unittest@rsystems.com"
            });

            var addCollateralFailure = new CollateralModel
            {
                ContactTypeID = -1,
                DriverLicense = "driverLicense",
                DriverLicenseStateID = 10,
                ContactRelationshipID = 1,
                LivingWithClientStatus = true,
                ReceiveCorrespondenceID = 1,
                FirstName = "firstName",
                LastName = "lastName",
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
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode != 0, "Collateral created.");
        }

        /// <summary>
        /// Update collateral - success test case.
        /// </summary>
        [TestMethod]
        public void UpdateCollateral_Success()
        {
            //Arrange
            var url = baseRoute + "updateCollateral";

            //Update Additional Demographic
            var updateCollateral = new CollateralModel
            {
                ContactID = 11,
                ContactTypeID = 4,
                FirstName = "firstName1",
                LastName = "lastName1",
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID =2,
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
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Collateral could not be updated.");
            Assert.IsTrue(response.RowAffected > 0, "Collateral could not be updated.");
        }

        /// <summary>
        /// Update collateral - failure test case.
        /// </summary>
        [TestMethod]
        public void UpdateCollateral_Failure()
        {
            //Arrange
            var url = baseRoute + "updateCollateral";

            //Update Additional Demographic
            var updateCollateralFailure = new CollateralModel
            {
                ContactID = -1,
                ContactTypeID = 4,
                FirstName = "firstName1",
                LastName = "lastName1",
                LivingWithClientStatus = false,
                ReceiveCorrespondenceID =2,
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
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "Collateral updated.");
        }

        /// <summary>
        /// Delete collateral - success test case.
        /// </summary>
        [TestMethod]
        public void DeleteCollateral_Success()
        {
            //Arrange
            var url = baseRoute + "deleteCollateral";
            var param = new NameValueCollection();
            param.Add("Id", defaultDeleteContactID.ToString());

            //Act
            var response = communicationManager.Delete<Response<CollateralModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Collateral could not be deleted.");
            Assert.IsTrue(response.RowAffected > 0, "Collateral could not be deleted.");
        }

        /// <summary>
        /// Delete collateral - failure test case.
        /// </summary>
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
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "Collateral deleted.");
        }
    }
}