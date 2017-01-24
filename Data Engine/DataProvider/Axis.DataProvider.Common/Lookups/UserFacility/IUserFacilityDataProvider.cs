using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IUserFacilityDataProvider
    {
        Response<UserFacilityModel> GetUserFacility();
    }
}