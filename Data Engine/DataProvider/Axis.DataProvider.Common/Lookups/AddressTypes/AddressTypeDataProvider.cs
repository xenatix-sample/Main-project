using Axis.Data.Repository;
using Axis.Model.Address;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Helpers;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AddressTypeDataProvider : IAddressTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public AddressTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<AddressTypeModel> GetAddressTypes()
        {
            var repository = _unitOfWork.GetRepository<AddressTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetAddressTypeDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
