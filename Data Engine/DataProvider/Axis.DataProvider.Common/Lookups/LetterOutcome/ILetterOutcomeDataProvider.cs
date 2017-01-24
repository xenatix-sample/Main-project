using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ILetterOutcomeDataProvider
    {
        /// <summary>
        /// Gets the letter outcome.
        /// </summary>
        /// <returns>Response&lt;LetterOutcomeModel&gt;.</returns>
        Response<LetterOutcomeModel> GetLetterOutcome();
    }
}
