using System.Data;
using Axis.Model.ServiceConfiguration;
using Axis.Model.Common;
using Axis.Model.ServiceBatch;

namespace Axis.DataProvider.SynchronizationService
{
    public interface ISynchronizationServiceDataProvider
    {
        int CreateBatch();
        Response<ServiceConfigurationModel> GetServiceConfiguration(int configID);
        Response<ServiceConfigurationModel> GetServiceConfigurations();
        Response<ServiceBatchModel> AddBatch(ServiceBatchModel batch);
        Response<ServiceBatchModel> UpdateBatch(ServiceBatchModel batch);
        Response<ServiceBatchModel> GetLastBatch(int configID);
        void BulkInsert(DataTable dt, string destTableName, bool _debugMode, bool _debugLogFileMode);
    }
}
