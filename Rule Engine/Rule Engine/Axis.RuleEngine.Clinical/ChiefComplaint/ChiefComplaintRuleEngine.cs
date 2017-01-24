using System;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Service.Clinical.ChiefComplaint;

namespace Axis.RuleEngine.Clinical.ChiefComplaint
{
    public class ChiefComplaintRuleEngine : IChiefComplaintRuleEngine
    {
        private readonly IChiefComplaintService _service;

        public ChiefComplaintRuleEngine(IChiefComplaintService service)
        {
            _service = service;
        }

        public Response<ChiefComplaintModel> GetChiefComplaint(long contactId)
        {
            return _service.GetChiefComplaint(contactId);
        }

        public Response<ChiefComplaintModel> GetChiefComplaints(long contactId)
        {
            return _service.GetChiefComplaints(contactId);
        }

        public Response<ChiefComplaintModel> AddChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            return _service.AddChiefComplaint(chiefComplaint);
        }

        public Response<ChiefComplaintModel> UpdateChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            return _service.UpdateChiefComplaint(chiefComplaint);
        }

        public Response<ChiefComplaintModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn)
        {
            return _service.DeleteChiefComplaint(chiefComplaintID, modifiedOn);
        }
    }
}
