﻿using Axis.Model.Common;
using Axis.Model.Common.ClientAudit;
using Axis.Model.Registration;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Repository
{
    public interface IClientAuditRepository
    {
        /// <summary>
        /// Adds the client Audit log.
        /// </summary>
        /// <param name="clientAudit">The client Audit model.</param>
        /// <returns></returns>
        Response<ClientAuditModel> AddClientAudit(ClientAuditModel clientAudit);
        Response<ScreenAuditModel> AddScreenAudit(ScreenAuditModel screenAudit);

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns></returns>
        string GetUniqueId();

        /// <summary>
        /// Gets the history details.
        /// </summary>
        /// <param name="screenId">The screen identifier.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        string GetHistoryDetails(long screenId, long primaryKey);

        /// <summary>
        /// Gets the demography history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<DemographyHistoryModel> GetDemographyHistory(long contactId);

        /// <summary>
        /// Gets the alias history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AliasHistoryModel> GetAliasHistory(long contactId);

        /// <summary>
        /// Gets the identifier history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<IdHistoryModel> GetIdHistory(long contactId);
    }
}