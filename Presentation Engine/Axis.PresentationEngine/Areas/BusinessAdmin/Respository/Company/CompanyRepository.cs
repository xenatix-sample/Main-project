using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Company
{
    public class CompanyRepository : ICompanyRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "Company/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyRepository"/> class.
        /// </summary>
        public CompanyRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public CompanyRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        public Response<OrganizationModel> GetCompanies()
        {
            const string apiUrl = BaseRoute + "GetCompanies";
            return communicationManager.Get<Response<OrganizationModel>>(apiUrl);
        }

        /// <summary>
        /// Gets the company by identifier.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <returns></returns>
        public Response<CompanyDetailsModel> GetCompanyByID(long companyID)
        {
            const string apiUrl = BaseRoute + "GetCompanyByID";
            var param = new NameValueCollection { { "companyID", companyID.ToString() } };
            return communicationManager.Get<Response<CompanyDetailsModel>>(param, apiUrl);
        }

        /// <summary>
        /// Saves the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        public Response<CompanyDetailsModel> SaveCompany(CompanyDetailsModel company)
        {
            const string apiUrl = BaseRoute + "SaveCompany";
            return communicationManager.Post<CompanyDetailsModel, Response<CompanyDetailsModel>>(company, apiUrl);
        }

        #endregion Public Methods
    }
}