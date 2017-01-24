using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Registration.Common;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referrals.Requestor
{
    public class ReferralHeaderDataProvider : IReferralHeaderDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;
        private IContactRelationshipDataProvider _contactRelationshipDataProvider;
        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralRequestorDetailsDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralHeaderDataProvider(IUnitOfWork unitOfWork, IContactRelationshipDataProvider contactRelationshipDataProvider)
        {
            this.unitOfWork = unitOfWork;
            this._contactRelationshipDataProvider = contactRelationshipDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> GetReferralHeader(long referralHeaderID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralHeaderModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("referralHeaderID", referralHeaderID) };
            var result = referralRepository.ExecuteStoredProc("usp_GetReferralHeader", procParams);
            if (result.DataItems.Count > 0 && (result.DataItems[0].IsReferrerConvertedToCollateral ?? false))
            {
                var contactRelationshipData = _contactRelationshipDataProvider.GetContactRelationship(result.DataItems[0].ContactID, result.DataItems[0].ParentContactID, result.DataItems[0].ReferralHeaderID);

                if (contactRelationshipData.ResultCode != 0)
                {
                    result.ResultCode = contactRelationshipData.ResultCode;
                    result.ResultMessage = contactRelationshipData.ResultMessage;
                    return result;
                }
                if (contactRelationshipData.DataItems != null && contactRelationshipData.DataItems.Count > 0)
                {
                    result.DataItems[0].ContactRelationShip = contactRelationshipData.DataItems[0];
                }
            }
            return result;
        }

        /// <summary>
        /// Adds the referral header.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> AddReferralHeader(ReferralHeaderModel referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralHeaderModel>(SchemaName.Registration);
            var procParams = BuildParams(referral);
            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                var result = referralRepository.ExecuteNQStoredProc("usp_AddReferralHeader", procParams, idResult: true);
                if (result.ResultCode != 0) {
                    return result;
                }
                referral.ReferralHeaderID = result.ID;
                var convertResult = ConvertToCollateral(referral, referralRepository);
                if (convertResult != null)
                {
                    if (convertResult.ResultCode != 0)
                    {
                        return convertResult;
                    }
                }

                if (!referral.ForceRollback.GetValueOrDefault(false))
                    unitOfWork.TransactionScopeComplete(transactionScope);

                return result;
            }            
        }

        /// <summary>
        /// Updates the referral header.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> UpdateReferralHeader(ReferralHeaderModel referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralHeaderModel>(SchemaName.Registration);
            var procParams = BuildParams(referral);
            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                var result = referralRepository.ExecuteNQStoredProc("usp_UpdateReferralHeader", procParams);
                var convertResult = ConvertToCollateral(referral, referralRepository);
                if (convertResult != null)
                {
                    if (convertResult.ResultCode != 0)
                    {
                        return convertResult;
                    }
                }
                if (!referral.ForceRollback.GetValueOrDefault(false))
                    unitOfWork.TransactionScopeComplete(transactionScope);

                return result;
            }
        }

        /// <summary>
        /// Deletes the referral header.
        /// </summary>
        /// <param name="referralHeaderID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> DeleteReferralHeader(long referralHeaderID, DateTime modifiedOn)
        {
            //TODO:
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Private Methods

        private List<SqlParameter> BuildParams(ReferralHeaderModel referral)
        {
            var spParameters = new List<SqlParameter>();
            if (referral.ReferralHeaderID > 0)
                spParameters.Add(new SqlParameter("ReferralHeaderID", referral.ReferralHeaderID));

            spParameters.Add(new SqlParameter("ContactID", referral.ContactID));
            spParameters.Add(new SqlParameter("ReferralStatusID", (object)referral.ReferralStatusID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralTypeID", (object)referral.ReferralTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ResourceTypeID", (object)referral.ResourceTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralCategorySourceID", (object)referral.ReferralCategorySourceID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralOriginID", (object)referral.ReferralOriginID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OrganizationID", (object)referral.OrganizationID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralOrganizationID", (object)referral.ReferralOrganizationID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherOrganization", (object)referral.OtherOrganization ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralSourceID", (object)referral.ReferralSourceID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OtherSource", (object)referral.OtherSource ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralDate", referral.ReferralDate));
            spParameters.Add(new SqlParameter("IsLinkedToContact", (object)referral.IsLinkedToContact ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsReferrerConvertedToContact", (object)referral.IsReferrerConvertedToCollateral ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", referral.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        private Response<ReferralHeaderModel> ConvertToCollateral(ReferralHeaderModel referral, IRepository<ReferralHeaderModel> referralRepository)
        {
            if (referral.IsReferrerConvertedToCollateral ?? false)
            {
                var convertProcParams = new List<SqlParameter>(){
                        new SqlParameter("ParentContactID", (object)referral.ContactRelationShip.ParentContactID ?? DBNull.Value),
                        new SqlParameter("ContactID", (object)referral.ContactRelationShip.ContactID ?? DBNull.Value),
                        new SqlParameter("ContactTypeID", (object)referral.ContactRelationShip.ContactRelationshipTypeID ?? DBNull.Value),
                        new SqlParameter("IsPolicyHolder", (object)referral.ContactRelationShip.IsPolicyHolder ?? false),
                        new SqlParameter("RelationshipTypeID", (object)referral.ContactRelationShip.RelationshipTypeID ?? DBNull.Value),
                        new SqlParameter("OtherRelationship", (object)referral.ContactRelationShip.OtherRelationship ?? DBNull.Value),
                        new SqlParameter("LivingWithClientStatus", (object)referral.ContactRelationShip.LivingWithClientStatus ?? DBNull.Value),
                        new SqlParameter("ReferralHeaderID", (object)referral.ReferralHeaderID ?? DBNull.Value),
                        new SqlParameter("ModifiedOn", (object)referral.ContactRelationShip.ModifiedOn ?? DateTime.Now)
                    };

                return unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc,
                                            "usp_ConvertReferrerToCollateral",
                                            convertProcParams, idResult: (referral.ReferralHeaderID <= 0),
                                            forceRollback: referral.ForceRollback.GetValueOrDefault(false)
                                        );
            }
            else
            {
                return null;
            }
        }

        #endregion Private Methods
    }
}
