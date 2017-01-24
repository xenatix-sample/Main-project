using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.Controllers;
using Axis.Plugins.Registration.Repository;
using System.Web.Mvc;
using Axis.Plugins.Registration.Model;
using Axis.Model.Common;
using System.Collections.Generic;
using Axis.Model.Address;
using Axis.Model.Email;
using Axis.Model.Phone;
using Axis.Model.Registration;
using System.Configuration;


namespace Axis.PresentationEngine.Tests.Controllers
{
    /// <summary>
    /// Contact Email
    /// </summary>
    [TestClass]
    public class ContactEmailTest
    {
        /// <summary>
        ///  contact identifier
        /// </summary>
        private int contactId = 1;

        /// <summary>
        ///  token
        /// </summary>
        private string token = ConfigurationManager.AppSettings["UnitTestToken"];

        /// <summary>
        /// contactEmail view model
        /// </summary>
        private ContactEmailViewModel contactEmailViewModel = null;

        /// <summary>
        /// Gets contactEmail success.
        /// </summary>
        [TestMethod]
        public void GetContactEmail_Success()
        {
            // Arrange
            var controller = new ContactEmailController(new ContactEmailRepository(token));

            // Act
            var result = controller.GetEmails(contactId);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var response = data as Response<ContactEmailViewModel>;           

           
            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one contact email must exists.");
        }

        /// <summary>
        /// Gets contactEmail failed.
        /// </summary>
        [TestMethod]
        public void GetContactEmail_Failed()
        {
            // Arrange
            var controller = new ContactEmailController(new ContactEmailRepository(token));

            // Act
            var result = controller.GetEmails(0);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var response = data as Response<ContactEmailViewModel>;
           
            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Record exist for invalid data");
        }
               

        /// <summary>
        /// Saves contactEmail success.
        /// </summary>
        [TestMethod]
        public void SaveContactEmail_Success()
        {
            // Arrange
            contactEmailViewModel = new ContactEmailViewModel
            {
                ContactID = 1,
                ContactEmailID = 0,
                Email = "test@xenatix.com",
                EmailID = 1,
                EmailPermissionID = 1,
                IsPrimary = true,
                ForceRollback = true
            };

            var controller = new ContactEmailController(new ContactEmailRepository(token));

            // Act

            var result = controller.AddUpdateEmail(contactEmailViewModel);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var response = data as Response<ContactEmailViewModel>;           

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsNotNull(response.DataItems, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Insert success");
            Assert.IsTrue(response.RowAffected > 0, "Contact Email could not be created.");
        }

        /// <summary>
        /// Saves contactEmail failed.
        /// </summary>
        [TestMethod]
        public void SaveContactEmail_Failed()
        {
            // Arrange
            contactEmailViewModel = new ContactEmailViewModel
            {
                ContactID = -1,
                ContactEmailID = 0,
                Email = "test@xenatix.com",
                EmailID = 0,
                EmailPermissionID = 0,
                IsPrimary = true,
                ForceRollback = true
            };
            var controller = new ContactEmailController(new ContactEmailRepository(token));

            // Act

            var result = controller.AddUpdateEmail(contactEmailViewModel);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var response = data as Response<ContactEmailViewModel>;
            

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");            
            Assert.IsTrue(response.ResultCode > 0, "Failed to insert");
            Assert.IsTrue(response.RowAffected > 0, "Email added for invalid data.");
        }

        /// <summary>
        /// Updates contactEmail success.
        /// </summary>
        [TestMethod]
        public void UpdateContactEmail_Success()
        {
            // Arrange
            var controller = new ContactEmailController(new ContactEmailRepository(token));

            // Act
            var contactEmailViewModel = new ContactEmailViewModel
            {
                ContactID = 1,
                ContactEmailID = 1,
                Email = "test@xenatix.com",
                EmailID = 1,
                EmailPermissionID = 1,
                IsPrimary = true,
                ForceRollback = true
            };
            var result = controller.AddUpdateEmail(contactEmailViewModel);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var response = data as Response<ContactEmailViewModel>;
           
            //Assert
            Assert.IsNotNull(response, "Response cann't be null");            
            Assert.IsTrue(response.ResultCode == 0, "update success");
            Assert.IsTrue(response.RowAffected > 0, "Contact Email could not be updated.");
        }

        /// <summary>
        /// Updates contactEmail failed.
        /// </summary>
        [TestMethod]
        public void UpdateContactEmail_Failed()
        {
            // Arrange
            var controller = new ContactEmailController(new ContactEmailRepository(token));

            // Act
            var contactEmailViewModel = new ContactEmailViewModel
            {
                ContactID = 0,
                ContactEmailID = -1,
                Email = "test@xenatix.com",
                EmailID = 0,
                EmailPermissionID = 0,
                IsPrimary = true,
                ForceRollback = true
            };
            var result = controller.AddUpdateEmail(contactEmailViewModel);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var response = data as Response<ContactEmailViewModel>;
          

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");  
            Assert.IsTrue(response.ResultCode > 0, "Failed to update");
            Assert.IsTrue(response.RowAffected > 0, "Email updated for invalid data.");
        }


        /// <summary>
        /// delete contactEmail success.
        /// </summary>
        [TestMethod]
        public void DeleteContactEmail_Success()
        {
            // Arrange
            var controller = new ContactEmailController(new ContactEmailRepository(token));

            var result = controller.DeleteEmail(1);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var response = data as Response<ContactEmailViewModel>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsTrue(response.RowAffected == 4, "Email deleted successfully"); //Audit in delete SP always affects 3 rows, so check is applied if 4th row is affected.
        }

        /// <summary>
        /// delete contactEmail failed.
        /// </summary>
        [TestMethod]
        public void DeleteContactEmail_Failed()
        {
            // Arrange
            var controller = new ContactEmailController(new ContactEmailRepository(token));

            // Act          
            var result = controller.DeleteEmail(-1);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var response = data as Response<ContactEmailViewModel>;

            //Assert
            Assert.IsNotNull(response, "Response cann't be null");
            Assert.IsTrue(response.RowAffected == 3, "Email deleted for invalid data"); //Audit in delete SP always affects 3 rows, so check is applied if 4th row is affected.
        }
    }
}
