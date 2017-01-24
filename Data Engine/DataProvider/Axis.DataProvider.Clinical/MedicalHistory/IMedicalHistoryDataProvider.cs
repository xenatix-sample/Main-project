using System;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;

namespace Axis.DataProvider.Clinical.MedicalHistory
{
    public interface IMedicalHistoryDataProvider
    {
        Response<MedicalHistoryModel> GetMedicalHistoryBundle(long contactID);
        Response<MedicalHistoryModel> GetMedicalHistory(long medicalHistoryID);
        //Response<MedicalHistoryConditionModel> GetMedicalHistoryConditionDetails(long medicalHistoryID);
        //Response<MedicalHistoryConditionModel> GetAllMedicalConditions(long medicalHistoryID);
        Response<MedicalHistoryModel> DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn);
        Response<MedicalHistoryModel> AddMedicalHistory(MedicalHistoryModel medicalHistory);
        Response<MedicalHistoryModel> UpdateMedicalHistory(MedicalHistoryModel medicalHistory);
        Response<MedicalHistoryModel> SaveMedicalHistoryDetails(MedicalHistoryModel medicalHistory);
        Response<MedicalHistoryModel> GetMedicalHistoryConditionDetails(long medicalHistoryID);
        Response<MedicalHistoryModel> GetAllMedicalConditions(long medicalHistoryID);
    }
}
