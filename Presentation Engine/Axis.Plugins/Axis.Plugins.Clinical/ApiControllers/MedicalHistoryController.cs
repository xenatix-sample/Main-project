using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.MedicalHistory;
using Axis.Plugins.Clinical.Repository.MedicalHistory;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.ApiControllers
{
    public class MedicalHistoryController : BaseApiController
    {
        #region Class Variables

        private readonly IMedicalHistoryRepository _medicalHistoryRepository;

        #endregion

        #region Constructors

        public MedicalHistoryController(IMedicalHistoryRepository medicalHistoryRepository)
        {
            this._medicalHistoryRepository = medicalHistoryRepository;
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Get the MH bundle: MedicalHistoryID
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<MedicalHistoryViewModel> GetMedicalHistoryBundle(long contactID)
        {
            return _medicalHistoryRepository.GetMedicalHistoryBundle(contactID);
        }

        /// <summary>
        /// Get the medical conditions that are saved under a bundle
        /// </summary>
        /// <param name="medicalHistoryID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<MedicalHistoryViewModel> GetMedicalHistoryConditionDetails(long medicalHistoryID)
        {
            return _medicalHistoryRepository.GetMedicalHistoryConditionDetails(medicalHistoryID);
        }

        /// <summary>
        /// Get all medical conditions that are available for selection
        /// </summary>
        /// <param name="medicalHistoryID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<MedicalHistoryViewModel> GetAllMedicalConditions(long medicalHistoryID)
        {
            return _medicalHistoryRepository.GetAllMedicalConditions(medicalHistoryID);
        }

        [HttpDelete]
        public Response<MedicalHistoryViewModel> DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _medicalHistoryRepository.DeleteMedicalHistory(medicalHistoryID, modifiedOn);
        }

        [HttpPost]
        public Response<MedicalHistoryViewModel> AddMedicalHistory(MedicalHistoryViewModel medicalHistory)
        {
            medicalHistory.TakenTime = medicalHistory.TakenTime.ToUniversalTime();
            return _medicalHistoryRepository.AddMedicalHistory(medicalHistory);
        }

        [HttpPost]
        public Response<MedicalHistoryViewModel> UpdateMedicalHistory(MedicalHistoryViewModel medicalHistory)
        {
            medicalHistory.TakenTime = medicalHistory.TakenTime.ToUniversalTime();
            return _medicalHistoryRepository.UpdateMedicalHistory(medicalHistory);
        }

        [HttpPost]
        public Response<MedicalHistoryViewModel> SaveMedicalHistoryDetails(MedicalHistoryViewModel medicalHistory)
        {
            return _medicalHistoryRepository.SaveMedicalHistoryDetails(medicalHistory);
        }

        [HttpGet]
        public Response<MedicalHistoryViewModel> GetMedicalHistory(long medicalHistoryID)
        {
            return _medicalHistoryRepository.GetMedicalHistory(medicalHistoryID);
        }
        
        #endregion
    }
}
