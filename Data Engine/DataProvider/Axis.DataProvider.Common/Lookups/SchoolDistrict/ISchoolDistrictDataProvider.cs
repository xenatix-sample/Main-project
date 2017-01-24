using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// School district data provider
    /// </summary>
    public interface ISchoolDistrictDataProvider
    {
        /// <summary>
        /// Gets the school districts.
        /// </summary>
        /// <returns></returns>
        Response<SchoolDistrictModel> GetSchoolDistricts();
    }
}