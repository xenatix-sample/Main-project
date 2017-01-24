using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IPresentingProblemTypeDataProvider
    {
        Response<PresentingProblemTypeModel> GetPresentingProblemTypes();
    }
}
