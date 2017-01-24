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
    public class EmergencyContactDataProvider : RegistrationBaseDataProvider<EmergencyContactModel>, IEmergencyContactDataProvider
    {
        #region initializations

        public EmergencyContactDataProvider(
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
        /// To get Emergency Contact list for contact
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns>Response of type EmergencyContactModel</returns>
        public Response<EmergencyContactModel> GetEmergencyContacts(long contactID, int contactTypeID)
        {
            return base.GetContact(contactID, contactTypeID);
            }

        /// <summary>
        /// To add emergency contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response of type EmergencyContactModel</returns>
        public Response<EmergencyContactModel> AddEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            return base.AddContact(emergencyContactModel);
        }

        /// <summary>
        /// To update emergency contact
        /// </summary>
        /// <param name="emergencyContactModel">Emergency Contact Model</param>
        /// <returns>Response of type EmergencyContactModel</returns>
        public Response<EmergencyContactModel> UpdateEmergencyContact(EmergencyContactModel emergencyContactModel)
        {
            return base.UpdateContact(emergencyContactModel);
        }

        /// <summary>
        /// To remove emergency contact
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<EmergencyContactModel> DeleteEmergencyContact(long Id, DateTime modifiedOn)
        {
            return base.DeleteContact(Id, modifiedOn);
        }

        #endregion exposed functionality

        #region Helpers

        /// <summary>
        /// Build parameters to be passed to stored procedure
        /// </summary>
        /// <param name="emergencyContactModel"></param>
        /// <returns></returns>
        protected override List<SqlParameter> BuildContactAddUpdSpParams(EmergencyContactModel emergencyContactModel)
        {
            var spParameters = new List<SqlParameter>
            {
                new SqlParameter("ContactRelationshipID", emergencyContactModel.ContactRelationshipID),
                new SqlParameter("ParentContactID", emergencyContactModel.ParentContactID),
                new SqlParameter("ContactID", emergencyContactModel.ContactID),
                new SqlParameter("ContactTypeID", emergencyContactModel.ContactTypeID),
                new SqlParameter("FirstName", emergencyContactModel.FirstName),
                new SqlParameter("Middle", (object)emergencyContactModel.Middle ?? DBNull.Value),
                new SqlParameter("LastName", emergencyContactModel.LastName),
                new SqlParameter("SuffixID", (object)emergencyContactModel.SuffixID ?? DBNull.Value),
                new SqlParameter("GenderID", (object)emergencyContactModel.GenderID ?? DBNull.Value),
                new SqlParameter("DOB", (object)emergencyContactModel.DOB ?? DBNull.Value),
                new SqlParameter("RelationshipTypeID", emergencyContactModel.RelationshipTypeID),
                new SqlParameter("LivingWithClientStatusID", (object)emergencyContactModel.LivingWithClientStatusID ?? DBNull.Value),
                new SqlParameter("SSN", (object)emergencyContactModel.SSN ?? DBNull.Value),
                new SqlParameter("DriverLicense", (object)emergencyContactModel.DriverLicense ?? DBNull.Value),
                new SqlParameter("DriverLicenseStateID", DBNull.Value),
                new SqlParameter("AlternateID", (object)emergencyContactModel.AlternateID ?? DBNull.Value),
                new SqlParameter("ClientIdentifierTypeID", DBNull.Value),
                new SqlParameter("ModifiedOn", emergencyContactModel.ModifiedOn ?? DateTime.Now)
            };

            return spParameters;
        }

        #endregion Helpers
    }
}
