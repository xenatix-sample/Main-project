using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.RuleEngine.Registration;
using Moq;
using Axis.Model.Registration;
using Axis.Model.Address;
using Axis.Model.Common;
using System.Linq;
using Axis.RuleEngine.Plugins.Registration;
using Axis.RuleEngine.Service.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Helpers;
using Axis.Service;
using System.Collections.Specialized;
using Axis.RuleEngine.RecordedServices.VoidService;
using Axis.Model.RecordedServices;
namespace Axis.RuleEngine.Tests.Controllers
{
    [TestClass]
    public class VoidServiceTest
    {
        /// <summary>
        /// Represent VoidService Controller Test Class
        /// </summary>
        [TestClass]
        public class VoidServiceControllerTest
        {
            /// <summary>
            /// The VoidService data provider
            /// </summary>
            private IVoidServiceRuleEngine voidServiceRuleEngine;



            /// <summary>
            /// Initializes this instance.
            /// </summary>
            [TestInitialize]
            public void Initialize()
            {
            }

            /// <summary>
            /// Adds the void service for success.
            /// </summary>
            [TestMethod]
            public void VoidRecordedService_Success()
            {
                Mock<IVoidServiceRuleEngine> mock = new Mock<IVoidServiceRuleEngine>();
                voidServiceRuleEngine = mock.Object;
                VoidServiceController voidServiceController = new VoidServiceController(voidServiceRuleEngine);
                var voidServiceModel = new VoidServiceModel
                {
                    ServiceRecordingID = 1,
                    ServiceRecordingVoidReasonID = 1,
                    IncorrectOrganization = true,
                    IncorrectServiceType = true,
                    IncorrectServiceItem = true,
                    IncorrectServiceStatus = true,
                    IncorrectSupervisor = true,
                    IncorrectAdditionalUser = true,
                    IncorrectAttendanceStatus = true,
                    IncorrectStartDate = true,
                    IncorrectStartTime = true,
                    IncorrectEndDate = true,
                    IncorrectEndTime = true,
                    IncorrectDeliveryMethod = true,
                    IncorrectServiceLocation = true,
                    IncorrectRecipientCode = true,
                    IncorrectTrackingField = true,
                    Comments = "",
                    ModifiedOn = DateTime.Now
                };
                //Act
                var saveResult = voidServiceController.VoidRecordedService(voidServiceModel);
                var response = saveResult as HttpResult<Response<VoidServiceModel>>;
                var count = response.Value.DataItems.Count();

                //Assert
                Assert.IsNotNull(voidServiceModel);
                Assert.IsTrue(count > 0);
            }

            /// <summary>
            /// Adds the Contact demographic for failed.
            /// </summary>
            [TestMethod]
            public void AddContactDemographic_Failed()
            {
                Mock<IVoidServiceRuleEngine> mock = new Mock<IVoidServiceRuleEngine>();
                voidServiceRuleEngine = mock.Object;
                VoidServiceController voidServiceController = new VoidServiceController(voidServiceRuleEngine);
                var voidServiceModel = new VoidServiceModel
                {
                    ServiceRecordingID = -1,
                    ServiceRecordingVoidReasonID = 1,
                    IncorrectOrganization = true,
                    IncorrectServiceType = true,
                    IncorrectServiceItem = true,
                    IncorrectServiceStatus = true,
                    IncorrectSupervisor = true,
                    IncorrectAdditionalUser = true,
                    IncorrectAttendanceStatus = true,
                    IncorrectStartDate = true,
                    IncorrectStartTime = true,
                    IncorrectEndDate = true,
                    IncorrectEndTime = true,
                    IncorrectDeliveryMethod = true,
                    IncorrectServiceLocation = true,
                    IncorrectRecipientCode = true,
                    IncorrectTrackingField = true,
                    Comments = "",
                    ModifiedOn = DateTime.Now
                };
                //Act
                var saveResult = voidServiceController.VoidRecordedService(voidServiceModel);
                var response = saveResult as HttpResult<Response<VoidServiceModel>>;
                var count = response.Value.DataItems.Count();

                //Assert
                Assert.IsTrue(count == 0);
            }
        }
    }
}
