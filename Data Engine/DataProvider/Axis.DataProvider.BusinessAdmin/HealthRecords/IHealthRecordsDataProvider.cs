using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.DataProvider.BusinessAdmin.HealthRecords
{
    public interface IHealthRecordsDataProvider
    {
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        Response<HealthRecordsModel> GetHealthRecords();
    }
}
