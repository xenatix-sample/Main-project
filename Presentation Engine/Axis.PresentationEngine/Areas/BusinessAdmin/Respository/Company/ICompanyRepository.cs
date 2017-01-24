using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Company
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        Response<OrganizationModel> GetCompanies();

        /// <summary>
        /// Gets the company by identifier.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        Response<CompanyDetailsModel> GetCompanyByID(long companyID);

        /// <summary>
        /// Saves the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        Response<CompanyDetailsModel> SaveCompany(CompanyDetailsModel company);
    }
}