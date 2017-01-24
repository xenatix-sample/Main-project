using System;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;
using Axis.Service.Clinical.MedicalHistory;

namespace Axis.RuleEngine.Clinical.MedicalHistory
{
    public class MedicalHistoryRuleEngine : IMedicalHistoryRuleEngine
    {
        #region Class Variables

        private readonly IMedicalHistoryService _medicalHistoryService;

        #endregion

        #region Constructors

        public MedicalHistoryRuleEngine(IMedicalHistoryService medicalHistoryService)
        {
            _medicalHistoryService = medicalHistoryService;
        }

        #endregion

        #region Public Methods

        public Response<MedicalHistoryModel> GetMedicalHistoryBundle(long contactID)
        {
            return _medicalHistoryService.GetMedicalHistoryBundle(contactID);
        }

        public Response<MedicalHistoryModel> GetMedicalHistoryConditionDetails(long medicalHistoryID)
        {
            return _medicalHistoryService.GetMedicalHistoryConditionDetails(medicalHistoryID);
        }

        public Response<MedicalHistoryModel> GetAllMedicalConditions(long medicalHistoryID)
        {
            return _medicalHistoryService.GetAllMedicalConditions(medicalHistoryID);
        }

        public Response<MedicalHistoryModel> DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn)
        {
            return _medicalHistoryService.DeleteMedicalHistory(medicalHistoryID, modifiedOn);
        }

        public Response<MedicalHistoryModel> AddMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            return _medicalHistoryService.AddMedicalHistory(medicalHistory);
        }

        public Response<MedicalHistoryModel> UpdateMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            return _medicalHistoryService.UpdateMedicalHistory(medicalHistory);
        }

        public Response<MedicalHistoryModel> SaveMedicalHistoryDetails(MedicalHistoryModel medicalHistory)
        {
            return _medicalHistoryService.SaveMedicalHistoryDetails(medicalHistory);
        }

        public Response<MedicalHistoryModel> GetMedicalHistory(long medicalHistoryID) {
            return _medicalHistoryService.GetMedicalHistory(medicalHistoryID);
        }

        #endregion
    }
}
