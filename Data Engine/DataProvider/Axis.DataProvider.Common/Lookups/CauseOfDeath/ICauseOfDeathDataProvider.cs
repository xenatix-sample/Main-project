using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICauseOfDeathDataProvider
    {
        Response<CauseOfDeathModel> GetCauseOfDeath();
    }
}
