﻿using System;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;

namespace Axis.Service.Clinical.MedicalHistory
{
    public interface IMedicalHistoryService
    {
        Response<MedicalHistoryModel> GetMedicalHistoryBundle(long contactID);
        Response<MedicalHistoryModel> GetMedicalHistoryConditionDetails(long medicalHistoryID);
        Response<MedicalHistoryModel> GetAllMedicalConditions(long medicalHistoryID);
        //Response<MedicalHistoryDetailModel> GetMedicalHistoryDetails(long medicalHistoryConditionID);
        Response<MedicalHistoryModel> DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn);
        Response<MedicalHistoryModel> AddMedicalHistory(MedicalHistoryModel medicalHistory);
        Response<MedicalHistoryModel> UpdateMedicalHistory(MedicalHistoryModel medicalHistory);
        Response<MedicalHistoryModel> SaveMedicalHistoryDetails(MedicalHistoryModel medicalHistory);
        Response<MedicalHistoryModel> GetMedicalHistory(long medicalHistoryID);
    }
}