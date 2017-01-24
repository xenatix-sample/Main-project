using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Axis.DataProvider.Account;
using Axis.Model.Account;
using Axis.DataEngine.Helpers.Results;

namespace Axis.DataEngine.Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContactDemographicsController : ApiController
    {
        readonly IAccountDataProvider _accountDataProvider;
        
        public ContactDemographicsController(IAccountDataProvider accountDataProvider)
        {
            _accountDataProvider = accountDataProvider;
        }

        //[HttpGet]
        //public IHttpActionResult GetContactDemographics([FromUri]UserProfileModel userProfile)
        //{
        //    return new HttpResult<UserProfileModel>(_accountDataProvider.GetUserProfile(userProfile.UserName), Request);
        //}

    }
}
