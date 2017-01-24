using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ISuffixDataProvider
    {
        Response<SuffixModel> GetSuffixes();
    }
}