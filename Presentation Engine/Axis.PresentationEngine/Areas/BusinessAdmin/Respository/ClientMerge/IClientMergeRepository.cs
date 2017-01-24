
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ClientMerge
{
    public interface IClientMergeRepository
    {
        /// <summary>
        /// Get total merge contact
        /// </summary>
        /// <returns></returns>
        Response<ClientMergeCountModel> GetClientMergeCounts();
        /// <summary>
        /// Gets the potential matches.
        /// </summary>
        /// <returns></returns>
        Response<PotentialMatchesModel> GetPotentialMatches();

        /// <summary>
        /// Merge two contact
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        Response<ClientMergeViewModel> MergeRecords(ClientMergeViewModel userDetail);
        /// <summary>
        /// Gets the merged records.
        /// </summary>
        /// <returns></returns>
        Response<MergeContactModel> GetMergedRecords();

        /// <summary>
        /// UnMerge records
        /// </summary>
        /// <param name="mergedContactsMappingID"></param>
        /// <returns></returns>
        Response<MergeContactModel> UnMergeRecords(long mergedContactsMappingID);

        /// <summary>
        /// Get refresh potential matches
        /// </summary>
        /// <returns></returns>
        Response<PotentialMatchesModel> GetRefreshPotentialMatches();

        /// <summary>
        /// Gets potential matches last run.
        /// </summary>
        /// <returns></returns>
        Response<PotentialMergeContactsLastRunModel> GetPotentialMergeContactsLastRun();
    }
}
