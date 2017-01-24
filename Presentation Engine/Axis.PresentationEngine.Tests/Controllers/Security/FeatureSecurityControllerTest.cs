using Axis.PresentationEngine.Areas.Security.Controllers;
using Axis.PresentationEngine.Areas.Security.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers
{
    [TestClass]
    public class FeatureSecurityControllerTest
    {
        [TestClass]
        public class SecurityControllerTest
        {
            private RoleSecurityController controller = null;

            [TestInitialize]
            public void Initialize()
            {
                controller = new RoleSecurityController(new SecurityRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
            }

            [TestMethod]
            public void GetUserRoleSecurity_Success()
            {
                // Act
                var result = controller.GetUserRoleSecurity();

                Assert.IsInstanceOfType(result, typeof(ViewResult));

                // Assert
                Assert.IsNotNull(result, "Atleast one user role security must exist");
            }
        }
    }
}