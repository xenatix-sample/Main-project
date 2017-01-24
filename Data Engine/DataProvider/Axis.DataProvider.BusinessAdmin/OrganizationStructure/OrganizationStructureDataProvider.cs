using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.BusinessAdmin.OrganizationStructure
{
    public class OrganizationStructureDataProvider : IOrganizationStructureDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnitsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public OrganizationStructureDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Organization Details

        /// <summary>
        /// Gets the organization structures.
        /// </summary>
        /// <param name="dataKey">The data key.</param>
        /// <returns></returns>
        public Response<OrganizationModel> GetOrganizationStructures(string dataKey)
        {
            var organizationAddressRepository = _unitOfWork.GetRepository<OrganizationModel>(SchemaName.Core);
            var procParams = new List<SqlParameter>() { new SqlParameter("DataKey", dataKey) };
            return organizationAddressRepository.ExecuteStoredProc("usp_GetOrganizationDetailsByDataKey", procParams);
        }

        /// <summary>
        /// Gets the organization structure by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        public Response<OrganizationDetailsModel> GetOrganizationStructureByID(long detailID)
        {
            var programUnitRepository = _unitOfWork.GetRepository<OrganizationDetailsModel>(SchemaName.Core);
            var procParams = new List<SqlParameter>() { new SqlParameter("DetailID", detailID) };
            var programUnits = programUnitRepository.ExecuteStoredProc("usp_GetOrganizationDetails", procParams);
            return programUnits;
        }

        /// <summary>
        /// Adds the organization structure.
        /// </summary>
        /// <param name="organizationDetails">The organization details.</param>
        /// <returns></returns>
        public Response<OrganizationDetailsModel> AddOrganizationStructure(OrganizationDetailsModel organizationDetails)
        {
            var programUnitRepository = _unitOfWork.GetRepository<OrganizationDetailsModel>(SchemaName.Core);
            var programUnitParams = BuildOrgStructureParams(organizationDetails);
            return programUnitRepository.ExecuteNQStoredProc("usp_AddOrganizationDetails", programUnitParams, idResult: true);
        }

        /// <summary>
        /// Updates the organization structure.
        /// </summary>
        /// <param name="organizationDetails">The organization details.</param>
        /// <returns></returns>
        public Response<OrganizationDetailsModel> UpdateOrganizationStructure(OrganizationDetailsModel organizationDetails)
        {
            var programUnitRepository = _unitOfWork.GetRepository<OrganizationDetailsModel>(SchemaName.Core);
            var programUnitParams = BuildOrgStructureParams(organizationDetails);
            return programUnitRepository.ExecuteNQStoredProc("usp_UpdateOrganizationDetails", programUnitParams);
        }

        #endregion Organization Details

        #region Organization Address

        /// <summary>
        /// Gets the organization address by identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        public Response<OrganizationAddressModel> GetOrganizationAddressByID(long detailID)
        {
            var organizationAddressRepository = _unitOfWork.GetRepository<OrganizationAddressModel>(SchemaName.Core);
            var procParams = new List<SqlParameter>() { new SqlParameter("DetailID", detailID) };
            return organizationAddressRepository.ExecuteStoredProc("usp_GetOrganizationAddresses", procParams);
        }

        /// <summary>
        /// Saves the organization address.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public Response<OrganizationAddressModel> SaveOrganizationAddress(List<OrganizationAddressModel> addresses)
        {
            if (addresses == null || addresses.Count == 0)
            {
                return new Response<OrganizationAddressModel>()
                {
                    ResultCode = 0
                };
            }

            var addressRepository = _unitOfWork.GetRepository<OrganizationAddressModel>(SchemaName.Core);
            var procParam = new List<SqlParameter>() {
                new SqlParameter("DetailID", addresses.FirstOrDefault().DetailID),
                new SqlParameter("AddressesXML", ToAddressesXML(addresses))
            };
            return addressRepository.ExecuteNQStoredProc("usp_SaveOrganizationAddresses", procParam);
        }

        #endregion Organization Address

        #region Organization Hierarchy Assignment

        public Response<OrganizationHierarchyModel> GetOrganizationHierarchyByID(long detailID, string dataKey)
        {
            var programUnitHierarchyRepository = _unitOfWork.GetRepository<OrganizationHierarchyModel>(SchemaName.Core);
            var procParams = new List<SqlParameter>() {
                new SqlParameter("DetailID", detailID),
                new SqlParameter("DataKeyFilter", dataKey)
            };

            return programUnitHierarchyRepository.ExecuteStoredProc("usp_GetOrganizationDetailsMappingByDetailID", procParams);
        }

        public Response<ServiceOrganizationModel> GetServiceOrganizationDetailsByID(long detailID)
        {
            var programUnitServicesRepository = _unitOfWork.GetRepository<ServiceOrganizationModel>(SchemaName.Reference);
            var procParams = new List<SqlParameter>() { new SqlParameter("DetailID", detailID) };
            return programUnitServicesRepository.ExecuteStoredProc("usp_GetServicesOrganizationDetailsByDetailID", procParams);
        }

        public Response<ServiceOrganizationModel> GetServicesOrganizationModuleComponentDetailsByDetailID(long detailID)
        {
            var programUnitServicesRepository = _unitOfWork.GetRepository<ServiceOrganizationModel>(SchemaName.Reference);
            var procParams = new List<SqlParameter>() { new SqlParameter("DetailID", detailID) };
            return programUnitServicesRepository.ExecuteStoredProc("usp_GetServicesOrganizationModuleComponentDetails", procParams);
        }

        /// <summary>
        /// Gets the organization details module component by detail identifier.
        /// </summary>
        /// <param name="detailID">The detail identifier.</param>
        /// <returns></returns>
        public Response<OrganizationDetailsModuleComponentModel> GetOrganizationDetailsModuleComponentByDetailID(long detailID)
        {
            var organizationDetailsModuleComponentRepository = _unitOfWork.GetRepository<OrganizationDetailsModuleComponentModel>(SchemaName.Core);
            var procParams = new List<SqlParameter>() { new SqlParameter("DetailID", detailID) };
            return organizationDetailsModuleComponentRepository.ExecuteStoredProc("usp_GetOrganizationDetailsModuleComponentByDetailID", procParams);
        }

        public Response<OrganizationHierarchyModel> SaveOrganizationHierarchy(List<OrganizationHierarchyModel> organizationHierarchies, string dataKey)
        {
            if (organizationHierarchies == null || organizationHierarchies.Count == 0)
            {
                return new Response<OrganizationHierarchyModel>()
                {
                    ResultCode = 0
                };
            }

            var programUnitHierarchyRepository = _unitOfWork.GetRepository<OrganizationHierarchyModel>(SchemaName.Core);
            var procParam = new List<SqlParameter>() {
                new SqlParameter("OrganizationDetailsMappingXML", ToOrganizationHierarchyXML(organizationHierarchies, dataKey)),
                new SqlParameter("ModifiedOn", DateTime.Now)
            };
            return programUnitHierarchyRepository.ExecuteNQStoredProc("usp_SaveOrganizationDetailsMappings", procParam);
        }

        public Response<ServiceOrganizationModel> SaveServiceOrganization(List<ServiceOrganizationModel> serviceOrganizations)
        {
            if (serviceOrganizations == null || serviceOrganizations.Count == 0)
            {
                return new Response<ServiceOrganizationModel>()
                {
                    ResultCode = 0
                };
            }

            var programUnitServicesRepository = _unitOfWork.GetRepository<ServiceOrganizationModel>(SchemaName.Reference);
            var procParam = new List<SqlParameter>() {
                new SqlParameter("OrganizationDetailsMappingXML", ToServiceOrganizationXML(serviceOrganizations)),
                new SqlParameter("ModifiedOn", DateTime.Now)
            };
            return programUnitServicesRepository.ExecuteNQStoredProc("usp_SaveServicesOrganizationDetails", procParam);
        }

        /// <summary>
        /// Saves the organization details module component.
        /// </summary>
        /// <param name="organizationDetailsModuleComponent">The organization details module component.</param>
        /// <returns></returns>
        public Response<OrganizationDetailsModuleComponentModel> SaveOrganizationDetailsModuleComponent(List<OrganizationDetailsModuleComponentModel> organizationDetailsModuleComponent)
        {
            var programUnitServicesRepository = _unitOfWork.GetRepository<OrganizationDetailsModuleComponentModel>(SchemaName.Core);
            var procParam = new List<SqlParameter>() {
                new SqlParameter("ModuleComponentXMLValue", ToOrganizationDetailsModuleComponent(organizationDetailsModuleComponent)),
                new SqlParameter("ModifiedOn", DateTime.Now)
            };
            return programUnitServicesRepository.ExecuteNQStoredProc("usp_SaveOrganizationDetailsModuleComponent", procParam);
        }

        #endregion Organization Hierarchy Assignment

        #region Private Methods

        /// <summary>
        /// Builds the org structure parameters.
        /// </summary>
        /// <param name="organizationDetails">The organization details.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildOrgStructureParams(OrganizationDetailsModel organizationDetails)
        {
            var spParameters = new List<SqlParameter>();
            if (organizationDetails.DetailID > 0)
                spParameters.Add(new SqlParameter("DetailID", organizationDetails.DetailID));

            if ((organizationDetails.DetailID ?? 0) == 0)
                spParameters.Add(new SqlParameter("DataKey", organizationDetails.DataKey));

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("Name", (object) organizationDetails.Name ?? DBNull.Value),
                new SqlParameter("Acronym", (object) organizationDetails.Acronym ?? DBNull.Value),
                new SqlParameter("Code", (object) organizationDetails.Code ?? DBNull.Value),
                new SqlParameter("EffectiveDate", (object) organizationDetails.EffectiveDate ?? DBNull.Value),
                new SqlParameter("ExpirationDate", (object) organizationDetails.ExpirationDate ?? DBNull.Value),
                new SqlParameter("IsExternal", (object) organizationDetails.IsExternal ?? DBNull.Value),
                new SqlParameter("IsActive", (object) organizationDetails.IsActive ?? true),
                new SqlParameter("ModifiedOn", (object) organizationDetails.ModifiedOn ?? DateTime.Now)
            });

            return spParameters;
        }

        /// <summary>
        /// To the addresses XML.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public string ToAddressesXML(List<OrganizationAddressModel> addresses)
        {
            var xml = new XElement("RequestXMLValue",
                from address in addresses
                where address != null
                select new XElement("Address",
                               new XElement("OrganizationAddressID", address.OrganizationAddressID),
                               new XElement("DetailID", address.DetailID),
                               new XElement("AddressID", address.AddressID),
                               new XElement("AddressTypeID", address.AddressTypeID),
                               new XElement("Line1", address.Line1),
                               new XElement("Line2", address.Line2),
                               new XElement("City", address.City),
                               new XElement("StateProvince", address.StateProvince),
                               new XElement("County", address.County),
                               new XElement("Zip", address.Zip),
                               new XElement("ComplexName", address.ComplexName),
                               new XElement("GateCode", address.GateCode),
                               new XElement("MailPermissionID", address.MailPermissionID),
                               new XElement("IsPrimary", address.IsPrimary),
                               new XElement("EffectiveDate", address.EffectiveDate ?? null),
                               new XElement("ExpirationDate", address.ExpirationDate ?? null),
                               new XElement("ModifiedOn", address.ModifiedOn ?? DateTime.Now)
                           ));

            return xml.ToString();
        }

        /// <summary>
        /// To the organization hierarchy XML.
        /// </summary>
        /// <param name="organizationHierarchies">The organization hierarchies.</param>
        /// <returns></returns>
        public string ToOrganizationHierarchyXML(List<OrganizationHierarchyModel> organizationHierarchies, string dataKey)
        {
            var xml = new XElement("OrganizationDetails",
                        new XElement("Hierarchy",
                            new XElement("Companies",
                                new XElement("Company",
                                    new XElement("Divisions",
                                        from hirarchy in organizationHierarchies
                                        select new XElement("Division",
                                            new XElement("DivisionDetailID", hirarchy.DivisionID),
                                                new XElement("Programs",
                                                    new XElement("Program",
                                                        new XElement("ProgramMappingID", hirarchy.MappingID),
                                                        new XElement("ProgramDetailID", hirarchy.ProgramID),
                                                        (hirarchy.EffectiveDate != null ? new XElement("ProgramEffectiveDate", hirarchy.EffectiveDate) : null),
                                                        (hirarchy.ExpirationDate != null ? new XElement("ProgramExpirationDate", hirarchy.ExpirationDate) : null),
                                                        (dataKey == OrganizationType.ProgramUnit.ToString() ?
                                                            new XElement("ProgramUnits",
                                                                new XElement("ProgramUnit",
                                                                    new XElement("ProgramUnitMappingID", hirarchy.MappingID),
                                                                    new XElement("ProgramUnitDetailID", hirarchy.ProgramUnitID),
                                                                    (hirarchy.EffectiveDate != null ? new XElement("ProgramUnitEffectiveDate", hirarchy.EffectiveDate) : null),
                                                                    (hirarchy.ExpirationDate != null ? new XElement("ProgramUnitExpirationDate", hirarchy.ExpirationDate) : null)
                                                                )
                                                            ) : null)
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            );

            return xml.ToString();
        }

        /// <summary>
        /// To the service organization XML.
        /// </summary>
        /// <param name="serviceOrganizations">The service organizations.</param>
        /// <returns></returns>
        public string ToServiceOrganizationXML(List<ServiceOrganizationModel> serviceOrganizations)
        {
            var xml = new XElement("OrganizationDetails",
                new XElement("Services",
                from services in serviceOrganizations
                where services != null
                select new XElement("Service",
                               new XElement("ServicesOrganizationDetailsID", services.ServicesOrganizationDetailsID),
                               new XElement("ServicesID", services.ServiceID),
                               new XElement("DetailID", services.DetailID),
                               new XElement("EffectiveDate", services.EffectiveDate ?? null),
                               new XElement("ExpirationDate", services.ExpirationDate ?? null)
                           )));

            return xml.ToString();
        }

        /// <summary>
        /// To the organization details module component.
        /// </summary>
        /// <param name="organizationDetailsModuleComponents">The organization details module components.</param>
        /// <returns></returns>
        public string ToOrganizationDetailsModuleComponent(List<OrganizationDetailsModuleComponentModel> organizationDetailsModuleComponents)
        {
            var xml = new XElement("ModuleComponentXMLValue",
                from moduleComponent in organizationDetailsModuleComponents
                where moduleComponent != null
                select new XElement("ModuleComponent",
                               new XElement("OrganizationDetailsModuleComponentID", moduleComponent.OrganizationDetailsModuleComponentID),
                               new XElement("ModuleComponentID", moduleComponent.ModuleComponentID),
                               new XElement("DetailID", moduleComponent.DetailID),
                               new XElement("EffectiveDate", moduleComponent.EffectiveDate ?? null),
                               new XElement("ExpirationDate", moduleComponent.ExpirationDate ?? null)
                           ));

            return xml.ToString();
        }

        #endregion Private Methods
    }
}