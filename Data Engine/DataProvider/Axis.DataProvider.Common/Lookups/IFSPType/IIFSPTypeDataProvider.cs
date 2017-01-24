using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IIFSPTypeDataProvider
    {
        Response<IFSPTypeModel> GetIFSPType();
    }
}