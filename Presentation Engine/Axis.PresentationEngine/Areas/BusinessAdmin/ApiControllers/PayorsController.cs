using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Payors;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class PayorsController : BaseApiController
    {
        #region Class Variables        
        /// <summary>
        /// The payor repository
        /// </summary>
        private readonly IPayorsRepository _payorRepository;

        #endregion

        #region Constructors
        public PayorsController(IPayorsRepository payorsRepository)
        {
            _payorRepository = payorsRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets payors.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PayorsModel> GetPayors(string searchText)
        {
            return _payorRepository.GetPayors(searchText);
        }


        /// <summary>
        /// Adds the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PayorsModel> AddPayor(PayorsModel payorDetails)
        {
            return _payorRepository.AddPayor(payorDetails);
        }

        /// <summary>
        /// Updates the payor.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<PayorsModel> UpdatePayor(PayorsModel payorDetails)
        {
            return _payorRepository.UpdatePayor(payorDetails);
        }

        /// <summary>
        /// Gets the payor by identifier.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PayorsModel> GetPayorByID(int payorId)
        {
            return _payorRepository.GetPayorByID(payorId);
        }
        #endregion




    }
}