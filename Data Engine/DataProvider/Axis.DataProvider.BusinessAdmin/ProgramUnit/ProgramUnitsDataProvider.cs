using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.BusinessAdmin.OrganizationStructure;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataProvider.BusinessAdmin.ProgramUnit
{
    /// <summary>
    ///
    /// </summary>
    public class ProgramUnitsDataProvider : IProgramUnitsDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        private readonly IOrganizationStructureDataProvider _organizationStructureDataProvider;
        private readonly IOrganizationIdentifiersDataProvider _organizationIdentifiersDataProvider;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnitsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ProgramUnitsDataProvider(IUnitOfWork unitOfWork, IOrganizationStructureDataProvider organizationStructureDataProvider,
            IOrganizationIdentifiersDataProvider organizationIdentifiersDataProvider)
        {
            _unitOfWork = unitOfWork;
            _organizationStructureDataProvider = organizationStructureDataProvider;
            _organizationIdentifiersDataProvider = organizationIdentifiersDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the program units.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetProgramUnits()
        {
            return _organizationStructureDataProvider.GetOrganizationStructures(OrganizationType.ProgramUnit.ToString());
        }

        /// <summary>
        /// Gets the program unit by identifier.
        /// </summary>
        /// <param name="programUnitID">The program unit identifier.</param>
        /// <returns></returns>
        public Response<ProgramUnitDetailsModel> GetProgramUnitByID(long programUnitID)
        {
            var programUnitResponse = new Response<ProgramUnitDetailsModel>()
            {
                DataItems = new List<ProgramUnitDetailsModel>(),
                ResultCode = 0
            };

            var programUnitDetails = new ProgramUnitDetailsModel();

            var programUnit = _organizationStructureDataProvider.GetOrganizationStructureByID(programUnitID);
            if (programUnit.ResultCode != 0)
            {
                programUnitResponse.ResultCode = programUnit.ResultCode;
                programUnitResponse.ResultMessage = programUnit.ResultMessage;
                return programUnitResponse;
            }
            else
            {
                programUnitDetails.ProgramUnit = programUnit.DataItems.FirstOrDefault();
            }

            var reportingUnit = _organizationIdentifiersDataProvider.GetOrganizationIdentifiersByID(programUnitID, OrganizationType.ProgramUnit.ToString());
            if (reportingUnit.ResultCode != 0)
            {
                programUnitResponse.ResultCode = reportingUnit.ResultCode;
                programUnitResponse.ResultMessage = reportingUnit.ResultMessage;
                return programUnitResponse;
            }
            else
            {
                programUnitDetails.ReportingUnit = reportingUnit.DataItems.FirstOrDefault();
            }

            var programUnitAddresses = _organizationStructureDataProvider.GetOrganizationAddressByID(programUnitID);
            if (programUnitAddresses.ResultCode != 0)
            {
                programUnitResponse.ResultCode = programUnitAddresses.ResultCode;
                programUnitResponse.ResultMessage = programUnitAddresses.ResultMessage;
                return programUnitResponse;
            }
            else
            {
                programUnitDetails.Addresses = programUnitAddresses.DataItems;
            }

            var programUnitHierarchy = _organizationStructureDataProvider.GetOrganizationHierarchyByID(programUnitID, OrganizationType.ProgramUnit.ToString());
            if (programUnitHierarchy.ResultCode != 0)
            {
                programUnitResponse.ResultCode = programUnitHierarchy.ResultCode;
                programUnitResponse.ResultMessage = programUnitHierarchy.ResultMessage;
                return programUnitResponse;
            }
            else
            {
                programUnitDetails.ProgramUnitHierarchies = programUnitHierarchy.DataItems;
            }

            var programUnitServices = _organizationStructureDataProvider.GetServiceOrganizationDetailsByID(programUnitID);
            if (programUnitServices.ResultCode != 0)
            {
                programUnitResponse.ResultCode = programUnitServices.ResultCode;
                programUnitResponse.ResultMessage = programUnitServices.ResultMessage;
                return programUnitResponse;
            }
            else
            {
                programUnitDetails.ProgramUnitServices = programUnitServices.DataItems;
            }

            var programUnitServiceWorkflows = _organizationStructureDataProvider.GetOrganizationDetailsModuleComponentByDetailID(programUnitID);
            if (programUnitServiceWorkflows.ResultCode != 0)
            {
                programUnitResponse.ResultCode = programUnitServiceWorkflows.ResultCode;
                programUnitResponse.ResultMessage = programUnitServiceWorkflows.ResultMessage;
                return programUnitResponse;
            }
            else
            {
                programUnitDetails.ProgramUnitServiceWorkflows = programUnitServiceWorkflows.DataItems;
            }

            programUnitResponse.DataItems.Add(programUnitDetails);
            return programUnitResponse;
        }

        /// <summary>
        /// Save the program unit.
        /// </summary>
        /// <param name="programUnit">The program unit.</param>
        /// <returns></returns>
        public Response<ProgramUnitDetailsModel> SaveProgramUnit(ProgramUnitDetailsModel programUnit)
        {
            var programUnitResponse = new Response<ProgramUnitDetailsModel>();
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                var programUnitResult = new Response<OrganizationDetailsModel>();
                if (programUnit.ProgramUnit.DetailID > 0)
                {
                    programUnitResult = _organizationStructureDataProvider.UpdateOrganizationStructure(programUnit.ProgramUnit);
                }
                else
                {
                    programUnit.ProgramUnit.DataKey = OrganizationType.ProgramUnit.ToString();
                    programUnitResult = _organizationStructureDataProvider.AddOrganizationStructure(programUnit.ProgramUnit);
                    programUnit.ProgramUnit.DetailID = programUnitResult.ID;
                }

                programUnit.ReportingUnit.DetailID = programUnit.ProgramUnit.DetailID;

                programUnit.Addresses.ForEach(item =>
                {
                    item.DetailID = programUnit.ProgramUnit.DetailID;
                });
                programUnit.ProgramUnitHierarchies.ForEach(item =>
                {
                    item.ProgramUnitID = programUnit.ProgramUnit.DetailID;
                });
                programUnit.ProgramUnitServices.ForEach(item =>
                {
                    item.DetailID = programUnit.ProgramUnit.DetailID;
                });
                programUnit.ProgramUnitServiceWorkflows.ForEach(item =>
                {
                    item.DetailID = programUnit.ProgramUnit.DetailID;
                });

                // if program unit is failed to save
                if (programUnitResult.ResultCode != 0)
                {
                    programUnitResponse.ResultCode = programUnitResult.ResultCode;
                    programUnitResponse.ResultMessage = programUnitResult.ResultMessage;
                    return programUnitResponse;
                }

                var reportingUnitResult = _organizationIdentifiersDataProvider.SaveOrganizationIdentifiers(new List<OrganizationIdentifiersModel>() { programUnit.ReportingUnit });
                // if reporting unit is failed to save
                if (reportingUnitResult.ResultCode != 0)
                {
                    programUnitResponse.ResultCode = reportingUnitResult.ResultCode;
                    programUnitResponse.ResultMessage = reportingUnitResult.ResultMessage;
                    return programUnitResponse;
                }

                var programUnitAddressResult = _organizationStructureDataProvider.SaveOrganizationAddress(programUnit.Addresses);
                // if program unit address is failed to save
                if (programUnitAddressResult.ResultCode != 0)
                {
                    programUnitResponse.ResultCode = programUnitAddressResult.ResultCode;
                    programUnitResponse.ResultMessage = programUnitAddressResult.ResultMessage;
                    return programUnitResponse;
                }

                var programUnitHierarchyResult = _organizationStructureDataProvider.SaveOrganizationHierarchy(programUnit.ProgramUnitHierarchies, OrganizationType.ProgramUnit.ToString());
                // if program unit hierarchy is failed to save
                if (programUnitHierarchyResult.ResultCode != 0)
                {
                    programUnitResponse.ResultCode = programUnitHierarchyResult.ResultCode;
                    programUnitResponse.ResultMessage = programUnitHierarchyResult.ResultMessage;
                    return programUnitResponse;
                }

                var programUnitServicesResult = _organizationStructureDataProvider.SaveServiceOrganization(programUnit.ProgramUnitServices);
                // if program unit services is failed to save
                if (programUnitServicesResult.ResultCode != 0)
                {
                    programUnitResponse.ResultCode = programUnitServicesResult.ResultCode;
                    programUnitResponse.ResultMessage = programUnitServicesResult.ResultMessage;
                    return programUnitResponse;
                }

                var programUnitServiceWorkflowsResult = _organizationStructureDataProvider.SaveOrganizationDetailsModuleComponent(programUnit.ProgramUnitServiceWorkflows);
                // if program unit service workflows is failed to save
                if (programUnitServiceWorkflowsResult.ResultCode != 0)
                {
                    programUnitResponse.ResultCode = programUnitServiceWorkflowsResult.ResultCode;
                    programUnitResponse.ResultMessage = programUnitServiceWorkflowsResult.ResultMessage;
                    return programUnitResponse;
                }

                if (!programUnit.ForceRollback.GetValueOrDefault(false))
                    _unitOfWork.TransactionScopeComplete(transactionScope);
            }

            return programUnitResponse;
        }

        #endregion Public Methods
    }
}