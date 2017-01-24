using System;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.ChiefComplaint;

namespace Axis.Plugins.Clinical.Repository.ChiefComplaint
{
    public interface IChiefComplaintRepository
    {
        Response<ChiefComplaintViewModel> GetChiefComplaint(long contactID);
        Response<ChiefComplaintViewModel> GetChiefComplaints(long contactID);
        Response<ChiefComplaintViewModel> AddChiefComplaint(ChiefComplaintViewModel chiefComplaint);
        Response<ChiefComplaintViewModel> UpdateChiefComplaint(ChiefComplaintViewModel chiefComplaint);
        Response<ChiefComplaintViewModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn);
    }
}
