using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Clinical.SocialRelationship
{
    public class SocialRelationshipDataProvider : ISocialRelationshipDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SocialRelationshipDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the social relationships by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<SocialRelationshipModel> GetSocialRelationshipsByContact(long contactID)
        {
            var srRepository = unitOfWork.GetRepository<SocialRelationshipModel>(SchemaName.Clinical);

            SqlParameter contactIdParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };

            var srByContact = srRepository.ExecuteStoredProc("usp_GetSocialRelationshipByContactID", procParams);

            return srByContact;
        }         

        /// <summary>
        /// Adds the social relationship.
        /// </summary>
        /// <param name="model">The sr.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<SocialRelationshipModel> AddSocialRelationship(SocialRelationshipModel model)
        {
            var srParameters = BuildsrSpParams(model, false);
            var srRepository = unitOfWork.GetRepository<SocialRelationshipModel>(SchemaName.Clinical);
            return unitOfWork.EnsureInTransaction(srRepository.ExecuteNQStoredProc, "usp_AddSocialRelationship", srParameters,
                forceRollback: model.ForceRollback.GetValueOrDefault(false), idResult: true);
        }

        /// <summary>
        /// Updates the social relationship.
        /// </summary>
        /// <param name="model">The sr.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<SocialRelationshipModel> UpdateSocialRelationship(SocialRelationshipModel model)
        {
            var srParameters = BuildsrSpParams(model, true);
            var srRepository = unitOfWork.GetRepository<SocialRelationshipModel>(SchemaName.Clinical);
            return unitOfWork.EnsureInTransaction(srRepository.ExecuteNQStoredProc, "usp_UpdateSocialRelationship", srParameters,
                          forceRollback: model.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the social relationship.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> DeleteSocialRelationship(long ID, DateTime modifiedOn)
        {
            var srRepository = unitOfWork.GetRepository<SocialRelationshipModel>(SchemaName.Clinical);
            var param = new SqlParameter("SocialRelationshipID", ID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = srRepository.ExecuteNQStoredProc("usp_DeleteSocialRelationship", procParams);
            return result;
        }

        #endregion Public Methods

        #region Helpers

        /// <summary>
        /// Builds the sr sp parameters.
        /// </summary>
        /// <param name="sr">The sr.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildsrSpParams(SocialRelationshipModel sr, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("SocialRelationshipID", sr.SocialRelationshipID));

            spParameters.Add(new SqlParameter("ContactID", (object)sr.ContactID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EncounterID", (object)sr.EncounterID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReviewedNoChanges", sr.ReviewedNoChanges));
            spParameters.Add(new SqlParameter("TakenBy", (object)sr.TakenBy ?? DBNull.Value));
            spParameters.Add(new SqlParameter("TakenTime", (object)sr.TakenTime ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ChildhoodHistory", (object)sr.ChildhoodHistory ?? DBNull.Value));
            spParameters.Add(new SqlParameter("RelationshipHistory", (object)sr.RelationShipHistory ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FamilyHistory", (object)sr.FamilyHistory ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", sr.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
