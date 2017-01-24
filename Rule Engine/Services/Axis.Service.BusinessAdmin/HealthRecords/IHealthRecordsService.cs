using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.Service.BusinessAdmin.HealthRecords
{
    public interface IHealthRecordsService
    {
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        Response<HealthRecordsModel> GetHealthRecords();
    }
}
