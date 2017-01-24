using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.PresentationEngine.Controllers;

namespace Axis.PresentationEngine.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
