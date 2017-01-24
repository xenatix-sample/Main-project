using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.ServiceBatch;
using Axis.Model.ServiceConfiguration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace Axis.DataProvider.SynchronizationService
{
    public class SynchronizationServiceDataProvider : ISynchronizationServiceDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger=null;

        #endregion Class Variables

        #region Constructors

        public SynchronizationServiceDataProvider(ILogger logger)
        {
            _unitOfWork = new UnitOfWork(new DbContext("XenatixDBConnection"));
            _logger = logger;
        }

        #endregion Constructors

        #region Public Methods

        public int CreateBatch()
        {
            return 1;
        }

        public Response<ServiceConfigurationModel> GetServiceConfigurations()
        {
            var repository = _unitOfWork.GetRepository<ServiceConfigurationModel>(SchemaName.Synch);

            List<SqlParameter> procParams = new List<SqlParameter>() { };
            var results = repository.ExecuteStoredProc("usp_GetServiceConfigurations", procParams);

            return results;
        }

        public Response<ServiceConfigurationModel> GetServiceConfiguration(int configID)
        {
            var repository = _unitOfWork.GetRepository<ServiceConfigurationModel>(SchemaName.Synch);
            //This may need to be modified in future
            string configName = "";
            SqlParameter configIDParam = new SqlParameter("ConfigID", configID);
            SqlParameter configNameParam = new SqlParameter("ConfigName", configName);
            List<SqlParameter> procParams = new List<SqlParameter>() { configIDParam, configNameParam };
            var results = repository.ExecuteStoredProc("usp_GetServiceConfiguration", procParams);

            return results;
        }

        public Response<ServiceConfigurationModel> GetServiceConfiguration(string configName)
        {
            var repository = _unitOfWork.GetRepository<ServiceConfigurationModel>(SchemaName.Synch);
            //This may need to be modified in future
            int configID = 0; //(object)(0)
            SqlParameter configIDParam = new SqlParameter("ConfigID", configID);
            SqlParameter configNameParam = new SqlParameter("ConfigName", configName);
            List<SqlParameter> procParams = new List<SqlParameter>() { configIDParam, configNameParam };
           
            var results = repository.ExecuteStoredProc("usp_GetServiceConfiguration", procParams);

            return results;
        }

        public Response<ServiceBatchModel> GetLastBatch(int configID)
        {
            var repository = _unitOfWork.GetRepository<ServiceBatchModel>(SchemaName.Synch);

            SqlParameter configIDParam = new SqlParameter("ConfigID", configID);
            List<SqlParameter> procParams = new List<SqlParameter>() { configIDParam };
            var results = repository.ExecuteStoredProc("usp_GetLastBatch", procParams);

            return results;
        }

        public Response<ServiceBatchModel> AddBatch(ServiceBatchModel batch)
        {
            var repository = _unitOfWork.GetRepository<ServiceBatchModel>(SchemaName.Synch);

            SqlParameter batchStatusIDParam = new SqlParameter("BatchStatusID", batch.BatchStatusID);
            SqlParameter batchTypeIDParam = new SqlParameter("BatchTypeID", batch.BatchTypeID);
            SqlParameter batchConfigIDParam = new SqlParameter("ConfigID", batch.ConfigID);
            SqlParameter batchUSNParam = new SqlParameter("usn", batch.USN);
            SqlParameter batchIsActiveParam = new SqlParameter("IsActive", batch.IsActive);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", batch.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { batchStatusIDParam, batchTypeIDParam, batchConfigIDParam, batchUSNParam, batchIsActiveParam, modifiedOnParam };
            
            return repository.ExecuteNQStoredProc("usp_AddBatch", procParams, idResult: true);
        }

        public Response<ServiceBatchModel> UpdateBatch(ServiceBatchModel batch)
        {
            var repository = _unitOfWork.GetRepository<ServiceBatchModel>(SchemaName.Synch);

            SqlParameter batchIDParam = new SqlParameter("BatchID", batch.BatchID);
            SqlParameter batchStatusIDParam = new SqlParameter("BatchStatusID", batch.BatchStatusID);
            SqlParameter batchTypeIDParam = new SqlParameter("BatchTypeID", batch.BatchTypeID);
            SqlParameter batchConfigIDParam = new SqlParameter("ConfigID", batch.ConfigID);
            SqlParameter batchUSNParam = new SqlParameter("USN", batch.USN);
            SqlParameter batchIsActiveParam = new SqlParameter("IsActive", batch.IsActive);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", batch.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { batchIDParam, batchStatusIDParam, batchTypeIDParam, batchConfigIDParam, batchUSNParam, batchIsActiveParam, modifiedOnParam };
            
            return _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_UpdateBatch", procParams, false, true);
        }

        public void BulkInsert(DataTable dt, string destTableName, bool _debugMode, bool _debugLogFileMode)
        {
            using (SqlConnection destinationConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["XenatixDBConnection"].ToString()))
            {
                destinationConnection.Open();

                // Set up the bulk copy object. 
                // NOTE: the column positions in the source data table match the column positions in the destination table so there is no need to map columns.
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    bulkCopy.DestinationTableName = destTableName;

                    try
                    {
                        if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Bulk copy writing to " + destTableName +  " Count: " + dt.Rows.Count.ToString(), EventLogEntryType.Information);
                        if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                            {
                                sw.WriteLine(String.Format("Bulk copy writing to {0}. Count: {1}"), destTableName, dt.Rows.Count.ToString());
                            }

                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        Console.WriteLine(ex.Message);
                        if (_debugMode) EventLog.WriteEntry("SynchronizationService", "Error bulk copying: " + ex.Message, EventLogEntryType.Information);
                        if (_debugLogFileMode) using (StreamWriter sw = File.AppendText("Log.txt"))
                            {
                                sw.WriteLine("Error bulk copying: " + ex.Message);
                            }
                    }
                }
            }
        }

        #endregion

    }
}
