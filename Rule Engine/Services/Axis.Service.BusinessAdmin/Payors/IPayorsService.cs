
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.Service.BusinessAdmin.Payors
{
    public interface IPayorsService
    {
        /// <summary>
        /// Gets payors.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Response<PayorsModel> GetPayors(string searchText);


        /// <summary>
        /// Adds the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        Response<PayorsModel> AddPayor(PayorsModel payorDetails);


        /// <summary>
        /// Updates the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        Response<PayorsModel> UpdatePayor(PayorsModel payorDetails);

        /// <summary>
        /// Gets the payor by identifier.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        Response<PayorsModel> GetPayorByID(int payorID);

    }
}
