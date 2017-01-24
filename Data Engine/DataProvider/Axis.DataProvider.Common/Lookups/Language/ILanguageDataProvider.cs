using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ILanguageDataProvider
    {
        Response<LanguageModel> GetLanguages();
    }
}