using System;
using Axis.Model.Clinical;
using Axis.Model.Common;

namespace Axis.RuleEngine.Clinical.ChiefComplaint
{
    public interface IChiefComplaintRuleEngine
    {
        Response<ChiefComplaintModel> GetChiefComplaint(long contactId);
        Response<ChiefComplaintModel> GetChiefComplaints(long contactId);
        Response<ChiefComplaintModel> AddChiefComplaint(ChiefComplaintModel chiefComplaint);
        Response<ChiefComplaintModel> UpdateChiefComplaint(ChiefComplaintModel chiefComplaint);
        Response<ChiefComplaintModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn);
    }
}
