using System;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.MedicalHistory;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class MedicalHistoryController : BaseApiController
    {
        #region Class Variables

        readonly IMedicalHistoryDataProvider _medicalHistoryDataProvider = null;

        #endregion

        #region Constructors

        public MedicalHistoryController(IMedicalHistoryDataProvider medicalHistoryDataProvider)
        {
            _medicalHistoryDataProvider = medicalHistoryDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetMedicalHistoryBundle(long contactID)
        {
            var response = _medicalHistoryDataProvider.GetMedicalHistoryBundle(contactID);
            return new HttpResult<Response<MedicalHistoryModel>>(response, Request);
        }

        [HttpGet]
        public IHttpActionResult GetMedicalHistoryConditionDetails(long medicalHistoryID)
        {
            var response = _medicalHistoryDataProvider.GetMedicalHistoryConditionDetails(medicalHistoryID);
            return new HttpResult<Response<MedicalHistoryModel>>(response, Request);
        }

        [HttpGet]
        public IHttpActionResult GetAllMedicalConditions(long medicalHistoryID)
        {
            var response = _medicalHistoryDataProvider.GetAllMedicalConditions(medicalHistoryID);
            return new HttpResult<Response<MedicalHistoryModel>>(response, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn)
        {
            var response = _medicalHistoryDataProvider.DeleteMedicalHistory(medicalHistoryID, modifiedOn);
            return new HttpResult<Response<MedicalHistoryModel>>(response, Request);
        }

        [HttpPost]
        public IHttpActionResult AddMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            var response = _medicalHistoryDataProvider.AddMedicalHistory(medicalHistory);
            return new HttpResult<Response<MedicalHistoryModel>>(response, Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            var response = _medicalHistoryDataProvider.UpdateMedicalHistory(medicalHistory);
            return new HttpResult<Response<MedicalHistoryModel>>(response, Request);
        }

        [HttpPost]
        public IHttpActionResult SaveMedicalHistoryDetails(MedicalHistoryModel medicalHistory)
        {
            var response = _medicalHistoryDataProvider.SaveMedicalHistoryDetails(medicalHistory);
            return new HttpResult<Response<MedicalHistoryModel>>(response, Request);
        }

        [HttpGet]
        public IHttpActionResult GetMedicalHistory(long medicalHistoryID)
        {
            var response = _medicalHistoryDataProvider.GetMedicalHistory(medicalHistoryID);
            return new HttpResult<Response<MedicalHistoryModel>>(response, Request);
        }

        #endregion
    }
}
