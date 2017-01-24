using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.BusinessAdmin.OrganizationStructure;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.BusinessAdmin.ServiceDefinition
{
    public class ServiceDefinitionDataProvider : IServiceDefinitionDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        private readonly IOrganizationStructureDataProvider _organizationStructureDataProvider;
        #endregion Class Variables



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDefinitionDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceDefinitionDataProvider(IUnitOfWork unitOfWork, IOrganizationStructureDataProvider organizationStructureDataProvider)
        {
            _unitOfWork = unitOfWork;
            _organizationStructureDataProvider = organizationStructureDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public Response<ServicesModel> GetServices(string searchText)
        {
            if (searchText == null)
            {
                searchText = "";
            }
            var serviceDefinitionRepository = _unitOfWork.GetRepository<ServicesModel>(SchemaName.Reference);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("ServiceName", searchText) };
            var services = serviceDefinitionRepository.ExecuteStoredProc("usp_GetServiceConfigServices", procParams);
            return services;
        }

        /// <summary>
        /// Gets the service definition by identifier.
        /// </summary>
        /// <param name="serviceID">The service identifier.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> GetServiceDefinitionByID(int serviceID)
        {
            var serviceDefinition = GetServiceDefinition(serviceID);

            if (serviceDefinition.ResultCode != 0)
            {
                return serviceDefinition;
            }
            else
            {
                var programUnits = GetServiceOrganizationDetailsByServicesID(serviceID);
                if (programUnits.ResultCode != 0)
                {
                    serviceDefinition.ResultCode = programUnits.ResultCode;
                    serviceDefinition.ResultMessage = programUnits.ResultMessage;
                    return serviceDefinition;
                }

                if (serviceDefinition.DataItems != null && serviceDefinition.DataItems.Count > 0)
                {
                    serviceDefinition.DataItems[0].ProgramUnitHierarchies = programUnits.DataItems;
                }
            }

            return serviceDefinition;
        }
        
        /// <summary>
        /// Saves the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> SaveServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            var serviceDefinitionResponse = new Response<ServiceDefinitionModel>();
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                if (serviceDefinition.ServicesID > 0)
                {
                    serviceDefinitionResponse = UpdateServiceDefinition(serviceDefinition);
                    UpdateProgramUnitModel(serviceDefinition.ProgramUnitHierarchies,(int) serviceDefinition.ServicesID);
                }
                else
                {
                    serviceDefinitionResponse = AddServiceDefinition(serviceDefinition);
                    UpdateProgramUnitModel(serviceDefinition.ProgramUnitHierarchies,(int)serviceDefinitionResponse.ID);
                }

                // if program unit is failed to save
                if (serviceDefinitionResponse.ResultCode != 0)
                {
                    return serviceDefinitionResponse;
                }

                var programUnitHierarchyResult = _organizationStructureDataProvider.SaveServiceOrganization(serviceDefinition.ProgramUnitHierarchies);
                // if program unit hierarchy is failed to save
                if (programUnitHierarchyResult.ResultCode != 0)
                {
                    serviceDefinitionResponse.ResultCode = programUnitHierarchyResult.ResultCode;
                    serviceDefinitionResponse.ResultMessage = programUnitHierarchyResult.ResultMessage;
                    return serviceDefinitionResponse;
                }
                _unitOfWork.TransactionScopeComplete(transactionScope);

                return serviceDefinitionResponse;
            }
        }

        #endregion Public Methods

        #region Private Method

        /// <summary>
        /// Adds the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> AddServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            var serviceDefinitionRepository = _unitOfWork.GetRepository<ServiceDefinitionModel>(SchemaName.Reference);
            var programUnitParams = BuildServiceDefinitionParams(serviceDefinition);
            return serviceDefinitionRepository.ExecuteNQStoredProc("usp_AddServices", programUnitParams, idResult: true);
        }

        /// <summary>
        /// Updates the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> UpdateServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            var serviceDefinitionRepository = _unitOfWork.GetRepository<ServiceDefinitionModel>(SchemaName.Reference);
            var programUnitParams = BuildServiceDefinitionParams(serviceDefinition);
            return serviceDefinitionRepository.ExecuteNQStoredProc("usp_UpdateServices", programUnitParams);
        }

        /// <summary>
        /// Gets the service definition.
        /// </summary>
        /// <param name="serviceID">The service identifier.</param>
        /// <returns></returns>
        private Response<ServiceDefinitionModel> GetServiceDefinition(long servicesID)
        {
            var serviceRepository = _unitOfWork.GetRepository<ServiceDefinitionModel>(SchemaName.Reference);
            var procParams = new List<SqlParameter>() { new SqlParameter("ServicesID", servicesID) };
            return serviceRepository.ExecuteStoredProc("usp_GetServiceDefinitionByServicesID", procParams);
        }

        private Response<ServiceOrganizationModel> GetServiceOrganizationDetailsByServicesID(int servicesID)
        {
            var serviceRepository = _unitOfWork.GetRepository<ServiceOrganizationModel>(SchemaName.Reference);
            var procParams = new List<SqlParameter>() { new SqlParameter("ServicesID", servicesID) };
            return serviceRepository.ExecuteStoredProc("usp_GetServicesOrganizationDetailsByServicesID", procParams);
        }


        private void UpdateProgramUnitModel(List<ServiceOrganizationModel> model, int id)
        {
            if (model.Count > 0)
            {
                model.ForEach(item =>
                {
                    item.ServiceID = id;
                });
            }

        }

        /// <summary>
        /// Builds the service definition parameters.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildServiceDefinitionParams(ServiceDefinitionModel serviceDefinition)
        {
            var spParameters = new List<SqlParameter>();
            if (serviceDefinition.ServicesID > 0)
                spParameters.Add(new SqlParameter("ServiceID", serviceDefinition.ServicesID));

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("ServiceName", (object) serviceDefinition.ServiceName ?? DBNull.Value),
                new SqlParameter("ServiceCode", (object) serviceDefinition.ServiceCode ?? DBNull.Value),
                new SqlParameter("ServiceConfigServiceTypeID", (object) serviceDefinition.ServiceConfigServiceTypeID ?? DBNull.Value),

                new SqlParameter("EffectiveDate", (object) serviceDefinition.EffectiveDate ?? DBNull.Value),
                new SqlParameter("ExpirationDate", (object) serviceDefinition.ExpirationDate ?? DBNull.Value),
                new SqlParameter("ExpirationReason", (object) serviceDefinition.ExpirationReason ?? DBNull.Value),
                new SqlParameter("EncounterReportable", (object) serviceDefinition.EncounterReportable ?? DBNull.Value),
                new SqlParameter("ServiceDefinition", (object) serviceDefinition.ServiceDefinition ?? DBNull.Value),
                new SqlParameter("Notes", (object) serviceDefinition.Notes ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object) serviceDefinition.ModifiedOn ?? DateTime.Now)
            });

            return spParameters;
        }

        #endregion Private Method
    }
}