using Axis.Model.Common;
using Axis.Model.Common.Lookups;

namespace Axis.DataProvider.Common
{
    public interface IDischargeReasonDataProvider
    {
        /// <summary>
        /// Gets the discharge reason.
        /// </summary>
        /// <returns></returns>
        Response<DischargeReasonModel> GetDischargeReason();
    }
}
