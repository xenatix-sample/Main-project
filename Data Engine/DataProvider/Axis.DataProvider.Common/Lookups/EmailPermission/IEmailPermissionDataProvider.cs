using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Email permission data provider
    /// </summary>
    public interface IEmailPermissionDataProvider
    {
        /// <summary>
        /// Gets the email permissions.
        /// </summary>
        /// <returns></returns>
        Response<EmailPermissionModel> GetEmailPermissions();
    }
}