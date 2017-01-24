using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public interface IUserIdentifierTypeDataProvider
    {
        Response<UserIdentifierTypeModel> GetUserIdentifierType();
    }
}
