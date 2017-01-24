using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IDrugLookupDataProvider
    {
        Response<DrugModel> GetDrugs();
    }
}
