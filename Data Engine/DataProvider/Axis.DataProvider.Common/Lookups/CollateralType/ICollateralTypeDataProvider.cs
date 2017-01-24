using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICollateralTypeDataProvider
    {
        Response<CollateralTypeModel> GetCollateralType();
    }
}
