using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.HealthRecords
{
    public interface IHealthRecordsRepository
    {
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        Response<HealthRecordsModel> GetHealthRecords();
    }
}
