using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.BusinessAdmin.OrganizationStructure
{
    /// <summary>
    ///
    /// </summary>
    public class OrganizationIdentifiersDataProvider : IOrganizationIdentifiersDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramUnitsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public OrganizationIdentifiersDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        public Response<OrganizationIdentifiersModel> GetOrganizationIdentifiersByID(long detailID, string dataKey)
        {
            var programUnitRepository = _unitOfWork.GetRepository<OrganizationIdentifiersModel>(SchemaName.Core);
            var procParams = new List<SqlParameter>() {
                new SqlParameter("DataKey", dataKey),
                new SqlParameter("DetailID", detailID)
            };
            var programUnits = programUnitRepository.ExecuteStoredProc("usp_GetOrganizationIdentifiersByDetailID", procParams);
            return programUnits;
        }

        public Response<OrganizationIdentifiersModel> SaveOrganizationIdentifiers(List<OrganizationIdentifiersModel> identifiers)
        {
            if (identifiers == null || identifiers.Count == 0)
            {
                return new Response<OrganizationIdentifiersModel>()
                {
                    ResultCode = 0
                };
            }

            var programUnitRepository = _unitOfWork.GetRepository<OrganizationIdentifiersModel>(SchemaName.Core);
            var procParam = new List<SqlParameter>() {
                new SqlParameter("DetailID", identifiers.FirstOrDefault().DetailID),
                new SqlParameter("OrganizationIdentifierXML", ToOrganizationIdentifiersXML(identifiers)),
                new SqlParameter("ModifiedOn", DateTime.Now)
            };
            return programUnitRepository.ExecuteNQStoredProc("usp_SaveOrganizationIdentifiers", procParam);
        }

        #endregion Public Methods

        #region Private Methods

        public string ToOrganizationIdentifiersXML(List<OrganizationIdentifiersModel> identifiers)
        {
            var xml = new XElement("OrganizationDetails",
                new XElement("Identifiers",
                from identifier in identifiers
                where identifier != null
                select new XElement("Identifier",
                               new XElement("OrganizationIdentifierID", identifier.OrganizationIdentifierID),
                               new XElement("DetailID", identifier.DetailID),
                               new XElement("OrganizationIdentifierTypeID", identifier.OrganizationIdentifierTypeID),
                               new XElement("OrganizationIdentifier", identifier.OrganizationIdentifier),
                               (identifier.EffectiveDate != null ? new XElement("EffectiveDate", identifier.EffectiveDate) : null),
                               (identifier.ExpirationDate != null ? new XElement("ExpirationDate", identifier.ExpirationDate) : null)
                           )));

            return xml.ToString();
        }

        #endregion Private Methods
    }
}