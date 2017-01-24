using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IUsersDataProvider
    {
        Response<UsersModel> GetUsers();
    }
}