
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.RuleEngine.BusinessAdmin.ClientMerge
{
    public interface IClientMergeRuleEngine
    {

        /// <summary>
        /// Gets the client merge count.
        /// </summary>
        /// <returns></returns>
        Response<ClientMergeCountModel> GetClientMergeCounts();

        /// <summary>
        /// Gets the potential matches.
        /// </summary>
        /// <returns></returns>
        Response<PotentialMatchesModel> GetPotentialMatches();

        /// <summary>
        /// Merge records.
        /// </summary>
        /// <returns></returns>
        Response<ClientMergeModel> MergeRecords(ClientMergeModel clientMerge);

        /// <summary>
        /// Gets the merged records.
        /// </summary>
        /// <returns></returns>
        Response<MergeContactModel> GetMergedRecords();

        /// <summary>
        /// Unmerged contact
        /// </summary>
        /// <param name="mergedContactsMappingID"></param>
        /// <returns></returns>
        Response<MergeContactModel> UnMergeRecords(long mergedContactsMappingID);


        /// <summary>
        /// Gets refresh potential matches.
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
