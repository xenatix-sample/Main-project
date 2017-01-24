using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// interface for living status data provider
    /// </summary>
    public interface ILivingStatusDataProvider
    {
        Response<LivingStatusModel> GetLivingStatus();
    }
}
