using Axis.Constant;
using Axis.Data.Repository;
using Axis.DataProvider.BusinessAdmin.OrganizationStructure;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataProvider.BusinessAdmin.Division
{
    /// <summary>
    ///
    /// </summary>
    public class DivisionDataProvider : IDivisionDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        private readonly IOrganizationStructureDataProvider _organizationStructureDataProvider;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="organizationStructureDataProvider">The organization structure data provider.</param>
        public DivisionDataProvider(IUnitOfWork unitOfWork, IOrganizationStructureDataProvider organizationStructureDataProvider)
        {
            _unitOfWork = unitOfWork;
            _organizationStructureDataProvider = organizationStructureDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        public Response<OrganizationModel> GetDivisions()
        {
            return _organizationStructureDataProvider.GetOrganizationStructures(OrganizationType.Division.ToString());
        }

        public Response<DivisionDetailsModel> GetDivisionByID(long divisionID)
        {
            var divisionResponse = new Response<DivisionDetailsModel>()
            {
                DataItems = new List<DivisionDetailsModel>(),
                ResultCode = 0
            };

            var divisionDetails = new DivisionDetailsModel();

            var division = _organizationStructureDataProvider.GetOrganizationStructureByID(divisionID);
            if (division.ResultCode != 0)
            {
                divisionResponse.ResultCode = division.ResultCode;
                divisionResponse.ResultMessage = division.ResultMessage;
                return divisionResponse;
            }
            else
            {
                divisionDetails.Division = division.DataItems.FirstOrDefault();
            }

            var divisionHierarchies = _organizationStructureDataProvider.GetOrganizationHierarchyByID(divisionID, OrganizationType.Program.ToString());
            if (divisionHierarchies.ResultCode != 0)
            {
                divisionResponse.ResultCode = divisionHierarchies.ResultCode;
                divisionResponse.ResultMessage = divisionHierarchies.ResultMessage;
                return divisionResponse;
            }
            else
            {
                divisionDetails.DivisionHierarchies = divisionHierarchies.DataItems;
            }

            divisionResponse.DataItems.Add(divisionDetails);
            return divisionResponse;
        }

        public Response<DivisionDetailsModel> SaveDivision(DivisionDetailsModel division)
        {
            var divisionResponse = new Response<DivisionDetailsModel>();
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                var divisionResult = new Response<OrganizationDetailsModel>();
                if (division.Division.DetailID > 0)
                {
                    divisionResult = _organizationStructureDataProvider.UpdateOrganizationStructure(division.Division);
                }
                else
                {
                    division.Division.DataKey = OrganizationType.Division.ToString();
                    divisionResult = _organizationStructureDataProvider.AddOrganizationStructure(division.Division);
                    division.Division.DetailID = divisionResult.ID;
                }

                // if division is failed to save
                if (divisionResult.ResultCode != 0)
                {
                    divisionResponse.ResultCode = divisionResult.ResultCode;
                    divisionResponse.ResultMessage = divisionResult.ResultMessage;
                    return divisionResponse;
                }

                if (!division.ForceRollback.GetValueOrDefault(false))
                    _unitOfWork.TransactionScopeComplete(transactionScope);
            }

            return divisionResponse;
        }

        #endregion Public Methods
    }
}