using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IPayorTypeDataProvider
    {
        Response<PayorTypeModel> GetPayorType();
    }
}