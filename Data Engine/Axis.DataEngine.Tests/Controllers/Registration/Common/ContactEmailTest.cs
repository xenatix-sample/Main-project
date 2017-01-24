using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Plugins.Registration;
using Axis.DataProvider.Registration.Common;
using Axis.Model.Common;
using Axis.Model.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataEngine.Tests.Controllers.Registration.Common
{
    /// <summary>
    /// Contact Email: MOC test
    /// </summary>
    [TestClass]
    public class ContactEmailTest
    {

        /// <summary>
        ///  contact eamil data provider
        /// </summary>
        private IContactEmailDataProvider contactEmailDataProvider;

        /// <summary>
        ///  contact identifier
        /// </summary>
        private long contactId = 1;

        /// <summary>
        ///  contact email model
        /// </summary>
        private ContactEmailModel contactEmailsModel;        
    

        /// <summary>
        /// Mock_s  contact Emails_ success.
        /// </summary>
        public void Mock_ContactEmails_Success()
        {
            Mock<IContactEmailDataProvider> mock = new Mock<IContactEmailDataProvider>();
            contactEmailDataProvider = mock.Object;

            var contactEmails = new List<ContactEmailModel>();
            contactEmailsModel = new ContactEmailModel()
            {
                ContactID = 1,
                ContactEmailID = 0,
                Email = "test@xenatix.com",
                EmailID = 1,
                EmailPermissionID = 1,
                IsPrimary = true,
            };
            contactEmails.Add(contactEmailsModel);
            var contactEmail = new Response<ContactEmailModel>()
            {
                DataItems = contactEmails
            };

            //Get contactEmails
            Response<ContactEmailModel> contactEmailsResponse = new Response<ContactEmailModel>();
            contactEmailsResponse.DataItems = contactEmails.Where(contact => contact.ContactID == contactId).ToList();

            mock.Setup(r => r.GetEmails(It.IsAny<long>(), It.IsAny<int>()))
                .Returns(contactEmailsResponse);

            //Add contactEmails
            mock.Setup(r => r.AddEmails(It.IsAny<long>(), It.IsAny<List<ContactEmailModel>>()))
                 .Callback<long, List<ContactEmailModel>>((i, p) => contactEmails.AddRange(p))
                .Returns(contactEmail);

            //Update contactEmails
            mock.Setup(r => r.UpdateEmails(It.IsAny<long>(), It.IsAny<List<ContactEmailModel>>()))
                .Callback<long, List<ContactEmailModel>>((i, p) => contactEmails.AddRange(p))
                .Returns(contactEmail);

            //Delete Contact Email
            mock.Setup(r => r.DeleteEmail(It.IsAny<long>()))
                .Callback<long>(t => contactEmails.Remove(contactEmails.Find(c => c.ContactEmailID == t)))
                .Returns(contactEmail);
        }

        /// <summary>
        /// Mock_s  contact Emails_ failed.
        /// </summary>
        public void Mock_ContactEmails_Failed()
        {
            Mock<IContactEmailDataProvider> mock = new Mock<IContactEmailDataProvider>();
            contactEmailDataProvider = mock.Object;

            var contactEmails = new List<ContactEmailModel>();
            contactEmailsModel = new ContactEmailModel()
            {
                ContactID = -1,
                ContactEmailID = -1,
                Email = "test@xenatix.com",
                EmailID = 0,
                EmailPermissionID = 0,
                IsPrimary = true,
            };
            contactEmails.Add(contactEmailsModel);
            var contactEmail = new Response<ContactEmailModel>()
            {
                DataItems = contactEmails
            };

            //Get contactEmails
            Response<ContactEmailModel> contactEmailsResponse = new Response<ContactEmailModel>();
            contactEmailsResponse.DataItems = contactEmails.Where(contact => contact.ContactID == contactId).ToList();

            mock.Setup(r => r.GetEmails(It.IsAny<long>(), It.IsAny<int>()))
                .Returns(contactEmailsResponse);

            //Add contactEmails
            mock.Setup(r => r.AddEmails(It.IsAny<long>(), It.IsAny<List<ContactEmailModel>>()))
                 .Callback<long, List<ContactEmailModel>>((i, p) => contactEmails.AddRange(p))
                .Returns(contactEmail);

            //Update contactEmails
            mock.Setup(r => r.UpdateEmails(It.IsAny<long>(), It.IsAny<List<ContactEmailModel>>()))
                .Callback<long, List<ContactEmailModel>>((i, p) => contactEmails.AddRange(p))
                .Returns(contactEmail);

            //Delete Contact Email
            mock.Setup(r => r.DeleteEmail(It.IsAny<long>()))
                 .Callback<long>(t => contactEmails.Remove(contactEmails.Find(c => c.ContactEmailID == t)))
                .Returns(contactEmail);
        }

        /// <summary>
        /// Test  Contact email for success.
        /// </summary>
        [TestMethod]
        public void GetcontactEmail_Success()
        {
            Mock_ContactEmails_Success();
            // Arrange
            ContactEmailController contactEmailsController = new ContactEmailController(contactEmailDataProvider);

            //Act
            var getcontactEmailsResult = contactEmailsController.GetEmails(contactId, 1);
            var response = getcontactEmailsResult as HttpResult<Response<ContactEmailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.Value, "Response value cann't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.DataItems.Count() > 0, "Atleast one Contact Email must exists.");
        }

        /// <summary>
        /// Test  Contact email for failed.
        /// </summary>
        [TestMethod]
        public void GetcontactEmail_Failed()
        {
            Mock_ContactEmails_Failed();
            // Arrange
            ContactEmailController contactEmailsController = new ContactEmailController(contactEmailDataProvider);

            //Act
            var getcontactEmailsResult = contactEmailsController.GetEmails(-1, 0);
            var response = getcontactEmailsResult as HttpResult<Response<ContactEmailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.Value, "Record does not exist");
            Assert.IsNotNull(response.Value.DataItems, "Record does not exist");
            Assert.IsTrue(response.Value.DataItems.Count == 0, "Record does not exist");
            
        }

        /// <summary>
        /// Adds  Contact email for success.
        /// </summary>
        [TestMethod]
        public void AddcontactEmail_Success()
        {
            Mock_ContactEmails_Success();
            // Arrange
            ContactEmailController contactEmailsController = new ContactEmailController(contactEmailDataProvider);

            //Act
            List<ContactEmailModel> contactEmails = new List<ContactEmailModel>();
            contactEmails.Add(contactEmailsModel);
            var savecontactEmailsResult = contactEmailsController.AddUpdateEmails(contactEmails);
            var response = savecontactEmailsResult as HttpResult<Response<ContactEmailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.Value, "Response value cann't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data inserted");
        }

        /// <summary>
        /// Adds  Contact email for failed.
        /// </summary>
        [TestMethod]
        public void AddContactEmail_Failed()
        {
            Mock_ContactEmails_Failed();
            // Arrange
            ContactEmailController contactEmailsController = new ContactEmailController(contactEmailDataProvider);

            //Act
            List<ContactEmailModel> contactEmails = new List<ContactEmailModel>();
            contactEmails.Add(contactEmailsModel);
            var savecontactEmailsResult = contactEmailsController.AddUpdateEmails(contactEmails);
            var response = savecontactEmailsResult as HttpResult<Response<ContactEmailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.Value, "Response value cann't be null");
            Assert.IsNotNull(response.Value.DataItems, "Email inserted for wrong data");

        }

        /// <summary>
        /// Updates  Contact email for success.
        /// </summary>
        [TestMethod]
        public void UpdateContactEmail_Success()
        {
            Mock_ContactEmails_Success();
            // Arrange
            ContactEmailController contactEmailsController = new ContactEmailController(contactEmailDataProvider);

            //Act
            List<ContactEmailModel> contactEmails = new List<ContactEmailModel>();
            contactEmails.Add(contactEmailsModel);
            var savecontactEmailsResult = contactEmailsController.AddUpdateEmails(contactEmails);
            var response = savecontactEmailsResult as HttpResult<Response<ContactEmailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.Value, "Response value cann't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data item cann't be null");
        }

        /// <summary>
        /// Updates  Contact email for failed.
        /// </summary>
        [TestMethod]
        public void UpdateContactEmail_Failed()
        {
            Mock_ContactEmails_Failed();
            // Arrange
            ContactEmailController contactEmailsController = new ContactEmailController(contactEmailDataProvider);

            //Act
            List<ContactEmailModel> contactEmails = new List<ContactEmailModel>();
            contactEmails.Add(contactEmailsModel);
            var savecontactEmailsResult = contactEmailsController.AddUpdateEmails(contactEmails);
            var response = savecontactEmailsResult as HttpResult<Response<ContactEmailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.Value, "Response value cann't be null");
            Assert.IsNotNull(response.Value.DataItems, "Data item cann't be null");

        }


        /// <summary>
        /// Delete  Contact email for success.
        /// </summary>
        [TestMethod]
        public void DeleteContactEmail_Success()
        {
            Mock_ContactEmails_Success();
            // Arrange
            ContactEmailController contactEmailsController = new ContactEmailController(contactEmailDataProvider);

            //Act           
            var savecontactEmailsResult = contactEmailsController.DeleteEmail(contactId);
            var response = savecontactEmailsResult as HttpResult<Response<ContactEmailModel>>;
            var contactEmail = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.Value, "Response value cann't be null");
            Assert.IsNotNull(response.Value.RowAffected, "contact email could not be deleted.");
        }

        /// <summary>
        /// Delete  Contact email for failed.
        /// </summary>
        [TestMethod]
        public void DeleteContactEmail_Failed()
        {
            Mock_ContactEmails_Failed();
            // Arrange
            ContactEmailController contactEmailsController = new ContactEmailController(contactEmailDataProvider);

            //Act           
            var savecontactEmailsResult = contactEmailsController.DeleteEmail(-1);
            var response = savecontactEmailsResult as HttpResult<Response<ContactEmailModel>>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.Value, "Response value cann't be null");
            Assert.AreEqual(0, response.Value.RowAffected, "Row affected should greater than 0");

        }
    }
}
