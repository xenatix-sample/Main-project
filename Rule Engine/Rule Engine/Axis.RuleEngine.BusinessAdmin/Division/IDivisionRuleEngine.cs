using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.RuleEngine.BusinessAdmin.Division
{
    public interface IDivisionRuleEngine
    {
        /// <summary>
        /// Gets the divisions.
        /// </summary>
        /// <returns></returns>
        Response<OrganizationModel> GetDivisions();

        /// <summary>
        /// Gets the division by identifier.
        /// </summary>
        /// <param name="divisionID">The division identifier.</param>
        /// <returns></returns>
        Response<DivisionDetailsModel> GetDivisionByID(long divisionID);

        /// <summary>
        /// Saves the division.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <returns></returns>
        Response<DivisionDetailsModel> SaveDivision(DivisionDetailsModel division);
    }
}