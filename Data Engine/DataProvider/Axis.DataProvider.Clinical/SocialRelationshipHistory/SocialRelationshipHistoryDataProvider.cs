using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Clinical;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Axis.DataProvider.Clinical
{
    public class SocialRelationshipHistoryDataProvider : ISocialRelationshipHistoryDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipHistoryDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SocialRelationshipHistoryDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<SocialRelationshipHistoryModel> GetSocialRelationship(long socialRelationshipID)
        {
            var socialRelationshipRepository = _unitOfWork.GetRepository<SocialRelationshipHistoryModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("SocialRelationshipID", socialRelationshipID) };
            var socialRelationshipResults = socialRelationshipRepository.ExecuteStoredProc("usp_GetSocialRelationship", procParams);

            if (socialRelationshipResults == null || socialRelationshipResults.ResultCode != 0) 
                return socialRelationshipResults;

            //for getting Financial Assessment Detail and list of assessment
            if (socialRelationshipResults.DataItems != null && socialRelationshipResults.DataItems != null & socialRelationshipResults.DataItems.Count > 0)
            {
                socialRelationshipResults.DataItems[0].Details = GetSocialRelationshipDetails(socialRelationshipResults.DataItems[0].SocialRelationshipID).DataItems;
            }

            return socialRelationshipResults;
        }

        /// <summary>
        /// Add social relationship history for contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> AddSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            var socialRelationshipRepository =
                _unitOfWork.GetRepository<SocialRelationshipHistoryModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter>();
            procParams.AddRange(new List<SqlParameter>() 
                {
                    new SqlParameter("ContactID", model.ContactID),
                    new SqlParameter("EncounterID", (object)model.EncounterID ?? DBNull.Value),
                    new SqlParameter("ReviewedNoChanges", model.ReviewedNoChanges),
                    new SqlParameter("TakenBy", model.TakenBy),
                    new SqlParameter("TakenTime", model.TakenTime),
                    new SqlParameter("ChildhoodHistory", model.ChildhoodHistory),
                    new SqlParameter("RelationshipHistory", model.RelationShipHistory),
                    new SqlParameter("FamilyHistory", model.FamilyHistory),
                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                }
            );
            var socialRelationshipResults = socialRelationshipRepository.ExecuteNQStoredProc("usp_AddSocialRelationship", procParams, false, true);

            return socialRelationshipResults;
        }

        /// <summary>
        /// Update social relationship history
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> UpdateSocialRelationHistory(SocialRelationshipHistoryModel model)
        {
            var socialRelationshipRepository = _unitOfWork.GetRepository<SocialRelationshipHistoryModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter>();
            procParams.AddRange(new List<SqlParameter>() 
                {
                    new SqlParameter("SocialRelationshipID", model.SocialRelationshipID),
                    new SqlParameter("ContactID", model.ContactID),
                    new SqlParameter("EncounterID", (object)model.EncounterID ?? DBNull.Value),
                    new SqlParameter("ReviewedNoChanges", model.ReviewedNoChanges),
                    new SqlParameter("TakenBy", model.TakenBy),
                    new SqlParameter("TakenTime", model.TakenTime),
                    new SqlParameter("ChildhoodHistory", (object)model.ChildhoodHistory ?? DBNull.Value),
                    new SqlParameter("RelationshipHistory", (object)model.RelationShipHistory ?? DBNull.Value),
                    new SqlParameter("FamilyHistory", (object)model.FamilyHistory ?? DBNull.Value),
                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                }
           );
            var socialRelationshipResults = socialRelationshipRepository.ExecuteNQStoredProc("usp_UpdateSocialRelationship", procParams);

            return socialRelationshipResults;
        }

        public Response<SocialRelationshipHistoryDetailsModel> AddSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            var srRepository = _unitOfWork.GetRepository<SocialRelationshipHistoryDetailsModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter>();
            SqlParameter socialRelationshipXMLParam = new SqlParameter("SocialRelationshipXML", GenerateDetailXML(model));
            socialRelationshipXMLParam.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now);
            procParams.Add(socialRelationshipXMLParam);
            procParams.Add(modifiedOnParam);
            var srResult = srRepository.ExecuteNQStoredProc("usp_AddSocialRelationshipDetail", procParams);

            return srResult;
        }

        public Response<SocialRelationshipHistoryDetailsModel> UpdateSocialRelationshipDetail(SocialRelationshipHistoryDetailsModel model)
        {
            var srRepository = _unitOfWork.GetRepository<SocialRelationshipHistoryDetailsModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter>();
            SqlParameter socialRelationshipXMLParam = new SqlParameter("SocialRelationshipXML", GenerateDetailXML(model));
            socialRelationshipXMLParam.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now);
            procParams.Add(socialRelationshipXMLParam);
            procParams.Add(modifiedOnParam);
            var srResult = srRepository.ExecuteNQStoredProc("usp_UpdateSocialRelationshipDetail", procParams);

            return srResult;
        }

        /// <summary>
        /// Remove social relationship history 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<SocialRelationshipHistoryModel> DeleteSocialRelationHistory(long Id, DateTime modifiedOn)
        {
            List<SqlParameter> procsParams = new List<SqlParameter> { new SqlParameter("SocialRelationshipDetailID", Id), new SqlParameter("ModifiedOn", modifiedOn) };
            var modelRepository = _unitOfWork.GetRepository<SocialRelationshipHistoryModel>(SchemaName.Clinical);
            return modelRepository.ExecuteNQStoredProc("usp_DeleteSocialRelationshipDetail", procsParams);
        }

        #endregion

        #region Private Methods

        private Response<SocialRelationshipHistoryDetailsModel> GetSocialRelationshipDetails(long socialRelationshipID)
        {
            var socialRelationshipDetailsRepository = _unitOfWork.GetRepository<SocialRelationshipHistoryDetailsModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("SocialRelationshipID", socialRelationshipID) };

            return socialRelationshipDetailsRepository.ExecuteStoredProc("usp_GetSocialRelationshipDetail", procParams);
        }

        private string GenerateDetailXML(SocialRelationshipHistoryDetailsModel model)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("SocialRelationship");
                    XWriter.WriteStartElement("SocialRelationshipDetails");

                    if (model.SocialRelationshipDetailID > 0)
                    {
                        XWriter.WriteElementString("SocialRelationshipDetailID", model.SocialRelationshipDetailID.ToString());
                    }

                    XWriter.WriteElementString("FamilyRelationshipID", model.FamilyRelationshipID.ToString());
                    XWriter.WriteElementString("SocialRelationshipID", model.SocialRelationshipID.ToString());
                    XWriter.WriteElementString("ContactID", model.ContactID.ToString());
                    XWriter.WriteElementString("RelationshipTypeID", model.RelationshipTypeID.ToString());
                    XWriter.WriteElementString("FirstName", model.FirstName);
                    XWriter.WriteElementString("LastName", model.LastName);
                    XWriter.WriteElementString("IsDeceased", model.IsDeceased.ToString());
                    XWriter.WriteElementString("IsInvolved", model.IsInvolved.ToString());

                    XWriter.WriteEndElement();
                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        #endregion
    }
}
