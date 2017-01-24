using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Clinical.MedicalHistory;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;

namespace Axis.DataProvider.Clinical.MedicalHistory
{
    public class MedicalHistoryDataProvider : IMedicalHistoryDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public MedicalHistoryDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        public Response<MedicalHistoryModel> GetMedicalHistoryBundle(long contactID)
        {
            var repository = _unitOfWork.GetRepository<MedicalHistoryModel>(SchemaName.Clinical);

            SqlParameter contactIDParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam };
            var result = repository.ExecuteStoredProc("usp_GetMedicalHistoryByContactID", procParams);

            return result;
        }

        //public Response<MedicalHistoryConditionModel> GetMedicalHistoryConditionDetails(long medicalHistoryID)
        //{
        //    var repository = _unitOfWork.GetRepository<MedicalHistoryConditionModel>(SchemaName.Clinical);

        //    SqlParameter medicalHistoryIDParam = new SqlParameter("MedicalHistoryID", medicalHistoryID);
        //    List<SqlParameter> procParams = new List<SqlParameter>() { medicalHistoryIDParam };
        //    var conditionsResult = repository.ExecuteStoredProc("usp_GetMedicalHistoryConditions", procParams);

        //    for (int i = 0; i < conditionsResult.DataItems.Count; i++)
        //    {
        //        var relationshipsResult = GetConditionFamilyRelationships(conditionsResult.DataItems[i].MedicalHistoryConditionID);
        //        conditionsResult.DataItems[i].Details = relationshipsResult.DataItems;
        //    }

        //    return conditionsResult;
        //}

        public Response<MedicalHistoryModel> DeleteMedicalHistory(long medicalHistoryID, DateTime modifiedOn)
        {
            var repository = _unitOfWork.GetRepository<MedicalHistoryModel>(SchemaName.Clinical);

            SqlParameter medicalHistoryIDParam = new SqlParameter("MedicalHistoryID", medicalHistoryID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { medicalHistoryIDParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<MedicalHistoryModel>>(repository.ExecuteNQStoredProc, "usp_DeleteMedicalHistory", procParams);

            return result;
        }

        public Response<MedicalHistoryModel> AddMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            var repository = _unitOfWork.GetRepository<MedicalHistoryModel>(SchemaName.Clinical);

            SqlParameter contactIDParam = new SqlParameter("ContactID", medicalHistory.ContactID);
            SqlParameter encounterIDParam = new SqlParameter("EncounterID", medicalHistory.EncounterID ?? (object)DBNull.Value);
            SqlParameter takenByParam = new SqlParameter("TakenBy", medicalHistory.TakenBy);
            SqlParameter takenTimeParam = new SqlParameter("TakenTime", medicalHistory.TakenTime);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", medicalHistory.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam, encounterIDParam, takenByParam, takenTimeParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<MedicalHistoryModel>>(repository.ExecuteNQStoredProc, "usp_AddMedicalHistory", procParams, false, true);

            return result;
        }

        public Response<MedicalHistoryModel> UpdateMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            var repository = _unitOfWork.GetRepository<MedicalHistoryModel>(SchemaName.Clinical);

            SqlParameter medicalHistoryIDParam = new SqlParameter("MedicalHistoryID", medicalHistory.MedicalHistoryID);
            SqlParameter contactIDParam = new SqlParameter("ContactID", medicalHistory.ContactID);
            SqlParameter encounterIDParam = new SqlParameter("EncounterID", medicalHistory.EncounterID ?? (object)DBNull.Value);
            SqlParameter takenByParam = new SqlParameter("TakenBy", medicalHistory.TakenBy);
            SqlParameter takenTimeParam = new SqlParameter("TakenTime", medicalHistory.TakenTime);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", medicalHistory.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { medicalHistoryIDParam, contactIDParam, encounterIDParam, takenByParam, takenTimeParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<MedicalHistoryModel>>(repository.ExecuteNQStoredProc, "usp_UpdateMedicalHistory", procParams);

            return result;
        }

        public Response<MedicalHistoryModel> SaveMedicalHistoryDetails(MedicalHistoryModel medicalHistory)
        {
            var repository = _unitOfWork.GetRepository<MedicalHistoryModel>(SchemaName.Clinical);

            SqlParameter medicalHistoryXMLParam = new SqlParameter("MedicalHistoryXML", GenerateRequestXml(medicalHistory));
            SqlParameter contactIDParam = new SqlParameter("ContactID", medicalHistory.ContactID);
            medicalHistoryXMLParam.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", medicalHistory.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { medicalHistoryXMLParam, contactIDParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<MedicalHistoryModel>>(repository.ExecuteNQStoredProc, "usp_SaveMedicalHistoryDetails", procParams);

            return result;
        }

        //public Response<MedicalHistoryConditionModel> GetAllMedicalConditions(long medicalHistoryID)
        //{
        //    var repository = _unitOfWork.GetRepository<MedicalHistoryConditionModel>(SchemaName.Clinical);

        //    SqlParameter medicalHistoryIDParam = new SqlParameter("MedicalHistoryID", medicalHistoryID);
        //    List<SqlParameter> procParams = new List<SqlParameter>() { medicalHistoryIDParam };
        //    var result = repository.ExecuteStoredProc("usp_GetMedicalConditionsByMedicalHistoryID", procParams);

        //    for (int i = 0; i < result.DataItems.Count; i++)
        //    {
        //        if (result.DataItems[i].MedicalHistoryConditionID != null && result.DataItems[i].MedicalHistoryConditionID != 0)
        //        {
        //            var relationshipsResult = GetConditionFamilyRelationships(result.DataItems[i].MedicalHistoryConditionID);
        //            result.DataItems[i].Details = relationshipsResult.DataItems;
        //        }
        //    }

        //    return result;
        //}

        public Response<MedicalHistoryModel> GetMedicalHistoryConditionDetails(long medicalHistoryID)
        {
            var repository = _unitOfWork.GetRepository<MedicalHistoryConditionModel>(SchemaName.Clinical);

            SqlParameter medicalHistoryIDParam = new SqlParameter("MedicalHistoryID", medicalHistoryID);
            List<SqlParameter> procParams = new List<SqlParameter>() { medicalHistoryIDParam };
            var conditionsResult = repository.ExecuteStoredProc("usp_GetMedicalHistoryConditions", procParams);

            for (int i = 0; i < conditionsResult.DataItems.Count; i++)
            {
                var relationshipsResult = GetConditionFamilyRelationships(conditionsResult.DataItems[i].MedicalHistoryConditionID);
                conditionsResult.DataItems[i].Details = relationshipsResult.DataItems;
            }

            var retResult = new Response<MedicalHistoryModel>();
            var dataItem = new List<MedicalHistoryModel>();

            if (conditionsResult.DataItems.Count > 0)
            {
                var model = new MedicalHistoryModel();
                model.MedicalHistoryID = medicalHistoryID;
                model.Conditions = conditionsResult.DataItems;
                dataItem.Add(model);
            }

            retResult.DataItems = dataItem;
            return retResult;
        }

        public Response<MedicalHistoryModel> GetAllMedicalConditions(long medicalHistoryID)
        {
            var repository = _unitOfWork.GetRepository<MedicalHistoryConditionModel>(SchemaName.Clinical);

            SqlParameter medicalHistoryIDParam = new SqlParameter("MedicalHistoryID", medicalHistoryID);
            List<SqlParameter> procParams = new List<SqlParameter>() { medicalHistoryIDParam };
            var result = repository.ExecuteStoredProc("usp_GetMedicalConditionsByMedicalHistoryID", procParams);

            for (int i = 0; i < result.DataItems.Count; i++)
            {
                if (result.DataItems[i].MedicalHistoryConditionID != null && result.DataItems[i].MedicalHistoryConditionID != 0)
                {
                    var relationshipsResult = GetConditionFamilyRelationships(result.DataItems[i].MedicalHistoryConditionID);
                    result.DataItems[i].Details = relationshipsResult.DataItems;
                }
            }

            var retResult = new Response<MedicalHistoryModel>();
            var dataItem = new List<MedicalHistoryModel>();

            if (result.DataItems.Count > 0)
            {
                var model = new MedicalHistoryModel();
                model.MedicalHistoryID = medicalHistoryID;
                model.Conditions = result.DataItems;
                dataItem.Add(model);
            }

            retResult.DataItems = dataItem;
            return retResult;
            }

        public Response<MedicalHistoryModel> GetMedicalHistory(long medicalHistoryID) {
            var repository = _unitOfWork.GetRepository<MedicalHistoryModel>(SchemaName.Clinical);
            SqlParameter param = new SqlParameter("MedicalHistoryID", medicalHistoryID);
            List<SqlParameter> procParams = new List<SqlParameter>() { param };
            return repository.ExecuteStoredProc("usp_GetMedicalHistory", procParams);
        }

        #endregion

        #region Private Methods

        private string GenerateRequestXml(MedicalHistoryModel medicalHistory)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("MedicalHistory");
                    XWriter.WriteElementString("MedicalHistoryID", medicalHistory.MedicalHistoryID.ToString());

                    if (medicalHistory.Conditions != null)
                    {
                        foreach (MedicalHistoryConditionModel condition in medicalHistory.Conditions)
                        {
                            XWriter.WriteStartElement("MedicalHistoryCondition");
                            XWriter.WriteElementString("MedicalHistoryConditionID", condition.MedicalHistoryConditionID.ToString());
                            XWriter.WriteElementString("MedicalConditionID", condition.MedicalConditionID.ToString());
                            XWriter.WriteElementString("HasCondition", condition.HasCondition.ToString());

                            foreach (MedicalHistoryConditionDetailModel detail in condition.Details)
                            {
                                XWriter.WriteStartElement("MedicalHistoryConditionDetail");
                                    XWriter.WriteElementString("MedicalHistoryConditionDetailID", detail.MedicalHistoryConditionDetailID.ToString());
                                    XWriter.WriteElementString("IsSelf", detail.IsSelf.ToString());
                                    if (detail.FamilyRelationshipID > 0) XWriter.WriteElementString("FamilyRelationshipID", detail.FamilyRelationshipID.ToString());
                                    if (detail.RelationshipTypeID > 0) XWriter.WriteElementString("RelationshipTypeID", detail.RelationshipTypeID.ToString());
                                    XWriter.WriteElementString("FirstName", detail.FirstName ?? string.Empty);
                                    XWriter.WriteElementString("LastName", detail.LastName ?? string.Empty);
                                    XWriter.WriteElementString("IsDeceased", detail.IsDeceased.ToString());
                                    XWriter.WriteElementString("Comments", detail.Comments ?? string.Empty);
                                    XWriter.WriteElementString("IsActive", detail.IsActive.ToString());
                                    XWriter.WriteEndElement();
                            }

                            XWriter.WriteEndElement();
                        }
                    }

                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        private Response<MedicalHistoryConditionDetailModel> GetConditionFamilyRelationships(long? medicalHistoryConditionID)
        {
            var repository = _unitOfWork.GetRepository<MedicalHistoryConditionDetailModel>(SchemaName.Clinical);

            SqlParameter medicalHistoryConditionIDParam = new SqlParameter("MedicalHistoryConditionID", medicalHistoryConditionID);
            List<SqlParameter> procParams = new List<SqlParameter>() { medicalHistoryConditionIDParam };
            var detailsResult = repository.ExecuteStoredProc("usp_GetMedicalHistoryConditionDetails", procParams);

            return detailsResult;
        }

        #endregion
    }
}
