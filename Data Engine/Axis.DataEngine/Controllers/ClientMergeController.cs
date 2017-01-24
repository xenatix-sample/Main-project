using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.ClientMerge;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class ClientMergeController : BaseApiController
    {
        #region Class Variables

        readonly IClientMergeDataProvider _clientMergeDataProvider = null;

        #endregion

        #region Constructors

        public ClientMergeController(IClientMergeDataProvider clientMergeDataProvider)
        {
            _clientMergeDataProvider = clientMergeDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetClientMergeCounts()
        {
            var ClientMergeCountResponse = _clientMergeDataProvider.GetClientMergeCounts();
            return new HttpResult<Response<ClientMergeCountModel>>(ClientMergeCountResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetPotentialMatches()
        {
            var potentialMatchesResponse = _clientMergeDataProvider.GetPotentialMatches();
            return new HttpResult<Response<PotentialMatchesModel>>(potentialMatchesResponse, Request);
        }
        
        [HttpPost]
        public IHttpActionResult MergeRecords(ClientMergeModel clientMerge)
        {
            var clientMergeResponse = _clientMergeDataProvider.MergeRecords(clientMerge);
            return new HttpResult<Response<ClientMergeModel>>(clientMergeResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetMergedRecords()
        {
            var mergedRecords = _clientMergeDataProvider.GetMergedRecords();
            return new HttpResult<Response<MergeContactModel>>(mergedRecords, Request);
        }
        [HttpPost]
        public IHttpActionResult UnMergeRecords(long mergedContactsMappingID)
        {
            var clientUnMergeResponse = _clientMergeDataProvider.UnMergeRecords(mergedContactsMappingID);
            return new HttpResult<Response<MergeContactModel>>(clientUnMergeResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetRefreshPotentialMatches()
        {
            var potentialMatchesResponse = _clientMergeDataProvider.GetRefreshPotentialMatches();
            return new HttpResult<Response<PotentialMatchesModel>>(potentialMatchesResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetPotentialMergeContactsLastRun()
        {
            var potentialMatchesLastRunResponse = _clientMergeDataProvider.GetPotentialMergeContactsLastRun();
            return new HttpResult<Response<PotentialMergeContactsLastRunModel>>(potentialMatchesLastRunResponse, Request);
        }
        #endregion
    }
}