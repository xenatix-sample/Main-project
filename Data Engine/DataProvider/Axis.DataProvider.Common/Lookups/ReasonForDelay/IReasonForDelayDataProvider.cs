using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IReasonForDelayDataProvider
    {
        /// <summary>
        /// Gets the Reason For Delay
        /// </summary>
        /// <returns></returns>
        Response<ReasonForDelayModel> GetReasonForDelay(); 
    }
}