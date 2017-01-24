using System;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.PresentIllness;
using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class PresentIllnessController : BaseApiController
    {
        #region Class Variables

        readonly IPresentIllnessDataProvider _PresentIllnessDataProvider = null;

        #endregion

        #region Constructors

        public PresentIllnessController(IPresentIllnessDataProvider presentIllnessDataProvider)

        {
            _PresentIllnessDataProvider = presentIllnessDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetHPIBundle(long contactID)
        {
            var hpiResponse = _PresentIllnessDataProvider.GetHPIBundle(contactID);
            return new HttpResult<Response<PresentIllnessModel>>(hpiResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetHPI(long HPIID)
        {
            var HPIResponse = _PresentIllnessDataProvider.GetHPI(HPIID);
            return new HttpResult<Response<PresentIllnessModel>>(HPIResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetHPIDetail(long HPIID)
        {
            var HPIResponse = _PresentIllnessDataProvider.GetHPIDetail(HPIID);
            return new HttpResult<Response<PresentIllnessDetailModel>>(HPIResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteHPI(long HPIID, DateTime modifiedOn)
        {
            var HPIResponse = _PresentIllnessDataProvider.DeleteHPI(HPIID, modifiedOn);
            return new HttpResult<Response<PresentIllnessModel>>(HPIResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn)
        {
            var HPIResponse = _PresentIllnessDataProvider.DeleteHPIDetail(HPIDetailID, modifiedOn);
            return new HttpResult<Response<PresentIllnessDetailModel>>(HPIResponse, Request);
            }

        [HttpPost]
        public IHttpActionResult AddHPI(PresentIllnessModel hpi)
        {
            var HPIResponse = _PresentIllnessDataProvider.AddHPI(hpi);
            return new HttpResult<Response<PresentIllnessModel>>(HPIResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddHPIDetail(PresentIllnessDetailModel HPI)
        {
            var HPIResponse = _PresentIllnessDataProvider.AddHPIDetail(HPI);
            return new HttpResult<Response<PresentIllnessDetailModel>>(HPIResponse, Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateHPI(PresentIllnessModel hpi)
        {
            var HPIResponse = _PresentIllnessDataProvider.UpdateHPI(hpi);
            return new HttpResult<Response<PresentIllnessModel>>(HPIResponse, Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateHPIDetail(PresentIllnessDetailModel hpi)
        {
            var HPIResponse = _PresentIllnessDataProvider.UpdateHPIDetail(hpi);
            return new HttpResult<Response<PresentIllnessDetailModel>>(HPIResponse, Request);
        }
        #endregion
    }
}