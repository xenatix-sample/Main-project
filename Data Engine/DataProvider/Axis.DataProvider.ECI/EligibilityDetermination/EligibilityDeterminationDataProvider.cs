using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.ECI.EligibilityDetermination;

namespace Axis.DataProvider.ECI.EligibilityDetermination
{
    public class EligibilityDeterminationDataProvider : IEligibilityDeterminationDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public EligibilityDeterminationDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        public Response<EligibilityDeterminationModel> GetEligibilityDetermination(long contactID)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityDeterminationModel>(SchemaName.ECI);

            SqlParameter contactIDParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam };
            var eligibilityDeterminationResult = eligibilityDeterminationRepository.ExecuteStoredProc("usp_GetEligibility", procParams);

            return eligibilityDeterminationResult;
        }

        public Response<EligibilityDeterminationModel> GetEligibility(long eligibilityID)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityDeterminationModel>(SchemaName.ECI);

            SqlParameter eligibilityIDParam = new SqlParameter("EligibilityID", eligibilityID);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityIDParam };
            var eligibilityDeterminationResult = eligibilityDeterminationRepository.ExecuteStoredProc("usp_GetEligibilityDetail", procParams);

            return eligibilityDeterminationResult;
        }

        public Response<EligibilityTeamMemberModel> GetContactEligibilityMembers(long contactID)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityTeamMemberModel>(SchemaName.ECI);

            SqlParameter contactIDParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam };
            var eligibilityDeterminationResult = eligibilityDeterminationRepository.ExecuteStoredProc("usp_GetEligibilityTeamDisciplines", procParams);

            return eligibilityDeterminationResult;
        }

        public Response<EligibilityTeamMemberModel> GetEligibilityMembers(long eligibilityID)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityTeamMemberModel>(SchemaName.ECI);

            SqlParameter eligibilityIDParam = new SqlParameter("EligibilityID", eligibilityID);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityIDParam };
            var eligibilityDeterminationResult = eligibilityDeterminationRepository.ExecuteStoredProc("usp_GetEligibilityTeamDisciplinesByID", procParams);

            return eligibilityDeterminationResult;
        }

        public Response<EligibilityDeterminationModel> GetEligibilityNote(long eligibilityID)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityDeterminationModel>(SchemaName.ECI);

            SqlParameter eligibilityIDParam = new SqlParameter("EligibilityID", eligibilityID);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityIDParam };
            var eligibilityDeterminationResult = eligibilityDeterminationRepository.ExecuteStoredProc("usp_GetEligibilityNote", procParams);

            return eligibilityDeterminationResult;
        }

        public Response<EligibilityDeterminationModel> AddEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityDeterminationModel>(SchemaName.ECI);
            SqlParameter contactIDParam = new SqlParameter("ContactID", eligibilityDetermination.ContactID);
            SqlParameter eligibilityDateParam = new SqlParameter("EligibilityDate", eligibilityDetermination.EligibilityDate);
            SqlParameter eligibilityTypeIDParam = new SqlParameter("EligibilityTypeID", eligibilityDetermination.EligibilityTypeID);
            SqlParameter eligibilityDurationIDParam = new SqlParameter("EligibilityDurationID", eligibilityDetermination.EligibilityDurationID);
            SqlParameter eligibilityCategoryIDParam = new SqlParameter("EligibilityCategoryID", eligibilityDetermination.EligibilityCategoryID);
            SqlParameter eligibilityXML = new SqlParameter("EligibilityXML", GenerateMemberXML(eligibilityDetermination.Members));
            eligibilityXML.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", eligibilityDetermination.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIDParam, eligibilityDateParam, eligibilityTypeIDParam, eligibilityDurationIDParam, eligibilityCategoryIDParam, eligibilityXML, modifiedOnParam };            
            var eligibilityDeterminationResult = _unitOfWork.EnsureInTransaction<Response<EligibilityDeterminationModel>>(eligibilityDeterminationRepository.ExecuteNQStoredProc, "usp_AddEligibility", procParams, idResult: true, forceRollback: eligibilityDetermination.ForceRollback.GetValueOrDefault(false));

            return eligibilityDeterminationResult;
        }

        public Response<EligibilityDeterminationModel> UpdateEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityDeterminationModel>(SchemaName.ECI);
            SqlParameter eligibilityIDParam = new SqlParameter("EligibilityID", eligibilityDetermination.EligibilityID);
            SqlParameter eligibilityDateParam = new SqlParameter("EligibilityDate", eligibilityDetermination.EligibilityDate);
            SqlParameter eligibilityTypeIDParam = new SqlParameter("EligibilityTypeID", eligibilityDetermination.EligibilityTypeID);
            SqlParameter eligibilityDurationIDParam = new SqlParameter("EligibilityDurationID", eligibilityDetermination.EligibilityDurationID);
            SqlParameter eligibilityCategoryIDParam = new SqlParameter("EligibilityCategoryID", eligibilityDetermination.EligibilityCategoryID);
            SqlParameter eligibilityXML = new SqlParameter("EligibilityXML", GenerateMemberXML(eligibilityDetermination.Members));
            eligibilityXML.DbType = DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", eligibilityDetermination.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityIDParam, eligibilityDateParam, eligibilityTypeIDParam, eligibilityDurationIDParam, eligibilityCategoryIDParam, eligibilityXML, modifiedOnParam };
            var eligibilityDeterminationResult = _unitOfWork.EnsureInTransaction<Response<EligibilityDeterminationModel>>(eligibilityDeterminationRepository.ExecuteNQStoredProc, "usp_UpdateEligibility", procParams, forceRollback: eligibilityDetermination.ForceRollback.GetValueOrDefault(false));

            return eligibilityDeterminationResult;
        }

        public Response<EligibilityDeterminationModel> DeactivateEligibility(long eligibilityID, DateTime modifiedOn)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityDeterminationModel>(SchemaName.ECI);
            SqlParameter eligibilityIDParam = new SqlParameter("EligibilityID", eligibilityID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityIDParam, modifiedOnParam };
            var eligibilityDeterminationResult = eligibilityDeterminationRepository.ExecuteNQStoredProc("usp_DeleteEligibility", procParams);

            return eligibilityDeterminationResult;
        }

        public Response<EligibilityDeterminationModel> SaveEligibilityNote(EligibilityDeterminationModel eligibilityDetermination)
        {
            var eligibilityDeterminationRepository = _unitOfWork.GetRepository<EligibilityDeterminationModel>(SchemaName.ECI);
            SqlParameter eligibilityIDParam = new SqlParameter("EligibilityID", eligibilityDetermination.EligibilityID);
            SqlParameter eligibilityNotesParam = new SqlParameter("Notes", eligibilityDetermination.Notes);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", eligibilityDetermination.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityIDParam, eligibilityNotesParam, modifiedOnParam };
            var eligibilityDeterminationResult = _unitOfWork.EnsureInTransaction<Response<EligibilityDeterminationModel>>(eligibilityDeterminationRepository.ExecuteNQStoredProc, "usp_SaveEligibilityNote", procParams, forceRollback: eligibilityDetermination.ForceRollback.GetValueOrDefault(false));

            return eligibilityDeterminationResult;
        }

        #endregion

        #region Private Methods

        private string GenerateMemberXML(List<int> members)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("EligibilityDetails");

                    if (members != null)
                    {
                        foreach (int userID in members)
                        {
                            XWriter.WriteElementString("UserID", userID.ToString());
                        }
                    }

                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        #endregion
    }
}
