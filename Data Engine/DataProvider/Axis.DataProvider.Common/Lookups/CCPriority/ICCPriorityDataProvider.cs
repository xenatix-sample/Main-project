﻿using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICCPriorityDataProvider
    {
        Response<CCPriorityModel> GetCCPriorities();
    }
}
