using Axis.Model.Common;
using Axis.Model.Common.Lookups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Common.Lookups.Services
{
    public interface IServicesDataProvider
    {
        /// <summary>
        /// Gets the Services.
        /// </summary>
        /// <returns></returns>
        Response<ServicesModel> GetServicesModuleComponents();
    }
}
