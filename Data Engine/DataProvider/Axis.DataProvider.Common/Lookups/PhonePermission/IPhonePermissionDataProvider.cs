using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IPhonePermissionDataProvider
    {
        Response<PhonePermissionModel> GetPhonePermissions();
    }
}