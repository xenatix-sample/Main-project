using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Common
{
    public class RelationshipTypeDataProvider : IRelationshipTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork unitOfWork;

        public RelationshipTypeDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality
        public Response<RelationshipTypeModel> GetRelationshipTypeDetails()
        {
            var repository = unitOfWork.GetRepository<RelationshipTypeModel>(SchemaName.Reference);
            SqlParameter relationshipGroupID = new SqlParameter("RelationshipGroupID", DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { relationshipGroupID };
            var results = repository.ExecuteStoredProc("usp_GetRelationshipTypeDetails", procParams);

            return results;
        }
        #endregion exposed functionality  
    }
}
