using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IProgramClientIdentifierDataProvider
    {
        Response<ProgramClientIdentifierModel> GetProgramClientIdentifiers();
    }
}
