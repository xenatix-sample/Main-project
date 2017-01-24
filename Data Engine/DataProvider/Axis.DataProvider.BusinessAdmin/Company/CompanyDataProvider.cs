using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.BusinessAdmin.OrganizationStructure;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataProvider.BusinessAdmin.Company
{
    /// <summary>
    ///
    /// </summary>
    public class CompanyDataProvider : ICompanyDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        private readonly IOrganizationStructureDataProvider _organizationStructureDataProvider;
        private readonly IOrganizationIdentifiersDataProvider _organizationIdentifiersDataProvider;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnitsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CompanyDataProvider(IUnitOfWork unitOfWork, IOrganizationStructureDataProvider organizationStructureDataProvider,
            IOrganizationIdentifiersDataProvider organizationIdentifiersDataProvider)
        {
            _unitOfWork = unitOfWork;
            _organizationStructureDataProvider = organizationStructureDataProvider;
            _organizationIdentifiersDataProvider = organizationIdentifiersDataProvider;
        }

        #endregion Constructors

        #region Public Methods

       
        public Response<OrganizationModel> GetCompanies()
        {
            return _organizationStructureDataProvider.GetOrganizationStructures(OrganizationType.Company.ToString());
        }

        
        public Response<CompanyDetailsModel> GetCompanyByID(long companyID)
        {
            var companyResponse = new Response<CompanyDetailsModel>()
            {
                DataItems = new List<CompanyDetailsModel>(),
                ResultCode = 0
            };

            var companyDetails = new CompanyDetailsModel();

            var company = _organizationStructureDataProvider.GetOrganizationStructureByID(companyID);
            if (company.ResultCode != 0)
            {
                companyResponse.ResultCode = company.ResultCode;
                companyResponse.ResultMessage = company.ResultMessage;
                return companyResponse;
            }
            else
            {
                companyDetails.Company = company.DataItems.FirstOrDefault();
            }

            var organizationIdentifiers = _organizationIdentifiersDataProvider.GetOrganizationIdentifiersByID(companyID, OrganizationType.Company.ToString());
            if (organizationIdentifiers.ResultCode != 0)
            {
                companyResponse.ResultCode = organizationIdentifiers.ResultCode;
                companyResponse.ResultMessage = organizationIdentifiers.ResultMessage;
                return companyResponse;
            }
            else
            {
                companyDetails.CompanyIdentifiers = organizationIdentifiers.DataItems;
            }

            var companyAddresses = _organizationStructureDataProvider.GetOrganizationAddressByID(companyID);
            if (companyAddresses.ResultCode != 0)
            {
                companyResponse.ResultCode = companyAddresses.ResultCode;
                companyResponse.ResultMessage = companyAddresses.ResultMessage;
                return companyResponse;
            }
            else
            {
                companyDetails.Addresses = companyAddresses.DataItems;
            }

            companyResponse.DataItems.Add(companyDetails);
            return companyResponse;
        }

        public Response<CompanyDetailsModel> SaveCompany(CompanyDetailsModel company)
        {
            var companyResponse = new Response<CompanyDetailsModel>();
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                var companyResult = new Response<OrganizationDetailsModel>();
                if (company.Company.DetailID > 0)
                {
                    companyResult = _organizationStructureDataProvider.UpdateOrganizationStructure(company.Company);
                }
                else
                {
                    company.Company.DataKey = OrganizationType.Company.ToString();
                    companyResult = _organizationStructureDataProvider.AddOrganizationStructure(company.Company);
                    company.Company.DetailID = companyResult.ID;
                }

                company.Addresses.ForEach(item =>
                {
                    item.DetailID = company.Company.DetailID;
                });
                company.CompanyIdentifiers.ForEach(item =>
                {
                    item.DetailID = company.Company.DetailID;
                });

                // if company is failed to save
                if (companyResult.ResultCode != 0)
                {
                    companyResponse.ResultCode = companyResult.ResultCode;
                    companyResponse.ResultMessage = companyResult.ResultMessage;
                    return companyResponse;
                }

                var companyIdentifiersResult = _organizationIdentifiersDataProvider.SaveOrganizationIdentifiers(company.CompanyIdentifiers);
                // if organization identifier is failed to save
                if (companyIdentifiersResult.ResultCode != 0)
                {
                    companyResponse.ResultCode = companyIdentifiersResult.ResultCode;
                    companyResponse.ResultMessage = companyIdentifiersResult.ResultMessage;
                    return companyResponse;
                }

                var organizationAddressResult = _organizationStructureDataProvider.SaveOrganizationAddress(company.Addresses);
                // if program unit address is failed to save
                if (organizationAddressResult.ResultCode != 0)
                {
                    companyResponse.ResultCode = organizationAddressResult.ResultCode;
                    companyResponse.ResultMessage = organizationAddressResult.ResultMessage;
                    return companyResponse;
                }
                
                if (!company.ForceRollback.GetValueOrDefault(false))
                    _unitOfWork.TransactionScopeComplete(transactionScope);
            }

            return companyResponse;
        }

        #endregion Public Methods
    }
}