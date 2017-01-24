using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Models;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Payors
{
    public interface IPayorsRepository
    {
        /// <summary>
        /// Gets Payors.
        /// </summary>
        /// <returns></returns>
        Response<PayorsModel> GetPayors(string searchText);

        
        Response<PayorsModel> GetPayorByID(int payorId);

        /// <summary>
        /// Adds the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        /// 
        Response<PayorsModel> AddPayor(PayorsModel payorDetails);

        /// <summary>
        /// Updates the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        Response<PayorsModel> UpdatePayor(PayorsModel payorDetails);


    }
}
