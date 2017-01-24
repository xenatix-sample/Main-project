using System;
using Axis.Data.Repository;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.DataProvider.Registration.Common;
using Axis.DataProvider.Registration.GeneralRegistration;

namespace Axis.DataProvider.Registration
{
    public class CollateralDataProvider : RegistrationBaseDataProvider<CollateralModel>, ICollateralDataProvider
    {
        #region initializations
        private string _getStoredProcedureName;
      
        public CollateralDataProvider(
        IUnitOfWork unitOfWork,
        IContactAddressDataProvider addressDataProvider,
        IContactPhoneDataProvider phoneDataProvider,
        IContactEmailDataProvider emailDataProvider,
        IContactClientIdentifierDataProvider clientIdentifierDataProvider)
            : base(unitOfWork, addressDataProvider, phoneDataProvider, emailDataProvider, clientIdentifierDataProvider)
        {           
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Get list of collateral for contact
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactTypeID"></param>
        /// <returns></returns>
        public Response<CollateralModel> GetCollaterals(long contactID, int contactTypeID, bool getContactDetails)
        {
            if (getContactDetails)
            {
               GetStoredProcedureName = "usp_GetContactDemographics";
            }
            else
            {
                GetStoredProcedureName = base.GetStoredProcedureName;
            }
            return GetContact(contactID, contactTypeID);
        }

        /// <summary>
        /// Builds the contact get parameters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns>List&lt;SqlParameter&gt;.</returns>
        protected override List<SqlParameter> BuildContactGetParams(long contactID, int? contactTypeID)
        {
            var procsParameters = new List<SqlParameter> { 
                                                            new SqlParameter("ContactID", contactID)
                                                        };
            if (contactTypeID.HasValue &&  GetStoredProcedureName == base.GetStoredProcedureName)
                procsParameters.Add(new SqlParameter("ContactTypeID", (object)contactTypeID ?? DBNull.Value));

            return procsParameters;
        }

        /// <summary>
        /// To override GetStoredProcedureName
        /// </summary>
        /// <returns>procedure name</returns>
        protected override string GetStoredProcedureName
        {
            get
            {
                return _getStoredProcedureName;
            }
            set
            {
                _getStoredProcedureName = value;
            }
        }
        /// <summary>
        /// Add collateral for contact
        /// </summary>
        /// <param name="CollateralModel"></param>
        /// <returns></returns>
        public Response<CollateralModel> AddCollateral(CollateralModel collateralModel)
        {
            return base.AddContact(collateralModel);
        }

        /// <summary>
        /// Update collateral for contact
        /// </summary>
        /// <param name="CollateralModel"></param>
        /// <returns></returns>
        public Response<CollateralModel> UpdateCollateral(CollateralModel collateralModel)
        {
            return base.UpdateContact(collateralModel);
        }


        /// <summary>
        /// Deletes the collateral.
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public new Response<CollateralModel> DeleteCollateral(long parentContactID, long contactID, DateTime modifiedOn)
        {
            return base.DeleteCollateral(parentContactID,contactID, modifiedOn);
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Build parameters to be passed to stored procedure
        /// </summary>
        /// <param name="collateralModel"></param>
        /// <returns></returns>
        protected override List<SqlParameter> BuildContactAddUpdSpParams(CollateralModel collateralModel)
        {

            var spParameters = new List<SqlParameter>()
            {
                new SqlParameter("ParentContactID", collateralModel.ParentContactID),
                new SqlParameter("ContactID", collateralModel.ContactID),
                new SqlParameter("ContactTypeID", collateralModel.ContactTypeID),
                new SqlParameter("FirstName", collateralModel.FirstName),
                new SqlParameter("Middle", (object)collateralModel.Middle ?? DBNull.Value),
                new SqlParameter("LastName", collateralModel.LastName),
                new SqlParameter("SuffixID", (object)collateralModel.SuffixID ?? DBNull.Value),
                new SqlParameter("GenderID", (object)collateralModel.GenderID ?? DBNull.Value),
                new SqlParameter("DOB", (object)collateralModel.DOB ?? DBNull.Value),
                new SqlParameter("LivingWithClientStatus", (object)collateralModel.LivingWithClientStatus ?? DBNull.Value),
                new SqlParameter("ReceiveCorrespondenceID", (object)collateralModel.ReceiveCorrespondenceID ?? DBNull.Value),
                new SqlParameter("SSN", (object)collateralModel.SSN ?? DBNull.Value),
                new SqlParameter("DriverLicense", (object)collateralModel.DriverLicense ?? DBNull.Value),
                new SqlParameter("DriverLicenseStateID", (object)collateralModel.DriverLicenseStateID ?? DBNull.Value),
                new SqlParameter("EmergencyContact", (object)collateralModel.EmergencyContact ?? false),
                new SqlParameter("EducationStatusID", (object)collateralModel.EducationStatusID ?? DBNull.Value),
                new SqlParameter("SchoolAttended", (object)collateralModel.SchoolAttended ?? DBNull.Value),
                new SqlParameter("SchoolBeginDate", (object)collateralModel.SchoolBeginDate ?? DBNull.Value),
                new SqlParameter("SchoolEndDate", (object)collateralModel.SchoolEndDate ?? DBNull.Value),
                new SqlParameter("EmploymentStatusID", (object)collateralModel.EmploymentStatusID ?? DBNull.Value),
                new SqlParameter("VeteranStatusID",(object)collateralModel.VeteranStatusID??DBNull.Value),
                new SqlParameter("CollateralEffectiveDate",(object)collateralModel.CollateralEffectiveDate??DBNull.Value),
                new SqlParameter("CollateralExpirationDate",(object)collateralModel.CollateralExpirationDate??DBNull.Value),
                new SqlParameter("ModifiedOn", collateralModel.ModifiedOn ?? DateTime.Now)
            };

            if (collateralModel.ContactRelationshipID > 0)
            {
                spParameters.Insert(0, new SqlParameter("ContactRelationshipID", collateralModel.ContactRelationshipID));
            }
            return spParameters;
        }

        #endregion Helpers
    }
}
