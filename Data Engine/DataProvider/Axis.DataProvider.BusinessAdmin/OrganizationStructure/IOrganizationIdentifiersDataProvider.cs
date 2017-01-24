using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.DataProvider.BusinessAdmin.OrganizationStructure
{
    public interface IOrganizationIdentifiersDataProvider
    {

        /// <summary>
        /// Gets the organization identifiers by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <param name="dataKey">The data key.</param>
        /// <returns></returns>
        Response<OrganizationIdentifiersModel> GetOrganizationIdentifiersByID(long detailID, string dataKey);

        /// <summary>
        /// Saves the organization identifiers.
        /// </summary>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns></returns>
        Response<OrganizationIdentifiersModel> SaveOrganizationIdentifiers(List<OrganizationIdentifiersModel> identifiers);
    }
}