using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class IFSPTypeDataProvider : IIFSPTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public IFSPTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        /// <summary>
        /// Gets IFSP type list
        /// </summary>
        /// <returns></returns>
        public Response<IFSPTypeModel> GetIFSPType()
        {
            var repository = _unitOfWork.GetRepository<IFSPTypeModel>(SchemaName.ECI);
            var results = repository.ExecuteStoredProc("usp_GetIFSPType");
            return results;
        }
    }
}