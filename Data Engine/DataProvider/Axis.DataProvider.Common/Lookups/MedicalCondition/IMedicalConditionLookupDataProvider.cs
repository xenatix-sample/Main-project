using Axis.Model.Common;
using Axis.Model.Common.Lookups.MedicalCondition;

namespace Axis.DataProvider.Common
{
    public interface IMedicalConditionLookupDataProvider
    {
        Response<MedicalConditionModel> GetMedicalConditions();
    }
}
