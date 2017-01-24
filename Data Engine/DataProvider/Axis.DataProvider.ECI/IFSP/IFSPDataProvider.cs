using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataProvider.ECI.IFSP
{
    public class IFSPDataProvider : IIFSPDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public IFSPDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        public Response<IFSPDetailModel> GetIFSP(long ifspID)
        {
            var ifspRepository = _unitOfWork.GetRepository<IFSPDetailModel>(SchemaName.ECI);
            SqlParameter ifspIDParam = new SqlParameter("ifspID", ifspID);
            List<SqlParameter> procParams = new List<SqlParameter>() { ifspIDParam };
            var result = ifspRepository.ExecuteStoredProc("usp_GetIFSP", procParams);
            return result;
        }
        
        /// <summary>
        /// Gets the list of IFSP
        /// </summary>
        /// <returns></returns>
        public Response<IFSPDetailModel> GetIFSPList(long contactId)
        {
            var ifspRepository = _unitOfWork.GetRepository<IFSPDetailModel>(SchemaName.ECI);
            var procsParameters = new List<SqlParameter> {new SqlParameter("ContactID", contactId)};
            return ifspRepository.ExecuteStoredProc("usp_GetIFSPList", procsParameters);
        }

        /// <summary>
        /// Adds IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        public Response<IFSPDetailModel> AddIFSP(IFSPDetailModel ifspDetail)
        {
            var ifspRepository = _unitOfWork.GetRepository<IFSPDetailModel>(SchemaName.ECI);
            var procParams = BuildIfspAddUpdSpParams(ifspDetail);
            return _unitOfWork.EnsureInTransaction(ifspRepository.ExecuteNQStoredProc, "usp_AddIFSP", procParams, idResult: true,
                            forceRollback: ifspDetail.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Updates IFSP
        /// </summary>
        /// <param name="ifspDetail"></param>
        /// <returns></returns>
        public Response<IFSPDetailModel> UpdateIFSP(IFSPDetailModel ifspDetail)
        {
            var ifspRepository = _unitOfWork.GetRepository<IFSPDetailModel>(SchemaName.ECI);
            var procParams = BuildIfspAddUpdSpParams(ifspDetail);
            return _unitOfWork.EnsureInTransaction(ifspRepository.ExecuteNQStoredProc, "usp_UpdateIFSP", procParams,
                            forceRollback: ifspDetail.ForceRollback.GetValueOrDefault(false));
        }

        public Response<bool> RemoveIFSP(long ifspID, DateTime modifiedOn)
        {
            var procsParameters = new List<SqlParameter> { new SqlParameter("IFSPID", ifspID), new SqlParameter("ModifiedOn", modifiedOn) };
            var ifspRepository = _unitOfWork.GetRepository<IFSPDetailModel>(SchemaName.ECI);
            var spResults = ifspRepository.ExecuteNQStoredProc("usp_DeleteIFSP", procsParameters);
            Response<bool> resultSet = new Response<bool>();
            resultSet.ResultCode = spResults.ResultCode;
            resultSet.ResultMessage = spResults.ResultMessage;
            resultSet.RowAffected = spResults.RowAffected;
            return resultSet;
        }

        /// <summary>
        /// Gets list of members for IFSP
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public Response<IFSPTeamMemberModel> GetIFSPMembers(long contactId)
        {
            var ifspRepository = _unitOfWork.GetRepository<IFSPTeamMemberModel>(SchemaName.ECI);
            var procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactId) };
            return ifspRepository.ExecuteStoredProc("usp_GetIFSPTeamDisciplines", procsParameters);
        }

        public Response<IFSPParentGuardianModel> GetIFSPParentGuardians(long contactId)
        {
            var ifspRepository = _unitOfWork.GetRepository<IFSPParentGuardianModel>(SchemaName.ECI);
            var procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactId) };
            return ifspRepository.ExecuteStoredProc("usp_GetIFSPParentGuardians", procsParameters);
        }

        #endregion

        #region Helpers
        
        private List<SqlParameter> BuildIfspAddUpdSpParams(IFSPDetailModel ifspDetail)
        {
            var spParameters = new List<SqlParameter>();
            if (ifspDetail.IFSPID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("IFSPID", ifspDetail.IFSPID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactID", ifspDetail.ContactID),
                new SqlParameter("IFSPTypeID", ifspDetail.IFSPTypeID),
                new SqlParameter("IFSPMeetingDate", ifspDetail.IFSPMeetingDate),
                new SqlParameter("IFSPFamilySignedDate", (object) ifspDetail.IFSPFamilySignedDate ?? DBNull.Value),
                new SqlParameter("MeetingDelayed", ifspDetail.MeetingDelayed),
                new SqlParameter("ReasonForDelayID", (object) ifspDetail.ReasonForDelayID ?? DBNull.Value),
                new SqlParameter("Comments", (object) ifspDetail.Comments ?? DBNull.Value),
                new SqlParameter("AssessmentID", (object) ifspDetail.AssessmentID ?? DBNull.Value),
                new SqlParameter("ResponseID", (object) ifspDetail.ResponseID ?? DBNull.Value),
                new SqlParameter("MembersXML", (object) GenerateMemberXML(ifspDetail.Members) ?? DBNull.Value),
                new SqlParameter("ParentGuardiansXML", (object) GenerateParentGuardianXML(ifspDetail.ParentGuardians) ?? DBNull.Value),
                new SqlParameter("ModifiedOn", ifspDetail.ModifiedOn ?? DateTime.Now)
            });

            return spParameters;
        }

        private string GenerateMemberXML(List<IFSPTeamMemberModel> members)
        {
            using (var sWriter = new StringWriter())
            {
                // ReSharper disable once ObjectCreationAsStatement
                var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
                using (var xWriter = XmlWriter.Create(sWriter, settings))
                {
                    xWriter.WriteStartElement("MemberDetails");
                    if (members != null)
                    {
                        foreach (var member in members)
                        {
                            xWriter.WriteElementString("UserID", member.UserID.ToString());
                        }
                    }

                    xWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        private string GenerateParentGuardianXML(List<IFSPParentGuardianModel> parentGuardians)
        {
            using (var sWriter = new StringWriter())
            {
                // ReSharper disable once ObjectCreationAsStatement
                var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
                using (var xWriter = XmlWriter.Create(sWriter, settings))
                {
                    xWriter.WriteStartElement("ParentGuardianDetails");
                    if (parentGuardians != null)
                    {
                        foreach (var parentGuardian in parentGuardians)
                        {
                            xWriter.WriteElementString("ContactID", parentGuardian.ContactID.ToString());
                        }
                    }

                    xWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }
        
        #endregion
    }
}
