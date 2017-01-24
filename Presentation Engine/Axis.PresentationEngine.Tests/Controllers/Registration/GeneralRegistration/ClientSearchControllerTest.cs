using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.ApiControllers;
using MvcControllers = Axis.Plugins.Registration.Controllers;
using Axis.Plugins.Registration.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers
{
    [TestClass]
    public class ClientSearchControllerTest
    {
        #region Class Variables

        private string token = "ihI1VMt/a+0ObMgen2QT7EsH4KoSvdKdXQsdYKfz84GaKSS9fgNaPnp90ViKdAwKJVvKHjSoNarCegJIMibiO00MKApNJFEuxu9Xx2vlv37L7P7j41zJ4auVLgOiqmIEyiaphr3jGWqfG1I65wmTMqqCGm/ZxIUS86qahnYGL2x5C7FvqC31Kh3S1917pjdE";
        private string searchText = "m";
        private string contactType = "1";

        #endregion

        #region ActionResults

        [TestMethod]
        public void Index_Success()
        {
            var controller = new MvcControllers.ClientSearchController();
            ActionResult result = controller.Index();

            Assert.IsNotNull(result);
        }
        #endregion


        [TestMethod]
        public void GetClientSummary_Success()
        {
            var controller = new ClientSearchController(new ClientSearchRepository(token));

            var modelResponse = controller.GetClientSummary(searchText, contactType);

            Assert.IsTrue(modelResponse.DataItems != null, "Data items can't be null");
            Assert.IsTrue(modelResponse.DataItems.Count > 0, "Record not found");
        }
    }
}
