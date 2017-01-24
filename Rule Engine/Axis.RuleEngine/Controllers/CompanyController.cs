using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.Company;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class CompanyController : BaseApiController
    {
        #region Class Variables

        private readonly ICompanyRuleEngine _companyRuleEngine = null;

        #endregion Class Variables

        #region Constructors

        public CompanyController(ICompanyRuleEngine companyRuleEngine)
        {
            _companyRuleEngine = companyRuleEngine;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanies()
        {
            return new HttpResult<Response<OrganizationModel>>(_companyRuleEngine.GetCompanies(), Request);
        }

        /// <summary>
        /// Gets the company by identifier.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanyByID(long companyID)
        {
            return new HttpResult<Response<CompanyDetailsModel>>(_companyRuleEngine.GetCompanyByID(companyID), Request);
        }

        /// <summary>
        /// Saves the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveCompany(CompanyDetailsModel company)
        {
            return new HttpResult<Response<CompanyDetailsModel>>(_companyRuleEngine.SaveCompany(company), Request);
        }

        #endregion Public Methods
    }
}