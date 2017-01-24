using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.RuleEngine.BusinessAdmin.HealthRecords
{
    public interface IHealthRecordsRuleEngine
    {
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        Response<HealthRecordsModel> GetHealthRecords();
    }
}
