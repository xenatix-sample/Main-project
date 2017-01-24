using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class DeliveryMethodDataProvider : IDeliveryMethodDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryMethodDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeliveryMethodDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Delivery Methods.
        /// </summary>
        /// <returns></returns>
        public Response<DeliveryMethodModel> GetDeliveryMethods()
        {
            var repository = _unitOfWork.GetRepository<DeliveryMethodModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetDeliveryMethodDetails");

            return results;
        }

        /// <summary>
        /// Gets the Delivery Methods.
        /// </summary>
        /// <returns></returns>
        public Response<DeliveryMethodModel> GetDeliveryMethodModuleComponents()
        {
            var repository = _unitOfWork.GetRepository<DeliveryMethodModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetDeliveryMethodModuleComponentDetails");

            return results;
        }

        #endregion exposed functionality
    }
}
