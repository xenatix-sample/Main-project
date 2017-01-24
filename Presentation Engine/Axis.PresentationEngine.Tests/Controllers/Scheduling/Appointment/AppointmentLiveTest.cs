using Axis.Model.Common;
using Axis.Plugins.Scheduling.ApiControllers;
using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Repository.Appointment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers.Scheduling.Appointment
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class AppointmentLiveTest
    {
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "appointment/";

        /// <summary>
        /// The appointment identifier
        /// </summary>
        private long appointmentId = 17;

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactID = 1;

        /// <summary>
        /// The resource type identifier
        /// </summary>
        private short resourceTypeID = 2;

        /// <summary>
        /// The resource identifier
        /// </summary>
        private int resourceID = 5;

        /// <summary>
        /// The program identifier
        /// </summary>
        private long programId = 1;

        /// <summary>
        /// The appointment type identifier
        /// </summary>
        private long appointmentTypeID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private AppointmentController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new AppointmentController(new AppointmentRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Success test case for GetAppointments
        /// </summary>
        [TestMethod]
        public void GetAppointments_Success()
        {
            // Act
            var result = controller.GetAppointments(contactID);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one appointment must exists.");
        }

        /// <summary>
        /// Faliure test case for GetAppointments
        /// </summary>
        [TestMethod]
        public void GetAppointments_Failed()
        {
            var result = controller.GetAppointments(-1);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Appointment should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetAppointment
        /// </summary>
        [TestMethod]
        public void GetAppointment_Success()
        {
            // Act
            var result = controller.GetAppointment(appointmentId);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one appointment must exists.");
        }

        /// <summary>
        /// Failure test case for GetAppointment
        /// </summary>
        [TestMethod]
        public void GetAppointment_Failed()
        {
            // Act
            var result = controller.GetAppointment(-1);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Appointment should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetAppointmentByResource
        /// </summary>
        [TestMethod]
        public void GetAppointmentByResource_Success()
        {
            // Act
            var result = controller.GetAppointmentByResource(resourceID, resourceTypeID);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one appointment must exists.");
        }

        /// <summary>
        /// Failure test case for GetAppointmentByResource
        /// </summary>
        [TestMethod]
        public void GetAppointmentByResource_Failed()
        {
            // Act
            var result = controller.GetAppointmentByResource(-1, -1);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Appointment should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetAppointmentResource
        /// </summary>
        [TestMethod]
        public void GetAppointmentResource_Success()
        {
            // Act
            var result = controller.GetAppointmentResource(appointmentId).Result;
            var response = result as Response<AppointmentResourceViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one appointment resource must exists.");
        }

        /// <summary>
        /// Failure test case for GetAppointmentResource
        /// </summary>
        [TestMethod]
        public void GetAppointmentResource_Failed()
        {
            // Act
            var result = controller.GetAppointmentResource(-1).Result;
            var response = result as Response<AppointmentResourceViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Appointment resource should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetAppointmentResourceByContact
        /// </summary>
        [TestMethod]
        public void GetAppointmentResourceByContact_Success()
        {
            // Act
            var result = controller.GetAppointmentResourceByContact(contactID);
            var response = result as Response<AppointmentResourceViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one appointment resource must exists.");
        }

        /// <summary>
        /// Failure test case for GetAppointmentResourceByContact
        /// </summary>
        [TestMethod]
        public void GetAppointmentResourceByContact_Failed()
        {
            // Act
            var result = controller.GetAppointmentResourceByContact(-1);
            var response = result as Response<AppointmentResourceViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Appointment resource should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetAppointmentLength
        /// </summary>
        [TestMethod]
        public void GetAppointmentLength_Success()
        {
            // Act
            var result = controller.GetAppointmentLength(appointmentTypeID);
            var response = result as Response<AppointmentLengthViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one appointment resource must exists.");
        }

        /// <summary>
        /// Failure test case for GetAppointmentLength
        /// </summary>
        [TestMethod]
        public void GetAppointmentLength_Failed()
        {
            // Act
            var result = controller.GetAppointmentLength(-1);
            var response = result as Response<AppointmentLengthViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Appointment resource should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetAppointmentType
        /// </summary>
        [TestMethod]
        public void GetAppointmentType_Success()
        {
            // Act
            var result = controller.GetAppointmentType(programId);
            var response = result as Response<AppointmentTypeViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one appointment resource must exists.");
        }

        /// <summary>
        /// Failure test case for GetAppointmentType
        /// </summary>
        [TestMethod]
        public void GetAppointmentType_Failed()
        {
            // Act
            var result = controller.GetAppointmentType(-1);
            var response = result as Response<AppointmentTypeViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Appointment resource should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for AddAppointment
        /// </summary>
        [TestMethod]
        public void AddAppointment_Success()
        {
            // Arrange
            var appointment = new AppointmentViewModel
            {
                ContactID = 1,
                ProgramID = 153,
                ServicesID = 172,
                ServiceStatusID = 1,
                AppointmentTypeID = 1,
                AppointmentDate = DateTime.UtcNow,
                AppointmentStartTime = 800,
                AppointmentLength = 30,
                NonMHMRAppointment = "nonMHMR appointment"
            };

            // Act
            var result = controller.AddAppointment(appointment);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Appointment could not be created.");
        }

        /// <summary>
        /// Failure test case for AddAppointment
        /// </summary>
        [TestMethod]
        public void AddAppointment_Failed()
        {
            // Arrange
            var appointment = new AppointmentViewModel
            {
                ContactID = -1,
                ProgramID = 153,
                ServicesID = 172,
                ServiceStatusID = 1,
                AppointmentTypeID = 1,
                AppointmentDate = DateTime.UtcNow,
                AppointmentStartTime = 800,
                AppointmentLength = 30,
                NonMHMRAppointment = "nonMHMR appointment"
            };

            // Act
            var result = controller.AddAppointment(appointment);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add appointment expected to be failed.");
        }

        /// <summary>
        /// Success test case for AddAppointmentContact
        /// </summary>
        [TestMethod]
        public void AddAppointmentContact_Success()
        {
            // Arrange
            var appointmentContact = new AppointmentContactViewModel
            {
                AppointmentID = 1,
                ContactID = 1
            };

            // Act
            var result = controller.AddAppointmentContact(appointmentContact);
            var response = result as Response<AppointmentContactViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Appointment contact could not be created.");
        }

        /// <summary>
        /// Failure test case for AddAppointmentContact
        /// </summary>
        [TestMethod]
        public void AddAppointmentContact_Failed()
        {
            // Arrange
            var appointmentContact = new AppointmentContactViewModel
            {
                AppointmentID = -1,
                ContactID = -1
            };

            // Act
            var result = controller.AddAppointmentContact(appointmentContact);
            var response = result as Response<AppointmentContactViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add appointment contact expected to be failed.");
        }

        /// <summary>
        /// Success test case for AddAppointmentResource
        /// </summary>
        [TestMethod]
        public void AddAppointmentResource_Success()
        {
            // Arrange
            var appointmentResources = new AppointmentResourceViewModel()
            {
                AppointmentID = 1,
                ResourceTypeID = 2,
                ResourceID = 5
            };

            // Act

            var result = controller.AddAppointmentResource(appointmentResources);
            var response = result as Response<AppointmentResourceViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Appointment resource could not be created.");
        }

        /// <summary>
        /// Failure test case for AddAppointmentResource
        /// </summary>
        [TestMethod]
        public void AddAppointmentResource_Failed()
        {
            // Arrange
            var appointmentResources = new AppointmentResourceViewModel()
            {
                AppointmentID = -1,
                ResourceTypeID = 2,
                ResourceID = 5
            };

            // Act
            var result = controller.AddAppointmentResource(appointmentResources);
            var response = result as Response<AppointmentResourceViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add appointment resource expected to be failed.");
        }

        /// <summary>
        /// Success test case for UpdateAppointment
        /// </summary>
        [TestMethod]
        public void UpdateAppointment_Success()
        {
            // Arrange
            var appointment = new AppointmentViewModel
            {
                AppointmentID = 1,
                ContactID = 1,
                ProgramID = 153,
                ServicesID = 172,
                ServiceStatusID = 1,
                AppointmentTypeID = 1,
                AppointmentDate = DateTime.UtcNow,
                AppointmentStartTime = 800,
                AppointmentLength = 30,
                NonMHMRAppointment = "nonMHMR appointment"
            };

            // Act
            var result = controller.UpdateAppointment(appointment);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Appointment could not be updated.");
        }

        /// <summary>
        /// Failure test case for UpdateAppointment
        /// </summary>
        [TestMethod]
        public void UpdateAppointment_Failed()
        {
            // Arrange
            var appointment = new AppointmentViewModel
            {
                AppointmentID = -1,
                ContactID = -1,
                ProgramID = 153,
                ServicesID = 172,
                ServiceStatusID = 1,
                AppointmentTypeID = 1,
                AppointmentDate = DateTime.UtcNow,
                AppointmentStartTime = 800,
                AppointmentLength = 30,
                NonMHMRAppointment = "nonMHMR appointment"
            };

            // Act
            var result = controller.UpdateAppointment(appointment);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Update appointment expected to be failed.");
        }

        /// <summary>
        /// Success test case for UpdateAppointmentContact
        /// </summary>
        [TestMethod]
        public void UpdateAppointmentContact_Success()
        {
            // Arrange
            var appointmentContact = new AppointmentContactViewModel
            {
                AppointmentContactID = 1,
                AppointmentID = 1,
                ContactID = 1
            };

            // Act
            var result = controller.UpdateAppointmentContact(appointmentContact);
            var response = result as Response<AppointmentContactViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Appointment contact could not be updated.");
        }

        /// <summary>
        /// Failure test case for UpdateAppointmentContact
        /// </summary>
        [TestMethod]
        public void UpdateAppointmentContact_Failed()
        {
            // Arrange
            var appointmentContact = new AppointmentContactViewModel
            {
                AppointmentContactID = -1,
                AppointmentID = -1,
                ContactID = 1
            };

            // Act
            var result = controller.UpdateAppointmentContact(appointmentContact);
            var response = result as Response<AppointmentContactViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Update appointment contact expected to be failed.");
        }

        /// <summary>
        /// Success test case for UpdateAppointmentResource
        /// </summary>
        [TestMethod]
        public void UpdateAppointmentResource_Success()
        {
            // Arrange
            var appointmentResources = new AppointmentResourceViewModel()
            {
                AppointmentResourceID = 1,
                AppointmentID = 1,
                ResourceTypeID = 2,
                ResourceID = 5
            };

            // Act
            var result = controller.UpdateAppointmentResource(appointmentResources);
            var response = result as Response<AppointmentResourceViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Appointment resource could not be updated.");
        }

        /// <summary>
        /// Failure test case for UpdateAppointmentResource
        /// </summary>
        [TestMethod]
        public void UpdateAppointmentResource_Failed()
        {
            // Arrange
            var appointmentResources = new AppointmentResourceViewModel()
            {
                AppointmentResourceID = -1,
                AppointmentID = -1,
                ResourceTypeID = 2,
                ResourceID = 5
            };

            // Act
            var result = controller.UpdateAppointmentResource(appointmentResources);
            var response = result as Response<AppointmentResourceViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Update appointment resource expected to be failed.");
        }



        /// <summary>
        /// Success test case for CancelAppointment
        /// </summary>
        [TestMethod]
        public void CancelAppointment_Success()
        {
            // Arrange
            var appointment = new AppointmentViewModel
            {
                AppointmentID = 1,
                ContactID = 1,
                ProgramID = 1,
                AppointmentTypeID = 1,
                AppointmentDate = DateTime.UtcNow,
                AppointmentStartTime = 800,
                AppointmentLength = 30,
                IsCancelled = true
            };

            // Act
            var result = controller.UpdateAppointment(appointment);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Appointment could not be cancelled.");
        }

        /// <summary>
        /// Failure test case for CancelAppointment
        /// </summary>
        [TestMethod]
        public void CancelAppointment_Failed()
        {
            // Arrange
            var appointment = new AppointmentViewModel
            {
                AppointmentID = -1,
                ContactID = -1,
                ProgramID = -1,
                AppointmentTypeID = 1,
                AppointmentDate = DateTime.UtcNow,
                AppointmentStartTime = 800,
                AppointmentLength = 30,
                IsCancelled = true
            };

            // Act
            var result = controller.UpdateAppointment(appointment);
            var response = result as Response<AppointmentViewModel>;

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Cancel appointment expected to be failed.");
        }
    }
}