using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers.Scheduling.Appointment
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class AppointmentLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

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
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Success test case for GetAppointments
        /// </summary>
        [TestMethod]
        public void GetAppointments_Success()
        {
            // Arrange
            var url = baseRoute + "getAppointments";

            var param = new NameValueCollection();
            param.Add("contactId", contactID.ToString());

            // Act
            var response = communicationManager.Get<Response<AppointmentModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointments";

            var param = new NameValueCollection();
            param.Add("contactId", "-1");

            // Act
            var response = communicationManager.Get<Response<AppointmentModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointment";

            var param = new NameValueCollection();
            param.Add("appointmentId", appointmentId.ToString());

            // Act
            var response = communicationManager.Get<Response<AppointmentModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointment";

            var param = new NameValueCollection();
            param.Add("appointmentId", "-1");

            // Act
            var response = communicationManager.Get<Response<AppointmentModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentByResource";

            var param = new NameValueCollection();
            param.Add("resourceID", resourceID.ToString());
            param.Add("resourceTypeID", resourceTypeID.ToString());

            // Act
            var response = communicationManager.Get<Response<AppointmentModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentByResource";

            var param = new NameValueCollection();
            param.Add("resourceID", "-1");
            param.Add("resourceTypeID", "-1");

            // Act
            var response = communicationManager.Get<Response<AppointmentModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentResource";

            var param = new NameValueCollection();
            param.Add("appointmentId", appointmentId.ToString());

            // Act
            var response = communicationManager.Get<Response<AppointmentResourceModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentResource";

            var param = new NameValueCollection();
            param.Add("appointmentId", "-1");

            // Act
            var response = communicationManager.Get<Response<AppointmentResourceModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentResourceByContact";

            var param = new NameValueCollection();
            param.Add("contactId", contactID.ToString());

            // Act
            var response = communicationManager.Get<Response<AppointmentResourceModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentResourceByContact";

            var param = new NameValueCollection();
            param.Add("contactId", "-1");

            // Act
            var response = communicationManager.Get<Response<AppointmentResourceModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentLength";

            var param = new NameValueCollection();
            param.Add("appointmentTypeID", appointmentTypeID.ToString());

            // Act
            var response = communicationManager.Get<Response<AppointmentLengthModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentLength";

            var param = new NameValueCollection();
            param.Add("appointmentTypeID", "-1");

            // Act
            var response = communicationManager.Get<Response<AppointmentLengthModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentType";

            var param = new NameValueCollection();
            param.Add("programId", programId.ToString());

            // Act
            var response = communicationManager.Get<Response<AppointmentTypeModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getAppointmentType";

            var param = new NameValueCollection();
            param.Add("programId", "-1");

            // Act
            var response = communicationManager.Get<Response<AppointmentTypeModel>>(param, url);

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
            var url = baseRoute + "addAppointment";

            var appointment = new AppointmentModel
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
            var response = communicationManager.Post<AppointmentModel, Response<AppointmentModel>>(appointment, url);

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
            var url = baseRoute + "addAppointment";

            var appointment = new AppointmentModel
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
            var response = communicationManager.Post<AppointmentModel, Response<AppointmentModel>>(appointment, url);

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
            var url = baseRoute + "addAppointmentContact";

            var appointmentContact = new AppointmentContactModel
            {
                AppointmentID = 1,
                ContactID = 1
            };

            // Act
            var response = communicationManager.Post<AppointmentContactModel, Response<AppointmentContactModel>>(appointmentContact, url);

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
            var url = baseRoute + "addAppointmentContact";

            var appointmentContact = new AppointmentContactModel
            {
                AppointmentID = 1,
                ContactID = 1
            };

            // Act
            var response = communicationManager.Post<AppointmentContactModel, Response<AppointmentContactModel>>(appointmentContact, url);

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
            var url = baseRoute + "addAppointmentResource";

            var appointmentResources = new List<AppointmentResourceModel>() {
                new AppointmentResourceModel
                {
                    AppointmentID = 1,
                    ResourceTypeID = 2,
                    ResourceID = 5
                },
                new AppointmentResourceModel
                {
                    AppointmentID = 1,
                    ResourceTypeID = 2,
                    ResourceID = 1
                }
            };

            // Act
            var response = communicationManager.Post<List<AppointmentResourceModel>, Response<AppointmentResourceModel>>(appointmentResources, url);

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
            var url = baseRoute + "addAppointmentResource";

            var appointmentResources = new List<AppointmentResourceModel>() {
                new AppointmentResourceModel
                {
                    AppointmentID = 1,
                    ResourceTypeID = 2,
                    ResourceID = 5
                },
                new AppointmentResourceModel
                {
                    AppointmentID = 1,
                    ResourceTypeID = 2,
                    ResourceID = 1
                }
            };

            // Act
            var response = communicationManager.Post<List<AppointmentResourceModel>, Response<AppointmentResourceModel>>(appointmentResources, url);

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
            var url = baseRoute + "updateAppointment";

            var appointment = new AppointmentModel
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
            var response = communicationManager.Post<AppointmentModel, Response<AppointmentModel>>(appointment, url);

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
            var url = baseRoute + "updateAppointment";

            var appointment = new AppointmentModel
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
            var response = communicationManager.Post<AppointmentModel, Response<AppointmentModel>>(appointment, url);

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
            var url = baseRoute + "updateAppointmentContact";

            var appointmentContact = new AppointmentContactModel
            {
                AppointmentContactID = 1,
                AppointmentID = 1,
                ContactID = 1
            };

            // Act
            var response = communicationManager.Post<AppointmentContactModel, Response<AppointmentContactModel>>(appointmentContact, url);

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
            var url = baseRoute + "updateAppointmentContact";

            var appointmentContact = new AppointmentContactModel
            {
                AppointmentContactID = -1,
                AppointmentID = -1,
                ContactID = 1
            };

            // Act
            var response = communicationManager.Post<AppointmentContactModel, Response<AppointmentContactModel>>(appointmentContact, url);

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
            var url = baseRoute + "updateAppointmentResource";

            var appointmentResources = new List<AppointmentResourceModel>() {
                new AppointmentResourceModel
                {
                    AppointmentResourceID=1,
                    AppointmentID = 1,
                    ResourceTypeID = 2,
                    ResourceID = 5
                },
                new AppointmentResourceModel
                {
                    AppointmentResourceID=2,
                    AppointmentID = 1,
                    ResourceTypeID = 2,
                    ResourceID = 1
                }
            };

            // Act
            var response = communicationManager.Post<List<AppointmentResourceModel>, Response<AppointmentResourceModel>>(appointmentResources, url);

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
            var url = baseRoute + "updateAppointmentResource";

            var appointmentResources = new List<AppointmentResourceModel>() {
                new AppointmentResourceModel
                {
                    AppointmentResourceID=-1,
                    AppointmentID = -1,
                    ResourceTypeID = 2,
                    ResourceID = 5
                },
                new AppointmentResourceModel
                {
                    AppointmentResourceID=-1,
                    AppointmentID = -1,
                    ResourceTypeID = 2,
                    ResourceID = 1
                }
            };

            // Act
            var response = communicationManager.Post<List<AppointmentResourceModel>, Response<AppointmentResourceModel>>(appointmentResources, url);

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
            var url = baseRoute + "updateAppointment";

            var appointment = new AppointmentModel
            {
                AppointmentID = 1,
                ContactID = 1,
                ProgramID = 1,
                AppointmentTypeID = 1,
                AppointmentDate = DateTime.UtcNow,
                AppointmentStartTime = 800,
                AppointmentLength = 30,
                IsCancelled=true
            };

            // Act
            var response = communicationManager.Post<AppointmentModel, Response<AppointmentModel>>(appointment, url);

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
            var url = baseRoute + "updateAppointment";

            var appointment = new AppointmentModel
            {
                AppointmentID = -1,
                ContactID = -1,
                ProgramID = -1,
                AppointmentTypeID = -1,
                AppointmentDate = DateTime.UtcNow,
                AppointmentStartTime = 800,
                AppointmentLength = 30,
                IsCancelled=true
            };

            // Act
            var response = communicationManager.Post<AppointmentModel, Response<AppointmentModel>>(appointment, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Cancel appointment expected to be failed.");
        }
    }
}