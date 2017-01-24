using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Company;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class CompanyController : BaseApiController
    {
        #region Class Variables

        private readonly ICompanyRepository _companyRepository;

        #endregion Class Variables

        #region Constructors

        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetCompanies()
        {
            return _companyRepository.GetCompanies();
        }

        /// <summary>
        /// Gets the company by identifier.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        public Response<CompanyDetailsModel> GetCompanyByID(long companyID)
        {
            return _companyRepository.GetCompanyByID(companyID);
        }

        /// <summary>
        /// Saves the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        public Response<CompanyDetailsModel> SaveCompany(CompanyDetailsModel company)
        {
            return _companyRepository.SaveCompany(company);
        }

        #endregion Constructors
    }
}