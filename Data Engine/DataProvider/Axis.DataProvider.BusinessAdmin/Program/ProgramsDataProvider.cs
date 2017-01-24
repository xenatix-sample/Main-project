using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.BusinessAdmin.OrganizationStructure;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataProvider.BusinessAdmin.Program
{
    /// <summary>
    ///
    /// </summary>
    public class ProgramsDataProvider : IProgramsDataProvider
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
        /// Initializes a new instance of the <see cref="ProgramsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ProgramsDataProvider(IUnitOfWork unitOfWork, IOrganizationStructureDataProvider organizationStructureDataProvider,
            IOrganizationIdentifiersDataProvider organizationIdentifiersDataProvider)
        {
            _unitOfWork = unitOfWork;
            _organizationStructureDataProvider = organizationStructureDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the programs.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetPrograms()
        {
            return _organizationStructureDataProvider.GetOrganizationStructures(OrganizationType.Program.ToString());
        }

        /// <summary>
        /// Gets the program by identifier.
        /// </summary>
        /// <param name="programID">The program identifier.</param>
        /// <returns></returns>
        public Response<ProgramDetailsModel> GetProgramByID(long programID)
        {
            var programResponse = new Response<ProgramDetailsModel>()
            {
                DataItems = new List<ProgramDetailsModel>(),
                ResultCode = 0
            };

            var programDetails = new ProgramDetailsModel();

            var program = _organizationStructureDataProvider.GetOrganizationStructureByID(programID);
            if (program.ResultCode != 0)
            {
                programResponse.ResultCode = program.ResultCode;
                programResponse.ResultMessage = program.ResultMessage;
                return programResponse;
            }
            else
            {
                programDetails.Program = program.DataItems.FirstOrDefault();
            }

            var programHierarchy = _organizationStructureDataProvider.GetOrganizationHierarchyByID(programID, OrganizationType.Program.ToString());
            if (programHierarchy.ResultCode != 0)
            {
                programResponse.ResultCode = programHierarchy.ResultCode;
                programResponse.ResultMessage = programHierarchy.ResultMessage;
                return programResponse;
            }
            else
            {
                programDetails.ProgramHierarchies = programHierarchy.DataItems;
            }

            var divisionHierarchy = _organizationStructureDataProvider.GetOrganizationHierarchyByID(programID, OrganizationType.ProgramUnit.ToString());
            if (divisionHierarchy.ResultCode != 0)
            {
                programResponse.ResultCode = divisionHierarchy.ResultCode;
                programResponse.ResultMessage = divisionHierarchy.ResultMessage;
                return programResponse;
            }
            else
            {
                programDetails.DivisionHierarchies = divisionHierarchy.DataItems;
            }

            programResponse.DataItems.Add(programDetails);
            return programResponse;
        }

        /// <summary>
        /// Save the program.
        /// </summary>
        /// <param name="program">The program.</param>
        /// <returns></returns>
        public Response<ProgramDetailsModel> SaveProgram(ProgramDetailsModel program)
        {
            var programResponse = new Response<ProgramDetailsModel>();
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                var programResult = new Response<OrganizationDetailsModel>();
                if (program.Program.DetailID > 0)
                {
                    programResult = _organizationStructureDataProvider.UpdateOrganizationStructure(program.Program);
                }
                else
                {
                    program.Program.DataKey = OrganizationType.Program.ToString();
                    programResult = _organizationStructureDataProvider.AddOrganizationStructure(program.Program);
                    program.Program.DetailID = programResult.ID;
                }

                program.ProgramHierarchies.ForEach(item =>
                {
                    item.ProgramID = program.Program.DetailID;
                });

                // if program is failed to save
                if (programResult.ResultCode != 0)
                {
                    programResponse.ResultCode = programResult.ResultCode;
                    programResponse.ResultMessage = programResult.ResultMessage;
                    return programResponse;
                }

                var programHierarchyResult = _organizationStructureDataProvider.SaveOrganizationHierarchy(program.ProgramHierarchies, OrganizationType.Program.ToString());
                // if program hierarchy is failed to save
                if (programHierarchyResult.ResultCode != 0)
                {
                    programResponse.ResultCode = programHierarchyResult.ResultCode;
                    programResponse.ResultMessage = programHierarchyResult.ResultMessage;
                    return programResponse;
                }
                
                if (!program.ForceRollback.GetValueOrDefault(false))
                    _unitOfWork.TransactionScopeComplete(transactionScope);
            }

            return programResponse;
        }

        #endregion Public Methods
    }
}