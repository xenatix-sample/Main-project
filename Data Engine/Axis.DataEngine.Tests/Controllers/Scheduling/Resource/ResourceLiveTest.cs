using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers.Scheduling.Resource
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class ResourceLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "resource/";

        /// <summary>
        /// The facility identifier
        /// </summary>
        private long facilityId = 1;

        /// <summary>
        /// The appointment type identifier
        /// </summary>
        private int appointmentTypeID = 12;

        /// <summary>
        /// The credential identifier
        /// </summary>
        private long credentialId = 13;

        /// <summary>
        /// The resource type identifier
        /// </summary>
        private short resourceTypeId = 2;

        /// <summary>
        /// The resource identifier
        /// </summary>
        private int resourceId = 5;

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
        /// Success test case for GetRooms
        /// </summary>
        [TestMethod]
        public void GetRooms_Success()
        {
            // Arrange
            var url = baseRoute + "getRooms";

            var param = new NameValueCollection();
            param.Add("facilityId", facilityId.ToString());

            // Act
            var response = communicationManager.Get<Response<RoomModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one room must exists.");
        }

        /// <summary>
        /// Faliure test case for GetRooms
        /// </summary>
        [TestMethod]
        public void GetRooms_Failed()
        {
            // Arrange
            var url = baseRoute + "getRooms";

            var param = new NameValueCollection();
            param.Add("facilityId", "-1");

            // Act
            var response = communicationManager.Get<Response<RoomModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Room should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetCredentialByAppointmentType
        /// </summary>
        [TestMethod]
        public void GetCredentialByAppointmentType_Success()
        {
            // Arrange
            var url = baseRoute + "getCredentialByAppointmentType";

            var param = new NameValueCollection();
            param.Add("appointmentTypeID", appointmentTypeID.ToString());

            // Act
            var response = communicationManager.Get<Response<AppointmentCredentialModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one credential must exists.");
        }

        /// <summary>
        /// Faliure test case for GetCredentialByAppointmentType
        /// </summary>
        [TestMethod]
        public void GetCredentialByAppointmentType_Failed()
        {
            // Arrange
            var url = baseRoute + "getCredentialByAppointmentType";

            var param = new NameValueCollection();
            param.Add("appointmentTypeID", "-1");

            // Act
            var response = communicationManager.Get<Response<AppointmentCredentialModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Credential should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetProviderByCredential
        /// </summary>
        [TestMethod]
        public void GetProviderByCredential_Success()
        {
            // Arrange
            var url = baseRoute + "getProviderByCredential";

            var param = new NameValueCollection();
            param.Add("credentialId", credentialId.ToString());

            // Act
            var response = communicationManager.Get<Response<ProviderModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one provider must exists.");
        }

        /// <summary>
        /// Faliure test case for GetProviderByCredential
        /// </summary>
        [TestMethod]
        public void GetProviderByCredential_Failed()
        {
            // Arrange
            var url = baseRoute + "getProviderByCredential";

            var param = new NameValueCollection();
            param.Add("credentialId", "-1");

            // Act
            var response = communicationManager.Get<Response<ProviderModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Provider should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetResources
        /// </summary>
        [TestMethod]
        public void GetResources_Success()
        {
            // Arrange
            var url = baseRoute + "getResources";

            var param = new NameValueCollection();
            param.Add("resourceTypeId", resourceTypeId.ToString());
            param.Add("facilityId", facilityId.ToString());

            // Act
            var response = communicationManager.Get<Response<ResourceModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one resource must exists.");
        }

        /// <summary>
        /// Faliure test case for GetResources
        /// </summary>
        [TestMethod]
        public void GetResources_Failed()
        {
            // Arrange
            var url = baseRoute + "getResources";

            var param = new NameValueCollection();
            param.Add("resourceTypeId", "-1");
            param.Add("facilityId", "-1");

            // Act
            var response = communicationManager.Get<Response<ResourceModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Resource should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetResourceDetails
        /// </summary>
        [TestMethod]
        public void GetResourceDetails_Success()
        {
            // Arrange
            var url = baseRoute + "getResourceDetails";

            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeId", resourceTypeId.ToString());

            // Act
            var response = communicationManager.Get<Response<ResourceModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one resource must exists.");
        }

        /// <summary>
        /// Faliure test case for GetResourceDetails
        /// </summary>
        [TestMethod]
        public void GetResourceDetails_Failed()
        {
            // Arrange
            var url = baseRoute + "getResourceDetails";

            var param = new NameValueCollection();
            param.Add("resourceId", "-1");
            param.Add("resourceTypeId", "-1");

            // Act
            var response = communicationManager.Get<Response<ResourceModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems[0].ResourceAvailabilities.Count == 0, "Resource should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetResourceAvailability
        /// </summary>
        [TestMethod]
        public void GetResourceAvailability_Success()
        {
            // Arrange
            var url = baseRoute + "getResourceAvailability";

            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeId", resourceTypeId.ToString());

            // Act
            var response = communicationManager.Get<Response<ResourceAvailabilityModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one resource availability must exists.");
        }

        /// <summary>
        /// Faliure test case for GetResourceAvailability
        /// </summary>
        [TestMethod]
        public void GetResourceAvailability_Failed()
        {
            // Arrange
            var url = baseRoute + "getResourceAvailability";

            var param = new NameValueCollection();
            param.Add("resourceId", "-1");
            param.Add("resourceTypeId", "-1");

            // Act
            var response = communicationManager.Get<Response<ResourceAvailabilityModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Resource availability should not exists for this test case.");
        }

        /// <summary>
        /// Success test case for GetResourceOverrides
        /// </summary>
        [TestMethod]
        public void GetResourceOverrides_Success()
        {
            // Arrange
            var url = baseRoute + "getResourceOverrides";

            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeId", resourceTypeId.ToString());

            // Act
            var response = communicationManager.Get<Response<ResourceOverridesModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one resource override must exists.");
        }

        /// <summary>
        /// Faliure test case for GetResourceOverrides
        /// </summary>
        [TestMethod]
        public void GetResourceOverrides_Failed()
        {
            // Arrange
            var url = baseRoute + "getResourceOverrides";

            var param = new NameValueCollection();
            param.Add("resourceId", "-1");
            param.Add("resourceTypeId", "-1");

            // Act
            var response = communicationManager.Get<Response<ResourceOverridesModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Resource override should not exists for this test case.");
        }
    }
}