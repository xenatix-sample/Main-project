using System;
using Axis.Model.Common;
using Axis.Model.Clinical;

namespace Axis.DataProvider.Clinical.ChiefComplaint
{
    public interface IChiefComplaintDataProvider
    {
        Response<ChiefComplaintModel> GetChiefComplaint(long contactID);
        Response<ChiefComplaintModel> GetChiefComplaints(long contactID);
        Response<ChiefComplaintModel> AddChiefComplaint(ChiefComplaintModel chiefComplaint);
        Response<ChiefComplaintModel> UpdateChiefComplaint(ChiefComplaintModel chiefComplaint);
        Response<ChiefComplaintModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn);
    }
}