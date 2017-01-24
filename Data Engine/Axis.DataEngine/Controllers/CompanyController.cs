using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.Company;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class CompanyController : BaseApiController
    {
        #region Class Variables

        private readonly ICompanyDataProvider _companyDataProvider = null;

        #endregion Class Variables

        #region Constructors

        public CompanyController(ICompanyDataProvider companyDataProvider)
        {
            _companyDataProvider = companyDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetCompanies()
        {
            return new HttpResult<Response<OrganizationModel>>(_companyDataProvider.GetCompanies(), Request);
        }

        /// <summary>
        /// Gets the company by identifier.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompanyByID(long companyID)
        {
            return new HttpResult<Response<CompanyDetailsModel>>(_companyDataProvider.GetCompanyByID(companyID), Request);
        }

        /// <summary>
        /// Saves the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveCompany(CompanyDetailsModel company)
        {
            return new HttpResult<Response<CompanyDetailsModel>>(_companyDataProvider.SaveCompany(company), Request);
        }

        #endregion Public Methods
    }
}