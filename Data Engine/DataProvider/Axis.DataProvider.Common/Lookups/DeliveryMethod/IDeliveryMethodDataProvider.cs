using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IDeliveryMethodDataProvider
    {
        /// <summary>
        /// Gets the Delivery Methods.
        /// </summary>
        /// <returns></returns>
        Response<DeliveryMethodModel> GetDeliveryMethods();

        /// <summary>
        /// Gets the Delivery Methods.
        /// </summary>
        /// <returns></returns>
        Response<DeliveryMethodModel> GetDeliveryMethodModuleComponents();
    }
}
