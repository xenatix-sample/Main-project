using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.RuleEngine.Tests.Controllers.Registration.AdditionalDemography
{
    /// <summary>
    /// Contact Email live testing
    /// </summary>
    [TestClass]
    public class ContactEmailLiveTest
    {

        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ContactEmail/";
        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactId = 1;

        private ContactEmailModel requestModel;

        private List<ContactEmailModel> requestListModel;

        #endregion

        #region Test Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];

            requestModel = new ContactEmailModel
            {
                ContactID = 1,
                ContactEmailID = 1,
                Email = "test@xenatix.com",
                EmailID = 1,
                EmailPermissionID = 1,
                IsPrimary = true,
            };
            requestModel.ForceRollback = true;
            requestListModel = new List<ContactEmailModel>();
            requestListModel.Add(requestModel);

        }


        /// <summary>
        /// Gets the contact Email_ success.
        /// </summary>
        [TestMethod]
        public void GetContactEmail_Success()
        {
            //Arrenge
            var url = baseRoute + "GetEmails";
            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());
            param.Add("contactTypeID", "1");

            //Act
            var response = communicationManager.Get<Response<ContactEmailModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one contact email must exists.");
        }

        /// <summary>
        /// Gets the contact Email_ failed.
        /// </summary>
        [TestMethod]
        public void GetContactEmail_Failed()
        {
            //Arrenge
            var url = baseRoute + "GetEmails";
            var param = new NameValueCollection();
            param.Add("contactId", "0");
            param.Add("contactTypeID", "0");

            //Act
            var response = communicationManager.Get<Response<ContactEmailModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Record exist for invalid data");
        }


        /// <summary>
        /// Adds the contact Email_ success.
        /// </summary>
        [TestMethod]
        public void AddContactEmail_Success()
        {
            //Arrenge
            var url = baseRoute + "AddUpdateEmail";

            var requestModel = new ContactEmailModel
            {
                ContactID = 1,
                ContactEmailID = 1,
                Email = "test@xenatix.com",
                EmailID = 1,
                EmailPermissionID = 1,
                IsPrimary = true,
                ForceRollback = true
            };
            var requestListModel = new List<ContactEmailModel>();
            requestListModel.Add(requestModel);

            //Act
            var response = communicationManager.Post<List<ContactEmailModel>, Response<ContactEmailModel>>(requestListModel, url);

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsTrue(response.RowAffected > 0, "Contact Demography could not be created.");

        }

        /// <summary>
        /// Adds the contact Email_ failed.
        /// </summary>
        [TestMethod]
        public void AddContactEmail_Failed()
        {
            //Arrenge
            var url = baseRoute + "AddUpdateEmail";

            //Act
            var requestModel = new ContactEmailModel
            {
                ContactID = -1,
                ContactEmailID = -1,
                Email = "test@xenatix.com",
                EmailID = 1,
                EmailPermissionID = 1,
                IsPrimary = true,
                ForceRollback = true
            };
            var requestListModel = new List<ContactEmailModel>();
            requestListModel.Add(requestModel);

            var response = communicationManager.Post<List<ContactEmailModel>, Response<ContactEmailModel>>(requestListModel, url);

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsTrue(response.ResultCode <= 0, "Email added for invalid data.");
        }

        /// <summary>
        /// Updates the contact Email_ success.
        /// </summary>
        [TestMethod]
        public void UpdateContactEmail_Success()
        {
            //Arrenge
            var url = baseRoute + "AddUpdateEmail";

            //Act
            var requestModel = new ContactEmailModel
            {
                ContactID = 1,
                ContactEmailID = 1,
                Email = "test@xenatix.com",
                EmailID = 1,
                EmailPermissionID = 1,
                IsPrimary = true,
                ForceRollback = true
            };
            var requestListModel = new List<ContactEmailModel>();
            requestListModel.Add(requestModel);

            var response = communicationManager.Post<List<ContactEmailModel>, Response<ContactEmailModel>>(requestListModel, url);

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsTrue(response.RowAffected > 0, "Contact email could not be updated.");
        }

        /// <summary>
        /// Updates the contact Email_ failed.
        /// </summary>
        [TestMethod]
        public void UpdateContactEmail_Failed()
        {
            //Arrenge
            var url = baseRoute + "AddUpdateEmail";

            //Act
            var requestModel = new ContactEmailModel
            {
                ContactID = -1,
                ContactEmailID = -1,
                Email = "test@xenatix.com",
                EmailID = 1,
                EmailPermissionID = 1,
                IsPrimary = true,
                ForceRollback = true
            };
            var requestListModel = new List<ContactEmailModel>();
            requestListModel.Add(requestModel);

            var response = communicationManager.Post<List<ContactEmailModel>, Response<ContactEmailModel>>(requestListModel, url);

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsTrue(response.RowAffected > 0, "Email updated for invalid data.");
        }

        /// <summary>
        /// delete the contact Email_ success.
        /// </summary>
        [TestMethod]
        public void DeleteContactEmail_Success()
        {
            //Arrange
            var url = baseRoute + "DeleteEmail";

            //Act
            var param = new NameValueCollection();
            param.Add("contactEmailID", contactId.ToString());
            var response = communicationManager.Delete<Response<ContactEmailModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsTrue(response.RowAffected == 4, "Email deleted successfully"); //Audit in delete SP always affects 3 rows, so check is applied if 4th row is affected.
        }

        /// <summary>
        /// delete the contact Email_ failed.
        /// </summary>
        [TestMethod]
        public void DeleteContactEmail_Failed()
        {
            //Arrenge
            var url = baseRoute + "DeleteEmail";

            //Act
            var param = new NameValueCollection();
            param.Add("contactEmailID", "-1");
            var response = communicationManager.Delete<Response<ContactEmailModel>>(param, url);

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsTrue(response.RowAffected == 3, "Email deleted for invalid data"); //Audit in delete SP always affects 3 rows, so check is applied if 4th row is affected.
        }

        #endregion


    }
}