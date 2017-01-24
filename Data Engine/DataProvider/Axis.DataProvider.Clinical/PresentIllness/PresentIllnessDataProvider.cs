using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;

namespace Axis.DataProvider.Clinical.PresentIllness
{
    public class PresentIllnessDataProvider : IPresentIllnessDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public PresentIllnessDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        public Response<PresentIllnessModel> GetHPIBundle(long contactID)
        {
            var hpiRepository = _unitOfWork.GetRepository<PresentIllnessModel>(SchemaName.Clinical);

            SqlParameter contactIDParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam }; ;
            var hpiBundleResult = hpiRepository.ExecuteStoredProc("usp_GetHPIByContactID", procParams);

            //We are only getting the most recent/active bundle
            return hpiBundleResult;
        }

        public Response<PresentIllnessModel> GetHPI(long HPIID)
        {
            var hpiRepository = _unitOfWork.GetRepository<PresentIllnessModel>(SchemaName.Clinical);

            SqlParameter HPIIDParam = new SqlParameter("HPIID", HPIID);
            List<SqlParameter> procParams = new List<SqlParameter>() { HPIIDParam }; ;
            var hpiBundleResult = hpiRepository.ExecuteStoredProc("usp_GetHPI", procParams);

            //We are only getting the most recent/active bundle
            return hpiBundleResult;
        }

        public Response<PresentIllnessDetailModel> GetHPIDetail(long HPIDetailID)
        {
            var hpiRepository = _unitOfWork.GetRepository<PresentIllnessDetailModel>(SchemaName.Clinical);

            SqlParameter HPIIDParam = new SqlParameter("HPIID", HPIDetailID);
            List<SqlParameter> procParams = new List<SqlParameter>() { HPIIDParam }; ;
            var hpiDetailResult = hpiRepository.ExecuteStoredProc("usp_GetHPIDetail", procParams);

            return hpiDetailResult;
        }

        public Response<PresentIllnessModel> DeleteHPI(long HPIID, DateTime modifiedOn)
        {
            var hpiRepository = _unitOfWork.GetRepository<PresentIllnessModel>(SchemaName.Clinical);

            SqlParameter HPIIDParam = new SqlParameter("HPIID", HPIID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { HPIIDParam, modifiedOnParam }; ;
            var hpiBundleResult = hpiRepository.ExecuteNQStoredProc("usp_DeleteHPI", procParams);
            
            //We are only getting the most recent/active bundle
            return hpiBundleResult;
        }

        public Response<PresentIllnessDetailModel> DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn)
        {

            var hpiRepository = _unitOfWork.GetRepository<PresentIllnessDetailModel>(SchemaName.Clinical);

            SqlParameter HPIDetailIDParam = new SqlParameter("HPIDetailID", HPIDetailID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { HPIDetailIDParam, modifiedOnParam }; ;
            var hpiDetailResult = hpiRepository.ExecuteNQStoredProc("usp_DeleteHPIDetail", procParams);

             return hpiDetailResult;
        }

        public Response<PresentIllnessModel> AddHPI(PresentIllnessModel hpi)
        {
               var hpiRepository = _unitOfWork.GetRepository<PresentIllnessModel>(SchemaName.Clinical);
            
               SqlParameter contactIDParam = new SqlParameter("ContactID", hpi.ContactID);
               SqlParameter EnocunterIDParam = new SqlParameter("EncounterID", hpi.EncounterID ?? (object)DBNull.Value);
               SqlParameter TakenByParam = new SqlParameter("TakenBy", hpi.TakenBy);
               SqlParameter TakenOnParam = new SqlParameter("TakenOn", hpi.TakenTime);
               SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", hpi.ModifiedOn ?? DateTime.Now);
               List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam, EnocunterIDParam, TakenByParam, TakenOnParam, modifiedOnParam };
               var hpiResult = _unitOfWork.EnsureInTransaction<Response<PresentIllnessModel>>(hpiRepository.ExecuteNQStoredProc, "usp_AddHPI", procParams, false, true);

               return hpiResult; 
           
        }

        public Response<PresentIllnessDetailModel> AddHPIDetail(PresentIllnessDetailModel hpi)
        {
            var hpiRepository = _unitOfWork.GetRepository<PresentIllnessDetailModel>(SchemaName.Clinical);

            SqlParameter HPIXMLParam = new SqlParameter("HPIXML", GenerateRequestXml(hpi));
            HPIXMLParam.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", hpi.ModifiedOn ?? DateTime.Now);

            List<SqlParameter> procParams = new List<SqlParameter>() { HPIXMLParam, modifiedOnParam };
            var hpiResult = _unitOfWork.EnsureInTransaction<Response<PresentIllnessDetailModel>>(hpiRepository.ExecuteNQStoredProc, "usp_AddHPIDetail", procParams, false, true);

            return hpiResult;
        }

        public Response<PresentIllnessModel> UpdateHPI(PresentIllnessModel hpi)
        {
            var hpiRepository = _unitOfWork.GetRepository<PresentIllnessModel>(SchemaName.Clinical);

            SqlParameter HPIIDParam = new SqlParameter("HPIID", hpi.HPIID);
            SqlParameter contactIDParam = new SqlParameter("ContactID", hpi.ContactID);
            SqlParameter EncounterIDParam = new SqlParameter("EncounterID", hpi.EncounterID ?? (object)DBNull.Value);
            SqlParameter takenByParam = new SqlParameter("TakenBy", hpi.TakenBy);
            SqlParameter takenonParam = new SqlParameter("TakenOn", hpi.TakenTime);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", hpi.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { HPIIDParam, contactIDParam, EncounterIDParam, takenByParam, takenonParam, modifiedOnParam };
            var hpiResult = _unitOfWork.EnsureInTransaction<Response<PresentIllnessModel>>(hpiRepository.ExecuteNQStoredProc, "usp_UpdateHPI", procParams);
            
            return hpiResult;
        }

        public Response<PresentIllnessDetailModel> UpdateHPIDetail(PresentIllnessDetailModel hpi)
        {
            var hpiRepository = _unitOfWork.GetRepository<PresentIllnessDetailModel>(SchemaName.Clinical);

            SqlParameter HPIXMLParam = new SqlParameter("HPIXML", GenerateRequestXml(hpi));
            HPIXMLParam.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", hpi.ModifiedOn ?? DateTime.Now);

            List<SqlParameter> procParams = new List<SqlParameter>() { HPIXMLParam, modifiedOnParam };
            var hpiResult = _unitOfWork.EnsureInTransaction<Response<PresentIllnessDetailModel>>(hpiRepository.ExecuteNQStoredProc, "usp_UpdateHPIDetail", procParams);

            return hpiResult;
        }

        private string GenerateRequestXml(PresentIllnessDetailModel hpi)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("HPI");

                    XWriter.WriteStartElement("HPIDetails");

                    XWriter.WriteElementString("HPIID", hpi.HPIID.ToString());
                    XWriter.WriteElementString("HPIDetailID", hpi.HPIDetailID.ToString());
                    XWriter.WriteElementString("Comment", hpi.Comment);
                    XWriter.WriteElementString("Location", hpi.Location);
                    if(hpi.HPISeverityID.HasValue){
                      XWriter.WriteElementString("HPISeverityID", hpi.HPISeverityID.ToString());
                    }
                    XWriter.WriteElementString("Quality", hpi.Quality);
                    XWriter.WriteElementString("Duration", hpi.Duration);
                    XWriter.WriteElementString("Timing", hpi.Timing);
                    XWriter.WriteElementString("Context", hpi.Context);
                    XWriter.WriteElementString("ModifyingFactors", hpi.Modifyingfactors);
                    XWriter.WriteElementString("Conditions", hpi.Conditions);
                    XWriter.WriteElementString("Symptoms", hpi.Symptoms);

                    XWriter.WriteEndElement(); //HPIDetails

                    XWriter.WriteEndElement(); //HPI
                    
                }

                return sWriter.ToString();
            }
        }
    }
}
