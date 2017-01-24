using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.ClientAudit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Axis.Security;
using Axis.Model.Registration;

namespace Axis.DataProvider.Common.ClientAudit
{
    public class ClientAuditDataProvider : IClientAuditDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ClientAuditDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion initializations

        #region exposed functionality
        /// <summary>
        /// Adds the client Audit log.
        /// </summary>
        /// <param name="clientAudit">The client Audit model.</param>
        /// <returns></returns>
        public Response<ClientAuditModel> AddClientAudit(ClientAuditModel clientAudit)
        {
            SqlParameter audiSourceParam = new SqlParameter("AuditSource", "Client");
            SqlParameter xmlParam = new SqlParameter("ReadDetails", GenerateXml(clientAudit));
            xmlParam.DbType = System.Data.DbType.Xml;
            SqlParameter createdByParam = new SqlParameter("CreatedBy", (object)clientAudit.CreatedBy ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { audiSourceParam, xmlParam };

            var repository = _unitOfWork.GetRepository<ClientAuditModel>(SchemaName.Core);
            return _unitOfWork.EnsureInTransaction(
                    repository.ExecuteNQStoredProc,
                    "usp_AddReadAuditLog",
                    procParams,
                    forceRollback: clientAudit.ForceRollback.GetValueOrDefault(false),
                    idResult: false
                );
        }

        public Response<ScreenAuditModel> AddScreenAudit(ScreenAuditModel screenAudit)
        {
            SqlParameter transactionLogIDParam = new SqlParameter("TransactionLogID", (object)screenAudit.TransactionLogID ?? DBNull.Value);
            SqlParameter userIDParam = new SqlParameter("UserID", AuthContext.Auth.User.UserID);            
            SqlParameter contactIDParam = new SqlParameter("ContactID", (object)screenAudit.ContactID ?? DBNull.Value);
            SqlParameter dataKeyParam = new SqlParameter("DataKey", (object)screenAudit.DataKey ?? DBNull.Value);
            SqlParameter actionTypeIDParam = new SqlParameter("ActionTypeID", (object)screenAudit.ActionTypeID ?? DBNull.Value);
            SqlParameter isCareMemberParam = new SqlParameter("IsCareMember", (object)screenAudit.IsCareMember ?? DBNull.Value);
            SqlParameter isBreaktheGlassEnabledParam = new SqlParameter("IsBreaktheGlassEnabled", (object)screenAudit.IsBreaktheGlassEnabled ?? DBNull.Value);
            SqlParameter searchTextParam = new SqlParameter("SearchText", (object)screenAudit.SearchText ?? DBNull.Value);
            SqlParameter createdOnParam = new SqlParameter("CreatedOn", DateTime.Now);


            List<SqlParameter> procParams = new List<SqlParameter>()
            { transactionLogIDParam, userIDParam, contactIDParam, dataKeyParam,actionTypeIDParam,isCareMemberParam,isBreaktheGlassEnabledParam,searchTextParam,createdOnParam};

            var repository = _unitOfWork.GetRepository<ScreenAuditModel>(SchemaName.Auditing);
            return _unitOfWork.EnsureInTransaction(
                    repository.ExecuteNQStoredProc,
                    "usp_AddPageLevelAuditLog",
                    procParams,
                    forceRollback: screenAudit.ForceRollback.GetValueOrDefault(false),
                    idResult: false
                );
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns></returns>
        public string GetUniqueId()
        {
            var repository = _unitOfWork.GetRepository<string>(SchemaName.Core);
            var procParams = new List<SqlParameter> { new SqlParameter("ModifiedBy", AuthContext.Auth.User.UserID) };
            var result = repository.ExecuteStoredProc("usp_GenerateTransactionLogID", procParams);
            if (result != null && result.DataItems != null && result.DataItems.Count > 0)
                return Convert.ToString(result.DataItems[0]);
            else
                return "0";
        }

        /// <summary>
        /// Gets the history details.
        /// </summary>
        /// <param name="screenId">The screen identifier.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        public string GetHistoryDetails(long screenId, long primaryKey)
        {
            var repository = _unitOfWork.GetRepository<string>();
            var procParams = new List<SqlParameter> { new SqlParameter("ScreenId", screenId),
                                                        new SqlParameter("PrimaryKey", primaryKey)};
            var result = repository.ExecuteStoredProc("usp_GetHistoryDetails", procParams);
            if (result != null && result.DataItems != null && result.DataItems.Count > 0)
                return Convert.ToString(result.DataItems[0][0]);
            else
                return "";
        }

        /// <summary>
        /// Gets the demography history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<DemographyHistoryModel> GetDemographyHistory(long contactId)
        {
            var repository = _unitOfWork.GetRepository<DemographyHistoryModel>(SchemaName.Auditing);
            var procParams = new List<SqlParameter> { new SqlParameter("contactId", contactId) };
            return repository.ExecuteStoredProc("usp_GetContactDemographicChangeLog", procParams);
        }

        /// <summary>
        /// Gets the alias history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AliasHistoryModel> GetAliasHistory(long contactId)
        {
            var repository = _unitOfWork.GetRepository<AliasHistoryModel>(SchemaName.Auditing);
            var procParams = new List<SqlParameter> { new SqlParameter("contactId", contactId) };
            return repository.ExecuteStoredProc("usp_GetContactAliasChangeLog", procParams);
        }

        /// <summary>
        /// Gets the identifier history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<IdHistoryModel> GetIdHistory(long contactId)
        {
            var repository = _unitOfWork.GetRepository<IdHistoryModel>(SchemaName.Auditing);
            var procParams = new List<SqlParameter> { new SqlParameter("contactId", contactId) };
            return repository.ExecuteStoredProc("usp_GetContactClientIdentifierChangeLog", procParams);
        }

        #endregion exposed functionality

        #region Helpers
        private string GenerateXml(ClientAuditModel clientAudit)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("ClientReadAuditData");
                    XWriter.WriteElementString("UserID", clientAudit.CreatedBy.ToString());
                    XWriter.WriteElementString("ReadDate", clientAudit.CreatedOn.ToString());
                    XWriter.WriteStartElement("Input");
                    XWriter.WriteStartElement("Parameter");
                    XWriter.WriteAttributeString("Name", clientAudit.AuditKey.ToString());
                    XWriter.WriteAttributeString("Value", clientAudit.AuditValue.ToString());
                    XWriter.WriteEndElement();
                    XWriter.WriteEndElement();
                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }
        #endregion Helpers

    }
}
