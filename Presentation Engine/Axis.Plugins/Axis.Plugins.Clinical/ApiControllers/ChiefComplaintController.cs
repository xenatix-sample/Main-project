using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.ChiefComplaint;
using Axis.Plugins.Clinical.Repository.ChiefComplaint;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.ApiControllers
{
    public class ChiefComplaintController : BaseApiController
    {
        private readonly IChiefComplaintRepository _repository;

        public ChiefComplaintController(IChiefComplaintRepository repository)
        {
            _repository = repository;
        }

        public ChiefComplaintController()
        {

        }

        [HttpGet]
        public Response<ChiefComplaintViewModel> GetChiefComplaint(long contactID)
        {
            return _repository.GetChiefComplaint(contactID);
        }

        [HttpGet]
        public Response<ChiefComplaintViewModel> GetChiefComplaints(long contactID)
        {
            return _repository.GetChiefComplaints(contactID);
        }

        [HttpPost]
        public Response<ChiefComplaintViewModel> AddChiefComplaint(ChiefComplaintViewModel chiefComplaint)
        {
            return _repository.AddChiefComplaint(chiefComplaint);
        }

        [HttpPut]
        public Response<ChiefComplaintViewModel> UpdateChiefComplaint(ChiefComplaintViewModel chiefComplaint)
        {
            return _repository.UpdateChiefComplaint(chiefComplaint);
        }

        [HttpDelete]
        public Response<ChiefComplaintViewModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _repository.DeleteChiefComplaint(chiefComplaintID, modifiedOn);
        }
    }
}
