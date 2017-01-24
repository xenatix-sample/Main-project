
using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Data.Repository;
using System.Data.SqlClient;
using System.Collections.Generic;
using Axis.Data.Repository.Schema;

namespace Axis.DataProvider.BusinessAdmin.ClientMerge
{
    public class ClientMergeDataProvider : IClientMergeDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class 

        #region Constructors

        public ClientMergeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods
        public Response<ClientMergeCountModel> GetClientMergeCounts()
        {
            var clientMergeRepository = _unitOfWork.GetRepository<ClientMergeCountModel>(SchemaName.Core);
            var result = clientMergeRepository.ExecuteStoredProc("usp_GetClientMergeCounts");

            return result;
        }

        public Response<PotentialMatchesModel> GetPotentialMatches()
        {
            var clientMergeRepository = _unitOfWork.GetRepository<PotentialMatchesModel>(SchemaName.Core);
            var result = clientMergeRepository.ExecuteStoredProc("usp_GetPotentialMergeContacts");

            return result;
        }

        public Response<ClientMergeModel> MergeRecords(ClientMergeModel clientMerge)
        {
            var mergeRecordsRepository = _unitOfWork.GetRepository<ClientMergeModel>(SchemaName.Core);
            SqlParameter parentMRNParam = new SqlParameter("ParentMRN", clientMerge.ParentMRN);
            SqlParameter childMRNParam = new SqlParameter("ChildMRN", clientMerge.ChildMRN);
            SqlParameter mergeDateParam = new SqlParameter("MergeDate", DateTime.Now);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { parentMRNParam, childMRNParam, mergeDateParam, modifiedOnParam};
            var result = mergeRecordsRepository.ExecuteNQStoredProc("usp_AddClientMerge", procParams, idResult: true);

            return result;
        }


        public Response<MergeContactModel> GetMergedRecords()
        {
            var clientMergeRepository = _unitOfWork.GetRepository<MergeContactModel>(SchemaName.Core);
            var result = clientMergeRepository.ExecuteStoredProc("usp_GetMergedContacts");

            return result;
        }

        public Response<MergeContactModel> UnMergeRecords(long mergedContactsMappingID)
        {
            SqlParameter mergedContactsMappingIDParms = new SqlParameter("MergedContactsMappingID", mergedContactsMappingID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { mergedContactsMappingIDParms, modifiedOnParam };
            var clientMergeRepository = _unitOfWork.GetRepository<MergeContactModel>(SchemaName.Core);
            var result = clientMergeRepository.ExecuteNQStoredProc("usp_DeleteMergedContact", procParams);
            return result;
        }


        public Response<PotentialMatchesModel> GetRefreshPotentialMatches()
        {

            var clientMergeRepository = _unitOfWork.GetRepository<PotentialMatchesModel>(SchemaName.Core);
            List<SqlParameter> procParams = new List<SqlParameter>() { new SqlParameter("ModifiedOn", DateTime.Now) };
            var result = clientMergeRepository.ExecuteNQStoredProc("usp_AddPotentialMergeContacts", procParams);

            return result;
        }


        public Response<PotentialMergeContactsLastRunModel> GetPotentialMergeContactsLastRun()
        {
            var clientMergeRepository = _unitOfWork.GetRepository<PotentialMergeContactsLastRunModel>(SchemaName.Core);
            var result = clientMergeRepository.ExecuteStoredProc("usp_GetPotentialMergeContactsLastRun");
            return result;
        }
        #endregion
    }
}
