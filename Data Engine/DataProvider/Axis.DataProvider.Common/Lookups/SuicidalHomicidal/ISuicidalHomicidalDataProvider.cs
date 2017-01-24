using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ISuicidalHomicidalDataProvider
    {
        Response<SuicidalHomicidalModel> GetSuicidalHomicidal();
    }
}
