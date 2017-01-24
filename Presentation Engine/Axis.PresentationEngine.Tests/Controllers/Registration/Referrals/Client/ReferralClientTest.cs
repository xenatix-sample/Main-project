using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.ClientInformation;
using System.Configuration;
using Axis.Model.Registration.Referrals;
using Axis.Model.Common;
using System.Collections.Generic;
using Axis.Model.Registration;

namespace Axis.PresentationEngine.Tests.Controllers.Registration.Referrals.Client
{
    [TestClass]
    public class ReferralClientTest
    {
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "client/";

        /// <summary>
        /// The referral header identifier
        /// </summary>
        private long referralHeaderID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private ReferralClientInformationController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new ReferralClientInformationController(new ReferralClientInformationRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Get Referral Client Information success unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralClientInformation_Success()
        {
            // Act
            var response = controller.GetClientInformation(referralHeaderID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one client information must exist.");
        }

        /// <summary>
        /// Get Referral Client Information failed unit test.
        /// </summary>
        [TestMethod]
        public void GetReferralClientInformation_Failed()
        {
            // Act
            var response = controller.GetClientInformation(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Referral Client Information should not exist for this test case.");
        }

        /// <summary>
        /// Add Referral Client Information success unit test.
        /// </summary>
        [TestMethod]
        public void AddReferralClientInformation_Success()
        {
            // Arrange
            var referralClient = new ReferralClientInformationModel
            {
                clientDemographicsModel = new ContactDemographicsModel
            {
                FirstName = "Unit",
                LastName = "Test",
                Middle = "M",
                SuffixID = 2,
                TitleID = 3,
                ContactMethodID = 2,
                GestationalAge = 3,
                ForceRollback = true
            },
                referralClientAdditionalDetails = new ReferralClientAdditionalDetailsModel
                {
                    ReferralHeaderID = 1,
                    ReasonforCare = "Unit Test Reason",
                    IsTransferred = true,
                    IsHousingProgram = true,
                    HousingDescription = "Unit Test Housing",
                    IsEligibleforFurlough = true,
                    IsReferralDischargeOrTransfer = true,
                    IsConsentRequired = true,
                    Comments = "Unit Test Comments",
                    ForceRollback = true
                },
                Concern = new ReferralClientConcernsModel { ReferralConcernID = 1, Diagnosis = "Diagnosis1", ReferralPriorityID = 1, ForceRollback = true },
                Addresses = new List<ContactAddressModel> {
                                   new ContactAddressModel() { AddressTypeID=1, Line1 = "Line1", Line2 = "Line2", City = "City", StateProvince = 5, Zip = "75038", ForceRollback = true}
                                   //new ReferralAddressModel() { AddressTypeID=1, Line1 = "Address Line1", Line2 = "Address Line2", City = "City", StateProvince = 10, Zip = "75038", ForceRollback = true}
                 },

                Phones = new List<ContactPhoneModel> {
                                    new ContactPhoneModel() { PhoneTypeID =1, Number = "123-456-7890", PhonePermissionID = 1, ForceRollback = true},
                                    new ContactPhoneModel() { PhoneTypeID =2, Number = "333-456-7890", PhonePermissionID = 2, ForceRollback = true}
                 }
            };

            // Act
            var response = controller.AddClientInformation(referralClient);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code should be 0");
        }


    }
}
