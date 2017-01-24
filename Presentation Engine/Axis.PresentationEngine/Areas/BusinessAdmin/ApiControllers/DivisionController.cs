using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Division;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class DivisionController : BaseApiController
    {
        #region Class Variables

        private readonly IDivisionRepository _divisionRepository;

        #endregion Class Variables

        #region Constructors

        public DivisionController(IDivisionRepository divisionRepository)
        {
            _divisionRepository = divisionRepository;
        }

        /// <summary>
        /// Gets the divisions.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetDivisions()
        {
            return _divisionRepository.GetDivisions();
        }

        /// <summary>
        /// Gets the division by identifier.
        /// </summary>
        /// <param name="divisionID">The division identifier.</param>
        /// <returns></returns>
        public Response<DivisionDetailsModel> GetDivisionByID(long divisionID)
        {
            return _divisionRepository.GetDivisionByID(divisionID);
        }

        /// <summary>
        /// Saves the division.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <returns></returns>
        public Response<DivisionDetailsModel> SaveDivision(DivisionDetailsModel division)
        {
            return _divisionRepository.SaveDivision(division);
        }

        #endregion Constructors
    }
}