using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IAllergyLookupDataProvider
    {
        Response<AllergyModel> GetAllergies();
        Response<AllergySymptomModel> GetAllergySymptoms();
        Response<AllergyTypeModel> GetAllergyTypes();
        Response<AllergySeverityModel> GetAllergySeverities();
    }
}
