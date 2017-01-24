using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.Company;

namespace Axis.RuleEngine.BusinessAdmin.Company
{
    public class CompanyRuleEngine : ICompanyRuleEngine
    {
        #region Class Variables

        /// <summary>
        /// The program units service
        /// </summary>
        private readonly ICompanyService _companyService;

        #endregion Class Variables

        #region Constructors

        public CompanyRuleEngine(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetCompanies()
        {
            return _companyService.GetCompanies();
        }

        /// <summary>
        /// Gets the company by identifier.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        public Response<CompanyDetailsModel> GetCompanyByID(long companyID)
        {
            return _companyService.GetCompanyByID(companyID);
        }

        /// <summary>
        /// Saves the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        public Response<CompanyDetailsModel> SaveCompany(CompanyDetailsModel company)
        {
            return _companyService.SaveCompany(company);
        }

        #endregion Public Methods
    }
}