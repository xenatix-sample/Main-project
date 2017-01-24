using System;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.MedicalHistory;

namespace Axis.Plugins.Clinical.Repository.MedicalHistory
{
    public interface IMedicalHistoryRepository
    {
        Response<MedicalHistoryViewModel> GetMedicalHistoryBundle(long contactID);
        Response<MedicalHistoryViewModel> GetMedicalHistoryConditionDetails(long medicalHistoryID);
        Response<MedicalHistoryViewModel> GetAllMedicalConditions(long medicalHistoryID);
        Response<MedicalHistoryViewModel> DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn);
        Response<MedicalHistoryViewModel> AddMedicalHistory(MedicalHistoryViewModel medicalHistory);
        Response<MedicalHistoryViewModel> UpdateMedicalHistory(MedicalHistoryViewModel medicalHistory);
        Response<MedicalHistoryViewModel> SaveMedicalHistoryDetails(MedicalHistoryViewModel medicalHistory);
        Response<MedicalHistoryViewModel> GetMedicalHistory(long medicalHistoryID);
    }
}
