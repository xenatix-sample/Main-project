using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Common
{
    public class ProgramUnitDataProvider : IProgramUnitDataProvider
    {
        readonly IUnitOfWork _unitOfWork;
        public ProgramUnitDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response<ProgramUnitModel> GetProgramUnit()
        {
            var repository = _unitOfWork.GetRepository<ProgramUnitModel>(SchemaName.Core);
            return repository.ExecuteStoredProc("usp_GetOrgDetailsByInternalServices");
        }
        public Response<ProgramUnitModel> GetWorkflowProgramUnits()
        {
            var repository = _unitOfWork.GetRepository<ProgramUnitModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetOrganizationDetailsModuleComponent");
        }
        
    }
}
