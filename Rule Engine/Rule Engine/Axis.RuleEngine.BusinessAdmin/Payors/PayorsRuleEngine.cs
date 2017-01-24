
using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.ClientMerge;
using Axis.Service.BusinessAdmin.Payors;

namespace Axis.RuleEngine.BusinessAdmin.Payors
{
    public class PayorsRuleEngine : IPayorsRuleEngine
    {
        #region Class Variables        
        /// <summary>
        /// The payors service
        /// </summary>
        private readonly IPayorsService _payorsService;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorsRuleEngine"/> class.
        /// </summary>
        /// <param name="payorsService">The payors service.</param>
        public PayorsRuleEngine(IPayorsService payorsService)
        {
            _payorsService = payorsService;
        }

        #endregion

        /// <summary>
        /// Gets payors.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public Response<PayorsModel> GetPayors(string searchText="")
        {
            return _payorsService.GetPayors(searchText);

        }

       
        /// <summary>
        /// Adds the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PayorsModel> AddPayor(PayorsModel payorDetails)
        {

            return _payorsService.AddPayor(payorDetails);
        }

        /// <summary>
        /// Updates the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        public Response<PayorsModel> UpdatePayor(PayorsModel payorDetails)
        {

            return _payorsService.UpdatePayor(payorDetails);
        }

        /// <summary>
        /// Gets the payor by identifier.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorsModel> GetPayorByID(int payorID)
        {
            return _payorsService.GetPayorByID(payorID);

        }




    }
}
