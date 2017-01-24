
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.Service.BusinessAdmin.ClientMerge
{
    public interface IClientMergeService
    {
        Response<ClientMergeCountModel> GetClientMergeCounts();
        Response<PotentialMatchesModel> GetPotentialMatches();
        Response<ClientMergeModel> MergeRecords(ClientMergeModel userDetail);
        Response<MergeContactModel> GetMergedRecords();
        Response<MergeContactModel> UnMergeRecords(long mergedContactsMappingID);
        Response<PotentialMatchesModel> GetRefreshPotentialMatches();
        Response<PotentialMergeContactsLastRunModel> GetPotentialMergeContactsLastRun();
    }
}
