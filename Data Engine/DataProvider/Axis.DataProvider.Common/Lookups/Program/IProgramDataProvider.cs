﻿using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IProgramDataProvider
    {
        Response<ProgramModel> GetProgram();
    }
}
