using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Common
{
    public interface IProgramUnitDataProvider
    {
        Response<ProgramUnitModel> GetProgramUnit();
        Response<ProgramUnitModel> GetWorkflowProgramUnits();
    }
}
