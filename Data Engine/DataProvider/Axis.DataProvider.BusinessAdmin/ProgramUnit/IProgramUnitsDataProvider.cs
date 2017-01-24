﻿using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.DataProvider.BusinessAdmin.ProgramUnit
{
    public interface IProgramUnitsDataProvider
    {
        /// <summary>
        /// Gets the program units.
        /// </summary>
        /// <returns></returns>
        Response<OrganizationModel> GetProgramUnits();

        /// <summary>
        /// Gets the program unit by identifier.
        /// </summary>
        /// <param name="programUnitID">The program unit identifier.</param>
        /// <returns></returns>
        Response<ProgramUnitDetailsModel> GetProgramUnitByID(long programUnitID);

        /// <summary>
        /// Save the program unit.
        /// </summary>
        /// <param name="programUnit">The program unit.</param>
        /// <returns></returns>
        Response<ProgramUnitDetailsModel> SaveProgramUnit(ProgramUnitDetailsModel programUnit);
    }
}