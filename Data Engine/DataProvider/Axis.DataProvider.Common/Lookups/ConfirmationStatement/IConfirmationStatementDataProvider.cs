using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IConfirmationStatementDataProvider
    {
        /// <summary>
        /// Gets the confirmation statement.
        /// </summary>
        /// <returns></returns>
        Response<ConfirmationStatementModel> GetConfirmationStatement();

        /// <summary>
        /// Gets the confirmation statement group.
        /// </summary>
        /// <returns></returns>
        Response<ConfirmationStatementGroupModel> GetConfirmationStatementGroup();
    }
}