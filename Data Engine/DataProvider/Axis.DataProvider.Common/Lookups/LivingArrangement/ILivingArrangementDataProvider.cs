using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ILivingArrangementDataProvider
    {
        Response<LivingArrangementModel> GetLivingArrangements();
    }
}