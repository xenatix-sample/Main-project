using System;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.ChiefComplaint;
using Axis.Model.Clinical;
using Axis.Model.Common;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class ChiefComplaintController : BaseApiController
    {
        #region Class Variables

        readonly IChiefComplaintDataProvider _dataProvider;

        #endregion

        #region Constructors

        public ChiefComplaintController(IChiefComplaintDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetChiefComplaint(long contactID)
        {
            return new HttpResult<Response<ChiefComplaintModel>>(_dataProvider.GetChiefComplaint(contactID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetChiefComplaints(long contactID)
        {
            return new HttpResult<Response<ChiefComplaintModel>>(_dataProvider.GetChiefComplaints(contactID), Request);
        }

        [HttpPost]
        public IHttpActionResult AddChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            return new HttpResult<Response<ChiefComplaintModel>>(_dataProvider.AddChiefComplaint(chiefComplaint), Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            return new HttpResult<Response<ChiefComplaintModel>>(_dataProvider.UpdateChiefComplaint(chiefComplaint), Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ChiefComplaintModel>>(_dataProvider.DeleteChiefComplaint(chiefComplaintID, modifiedOn), Request);
        }

        #endregion 
    }
}