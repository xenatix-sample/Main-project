using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;
using Axis.Service;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.BusinessAdmin;
using Axis.PresentationEngine.Areas.BusinessAdmin.Translator;
using System.Collections.Specialized;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ClientMerge
{
    public class ClientMergeRepository : IClientMergeRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "clientmerge/";

        #endregion Class Variables

        #region Constructors

        public ClientMergeRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public ClientMergeRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// Get total merge contact
        /// </summary>
        /// <returns></returns>
        public Response<ClientMergeCountModel> GetClientMergeCounts()
        {
            const string apiUrl = BaseRoute + "GetClientMergeCounts";
            return _communicationManager.Get<Response<ClientMergeCountModel>>(apiUrl);
        }

        /// <summary>
        /// Gets the potential matches.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMatchesModel> GetPotentialMatches()
        {
            var apiUrl = BaseRoute + "GetPotentialMatches";
            return _communicationManager.Get<Response<PotentialMatchesModel>>(apiUrl);
        }

        /// <summary>
        /// Merge two contact
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        public Response<ClientMergeViewModel> MergeRecords(ClientMergeViewModel clientMerge)
        {
            const string apiUrl = BaseRoute + "MergeRecords";
            var response = _communicationManager.Post<ClientMergeModel, Response<ClientMergeModel>>(clientMerge.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the merged records.
        /// </summary>
        /// <returns></returns>
        public Response<MergeContactModel> GetMergedRecords()
        {
            var apiUrl = BaseRoute + "GetMergedRecords";
            return _communicationManager.Get<Response<MergeContactModel>>(apiUrl);
        }

        /// <summary>
        /// UnMerge records
        /// </summary>
        /// <param name="mergedContactsMappingID"></param>
        /// <returns></returns>
        public Response<MergeContactModel> UnMergeRecords(long mergedContactsMappingID)
        {
            string apiUrl = BaseRoute + "UnMergeRecords";
            var param = new NameValueCollection();
            param.Add("mergedContactsMappingID", mergedContactsMappingID.ToString());
            var response = _communicationManager.Post<Response<MergeContactModel>>(param, apiUrl);
            return response;
        }


        /// <summary>
        /// Gets refresh potiential matches.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMatchesModel> GetRefreshPotentialMatches()
        {
            var apiUrl = BaseRoute + "GetRefreshPotentialMatches";
            return _communicationManager.Get<Response<PotentialMatchesModel>>(apiUrl);
        }


        /// <summary>
        /// Gets potential matches last run.
        /// </summary>
        /// <returns></returns>
        public Response<PotentialMergeContactsLastRunModel> GetPotentialMergeContactsLastRun()
        {
            var apiUrl = BaseRoute + "GetPotentialMergeContactsLastRun";
            return _communicationManager.Get<Response<PotentialMergeContactsLastRunModel>>(apiUrl);
        }
        #endregion
    }
}