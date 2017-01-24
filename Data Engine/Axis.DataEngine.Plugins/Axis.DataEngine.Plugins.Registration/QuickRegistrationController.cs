using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{

    public class QuickRegistrationController : BaseApiController
    {
        readonly IQuickRegistrationDataProvider _quickRegistrationDataProvider;
        public QuickRegistrationController(IQuickRegistrationDataProvider quickRegistrationDataProvider)
        {
            _quickRegistrationDataProvider = quickRegistrationDataProvider;
        }

        public IHttpActionResult GetQuickRegistration()
        {
            return new HttpResult<Response<QuickRegistrationModel>>(_quickRegistrationDataProvider.GetQuickRegistration(), Request);
        }


        public IHttpActionResult Add(QuickRegistrationModel contact)
        {
            return new HttpResult<Response<QuickRegistrationModel>>(_quickRegistrationDataProvider.Add(contact), Request);
        }


        public IHttpActionResult GetMrnMpi()
        {
            return new HttpResult<Response<QuickRegistrationModel>>(_quickRegistrationDataProvider.GetMrnMpi(), Request);
        }

    }
}
