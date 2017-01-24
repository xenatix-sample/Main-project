using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Configuration;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.BusinessAdmin.ClientMerge
{
    public class ClientMergeService : IClientMergeService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "ClientMerge/";

        #endregion

        #region Constructors

        public ClientMergeService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods
        public Response<ClientMergeCountModel> GetClientMergeCounts()
        {
            string apiUrl = BaseRoute + "GetClientMergeCounts";
            return _communicationManager.Get<Response<ClientMergeCountModel>>(apiUrl);
        }

        /// <summary>
        /// Gets the potential matches.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMatchesModel> GetPotentialMatches()
        {
            string apiUrl = BaseRoute + "GetPotentialMatches";
            return _communicationManager.Get<Response<PotentialMatchesModel>>(apiUrl);
        }

        public Response<ClientMergeModel> MergeRecords(ClientMergeModel clientMerge)
        {
            string apiUrl = BaseRoute + "MergeRecords";
            return _communicationManager.Post<ClientMergeModel, Response<ClientMergeModel>>(clientMerge, apiUrl);
        }

        public Response<MergeContactModel> GetMergedRecords()
        {
            string apiUrl = BaseRoute + "GetMergedRecords";
            return _communicationManager.Get<Response<MergeContactModel>>(apiUrl);
        }

        public Response<MergeContactModel> UnMergeRecords(long mergedContactsMappingID)
        {
            string apiUrl = BaseRoute + "UnMergeRecords";
            var param = new NameValueCollection();
            param.Add("mergedContactsMappingID", mergedContactsMappingID.ToString());
            return _communicationManager.Post<Response<MergeContactModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets refreshed potential matches.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMatchesModel> GetRefreshPotentialMatches()
        {
            string apiUrl = BaseRoute + "GetRefreshPotentialMatches";
            return _communicationManager.Get<Response<PotentialMatchesModel>>(apiUrl);
        }

        /// <summary>
        /// Gets potential matches last run.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMergeContactsLastRunModel> GetPotentialMergeContactsLastRun()
        {
            string apiUrl = BaseRoute + "GetPotentialMergeContactsLastRun";
            return _communicationManager.Get<Response<PotentialMergeContactsLastRunModel>>(apiUrl);
        }
        #endregion
    }
}
