using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ClientMerge;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class ClientMergeController : BaseApiController
    {
        #region Class Variables

        private readonly IClientMergeRepository _clientMergeRepository;

        #endregion

        #region Constructors
        public ClientMergeController(IClientMergeRepository clientMergeRepository)
        {
            _clientMergeRepository = clientMergeRepository;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public Response<ClientMergeCountModel> GetClientMergeCounts()
        {
            return _clientMergeRepository.GetClientMergeCounts();
        }

        /// <summary>
        /// Gets the potential matches.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<PotentialMatchesModel> GetPotentialMatches()
        {
            return _clientMergeRepository.GetPotentialMatches();
        }

        [HttpPost]
        public Response<ClientMergeViewModel> MergeRecords(ClientMergeViewModel clientMerge)
        {
            return _clientMergeRepository.MergeRecords(clientMerge);
        }

        /// <summary>
        /// Gets the merged records.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<MergeContactModel> GetMergedRecords()
        {
            return _clientMergeRepository.GetMergedRecords();
        }

        /// <summary>
        /// Un merge contact
        /// </summary>
        /// <param name="mergedContactsMappingID"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<MergeContactModel> UnMergeRecords([FromBody] int mergedContactsMappingID)
        {
            return _clientMergeRepository.UnMergeRecords(mergedContactsMappingID);
        }

        /// <summary>
        /// Get refresh potential matches
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<PotentialMatchesModel> GetRefreshPotentialMatches()
        {
            return _clientMergeRepository.GetRefreshPotentialMatches();
        }

        /// <summary>
        /// Gets potential matches last run.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<PotentialMergeContactsLastRunModel> GetPotentialMergeContactsLastRun()
        {
            return _clientMergeRepository.GetPotentialMergeContactsLastRun();
        }
        #endregion
    }
}