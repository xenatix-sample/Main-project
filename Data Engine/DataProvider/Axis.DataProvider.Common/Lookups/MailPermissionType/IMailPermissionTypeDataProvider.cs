using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Mail permission data provider
    /// </summary>
    public interface IMailPermissionTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the mail permission.
        /// </summary>
        /// <returns></returns>
        Response<MailPermissionModel> GetMailPermissionType();
    }
}