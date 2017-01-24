
using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.ClientMerge;

namespace Axis.RuleEngine.BusinessAdmin.ClientMerge
{
    public class ClientMergeRuleEngine : IClientMergeRuleEngine
    {
        #region Class Variables

        private readonly IClientMergeService _clientMergeService;

        #endregion

        #region Constructors

        public ClientMergeRuleEngine(IClientMergeService clientMergeService)
        {
            _clientMergeService = clientMergeService;
        }

        #endregion

        public Response<ClientMergeCountModel> GetClientMergeCounts()
        {
            return _clientMergeService.GetClientMergeCounts();
        }

        /// <summary>
        /// Gets the potential matches.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMatchesModel> GetPotentialMatches()
        {
            return _clientMergeService.GetPotentialMatches();
        }

        public Response<ClientMergeModel> MergeRecords(ClientMergeModel clientMerge)
        {
            return _clientMergeService.MergeRecords(clientMerge);
        }

        /// <summary>
        /// Gets the merged records.
        /// </summary>
        /// <returns></returns>
        public Response<MergeContactModel> GetMergedRecords()
        {
            return _clientMergeService.GetMergedRecords();
        }

        /// <summary>
        /// UnMerged Contact 
        /// </summary>
        /// <param name="mergedContactsMappingID"></param>
        /// <returns></returns>
        public Response<MergeContactModel> UnMergeRecords(long mergedContactsMappingID) {
            return _clientMergeService.UnMergeRecords(mergedContactsMappingID);
        }

        /// <summary>
        /// Gets refresh potential matches.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMatchesModel> GetRefreshPotentialMatches()
        {
            return _clientMergeService.GetRefreshPotentialMatches();
        }

        /// <summary>
        /// Gets potential matches last run.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMergeContactsLastRunModel> GetPotentialMergeContactsLastRun()
        {
            return _clientMergeService.GetPotentialMergeContactsLastRun();
        }
    }
}
