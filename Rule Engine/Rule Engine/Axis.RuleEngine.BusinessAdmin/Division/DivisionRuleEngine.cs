using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.Division;

namespace Axis.RuleEngine.BusinessAdmin.Division
{
    public class DivisionRuleEngine : IDivisionRuleEngine
    {
        #region Class Variables

        
        private readonly IDivisionService _divisionService;

        #endregion Class Variables

        #region Constructors

        public DivisionRuleEngine(IDivisionService divisionService)
        {
            _divisionService = divisionService;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the divisions.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetDivisions()
        {
            return _divisionService.GetDivisions();
        }

        /// <summary>
        /// Gets the division by identifier.
        /// </summary>
        /// <param name="divisionID">The division identifier.</param>
        /// <returns></returns>
        public Response<DivisionDetailsModel> GetDivisionByID(long divisionID)
        {
            return _divisionService.GetDivisionByID(divisionID);
        }


        /// <summary>
        /// Saves the division.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <returns></returns>
        public Response<DivisionDetailsModel> SaveDivision(DivisionDetailsModel division)
        {
            return _divisionService.SaveDivision(division);
        }

        #endregion Public Methods
    }
}