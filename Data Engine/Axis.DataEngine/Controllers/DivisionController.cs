using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.Division;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class DivisionController : BaseApiController
    {
        #region Class Variables

        private readonly IDivisionDataProvider _divisionDataProvider = null;

        #endregion Class Variables

        #region Constructors

        public DivisionController(IDivisionDataProvider divisionDataProvider)
        {
            _divisionDataProvider = divisionDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetDivisions()
        {
            return new HttpResult<Response<OrganizationModel>>(_divisionDataProvider.GetDivisions(), Request);
        }

        /// <summary>
        /// Gets the division by identifier.
        /// </summary>
        /// <param name="divisionID">The division identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDivisionByID(long divisionID)
        {
            return new HttpResult<Response<DivisionDetailsModel>>(_divisionDataProvider.GetDivisionByID(divisionID), Request);
        }

        /// <summary>
        /// Saves the division.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveDivision(DivisionDetailsModel division)
        {
            return new HttpResult<Response<DivisionDetailsModel>>(_divisionDataProvider.SaveDivision(division), Request);
        }

        #endregion Public Methods
    }
}