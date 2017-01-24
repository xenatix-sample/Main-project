using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// interface for organization
    /// </summary>
    public interface IOrganizationsDataProvider
    {
        /// <summary>
        /// Gets the organizations.
        /// </summary>
        /// <returns></returns>
        Response<OrganizationsModel> GetOrganizations();

        /// <summary>
        /// Gets the organization details.
        /// </summary>
        /// <returns></returns>
        Response<OrganizationsModel> GetOrganizationDetails();
    }
}