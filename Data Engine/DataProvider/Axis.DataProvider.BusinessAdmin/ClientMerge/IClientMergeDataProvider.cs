using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.DataProvider.BusinessAdmin.ClientMerge
{
    public interface IClientMergeDataProvider
    {
        Response<ClientMergeCountModel> GetClientMergeCounts();
        Response<ClientMergeModel> MergeRecords(ClientMergeModel userDetail);
        Response<PotentialMatchesModel> GetPotentialMatches();
        Response<MergeContactModel> GetMergedRecords();
        Response<MergeContactModel> UnMergeRecords(long mergedContactsMappingID);
        Response<PotentialMatchesModel> GetRefreshPotentialMatches();
        Response<PotentialMergeContactsLastRunModel> GetPotentialMergeContactsLastRun();
    }
}
