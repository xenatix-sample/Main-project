using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.RecordedServices;
using Axis.Model.Common;

namespace Axis.DataProvider.RecordedServices.VoidServices
{
    /// <summary>
    /// interface
    /// </summary>
    public interface IVoidServiceDataProvider
    {
        /// <summary>
        /// set service as void
        /// </summary>
        /// <param name="voidService"></param>
        /// <returns></returns>
        Response<VoidServiceModel> VoidService(VoidServiceModel voidService);


        /// <summary>
        /// set call center service as void
        /// </summary>
        /// <param name="voidService"></param>
        /// <returns></returns>
        Response<VoidServiceModel> VoidServiceCallCenter(VoidServiceModel voidService);

        /// <summary>
        /// get void service
        /// </summary>
        /// <param name="serviceRecordingID">serviceRecordingID</param>
        /// <returns></returns>
        Response<VoidServiceModel> GetVoidService(int serviceRecordingID);
    }
}
