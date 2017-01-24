using System;
using Axis.Model.Clinical;
using Axis.Model.Common;

namespace Axis.Service.Clinical.ChiefComplaint
{
    public interface IChiefComplaintService
    {
        Response<ChiefComplaintModel> GetChiefComplaint(long contactID);
        Response<ChiefComplaintModel> GetChiefComplaints(long contactID);
        Response<ChiefComplaintModel> AddChiefComplaint(ChiefComplaintModel chiefComplaint);
        Response<ChiefComplaintModel> UpdateChiefComplaint(ChiefComplaintModel chiefComplaint);
        Response<ChiefComplaintModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn);
    }
}
