using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IRecipientCodeDataProvider
    {
        /// <summary>
        /// Gets the Recipient Codes.
        /// </summary>
        /// <returns></returns>
        Response<RecipientCodeModel> GetRecipientCodes();

        /// <summary>
        /// Gets the Recipient Code Modules.
        /// </summary>
        /// <returns></returns>
        Response<RecipientCodeModel> GetRecipientCodeModuleComponents();
    }
}
