﻿using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.Service.BusinessAdmin.OrganizationStructure
{
    public interface IOrganizationStructureService
    {
        /// <summary>
        /// Gets the organization structure by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        Response<OrganizationDetailsModel> GetOrganizationStructureByID(long detailID);

        /// <summary>
        /// Gets the service organization details by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        Response<ServiceOrganizationModel> GetServiceOrganizationDetailsByID(long detailID);
    }
}