using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using Axis.Model.Registration.Referrals;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System.Collections.Generic;
using Axis.Model.Registration.Referrals.Common;
using Axis.Model.Registration;

namespace Axis.DataEngine.Tests.Controllers.Registration.Referrals.ClientInformation
{
    [TestClass]
    public class ClientInformationLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ReferralClientInformation/";

        
        /// <summary>
        /// The request model
        /// </summary>
        //private ReferralClientInformationModel requestModel = null;
        private long referralHeaderID = 1;
        
        #endregion Class Variables

        #region Model objects
        private ReferralClientInformationModel addClientInformation = new ReferralClientInformationModel
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
            Concern = new ReferralClientConcernsModel{ ReferralConcernID = 1, Diagnosis = "Diagnosis1", ReferralPriorityID = 1, ForceRollback = true },
            Addresses = new List<ContactAddressModel> {
                                   new ContactAddressModel() { AddressTypeID=1, Line1 = "Line1", Line2 = "Line2", City = "City", StateProvince = 5, Zip = "75038", ForceRollback = true}
                                   //new ReferralAddressModel() { AddressTypeID=1, Line1 = "Address Line1", Line2 = "Address Line2", City = "City", StateProvince = 10, Zip = "75038", ForceRollback = true}
                 },

            Phones = new List<ContactPhoneModel> {
                                    new ContactPhoneModel() { PhoneTypeID =1, Number = "123-456-7890", PhonePermissionID = 1, ForceRollback = true},
                                    new ContactPhoneModel() { PhoneTypeID =2, Number = "333-456-7890", PhonePermissionID = 2, ForceRollback = true}
                 }

        };
        #endregion

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetClientInformation_Success()
        {
            var url = baseRoute + "GetClientInformation";

            var param = new NameValueCollection();
            param.Add("referralHeaderID", referralHeaderID.ToString());

            var response = communicationManager.Get<Response<ReferralClientInformationModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one additional demography must exists.");
        }

        [TestMethod]
        public void AddClientInformation_Success()
        {
            var url = baseRoute + "AddClientInformation";

            var param = new NameValueCollection();

            var response = communicationManager.Post<ReferralClientInformationModel, Response<ReferralClientInformationModel>>(addClientInformation, url);

            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.ResultCode == 0, "Contact benefit could not be updated.");
        }


    }
}
