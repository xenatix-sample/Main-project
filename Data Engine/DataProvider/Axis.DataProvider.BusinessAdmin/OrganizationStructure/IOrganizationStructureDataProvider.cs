using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.DataProvider.BusinessAdmin.OrganizationStructure
{
    public interface IOrganizationStructureDataProvider
    {
        #region Organization Details

        /// <summary>
        /// Gets the organization structures.
        /// </summary>
        /// <param name="dataKey">The data key.</param>
        /// <returns></returns>
        Response<OrganizationModel> GetOrganizationStructures(string dataKey);

        /// <summary>
        /// Gets the organization structure by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        Response<OrganizationDetailsModel> GetOrganizationStructureByID(long detailID);

        Response<ServiceOrganizationModel> GetServicesOrganizationModuleComponentDetailsByDetailID(long detailID);
        
        /// <summary>
        /// Adds the organization structure.
        /// </summary>
        /// <param name="organizationDetails">The organization details.</param>
        /// <returns></returns>
        Response<OrganizationDetailsModel> AddOrganizationStructure(OrganizationDetailsModel organizationDetails);

        /// <summary>
        /// Updates the organization structure.
        /// </summary>
        /// <param name="organizationDetails">The organization details.</param>
        /// <returns></returns>
        Response<OrganizationDetailsModel> UpdateOrganizationStructure(OrganizationDetailsModel organizationDetails);

        #endregion Organization Details

        #region Organization Address

        /// <summary>
        /// Gets the organization address by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        Response<OrganizationAddressModel> GetOrganizationAddressByID(long detailID);

        /// <summary>
        /// Saves the organization address.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        Response<OrganizationAddressModel> SaveOrganizationAddress(List<OrganizationAddressModel> addresses);

        #endregion Organization Address

        #region Organization Hierarchy Assignment

        /// <summary>
        /// Gets the organization hierarchy by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        Response<OrganizationHierarchyModel> GetOrganizationHierarchyByID(long detailID, string dataKey);

        /// <summary>
        /// Gets the service organization details by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        Response<ServiceOrganizationModel> GetServiceOrganizationDetailsByID(long detailID);

        /// <summary>
        /// Gets the organization details module component by detail identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        Response<OrganizationDetailsModuleComponentModel> GetOrganizationDetailsModuleComponentByDetailID(long detailID);

        /// <summary>
        /// Saves the organization hierarchy.
        /// </summary>
        /// <param name="organizationHierarchies">The organization hierarchies.</param>
        /// <returns></returns>
        Response<OrganizationHierarchyModel> SaveOrganizationHierarchy(List<OrganizationHierarchyModel> organizationHierarchies, string dataKey);

        /// <summary>
        /// Saves the service organization.
        /// </summary>
        /// <param name="serviceOrganizations">The service organizations.</param>
        /// <returns></returns>
        Response<ServiceOrganizationModel> SaveServiceOrganization(List<ServiceOrganizationModel> serviceOrganizations);

        /// <summary>
        /// Saves the organization details module component.
        /// </summary>
        /// <param name="organizationDetailsModuleComponent">The organization details module component.</param>
        /// <returns></returns>
        Response<OrganizationDetailsModuleComponentModel> SaveOrganizationDetailsModuleComponent(List<OrganizationDetailsModuleComponentModel> organizationDetailsModuleComponent);

        #endregion Organization Hierarchy Assignment
    }
}