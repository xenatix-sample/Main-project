using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IBPMethodDataProvider
    {
        Response<BPMethodModel> GetBPMethods();
    }
}
