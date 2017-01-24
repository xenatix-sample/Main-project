using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Common;
using Axis.DataProvider.Registration.Common;
using Axis.DataProvider.Registration.GeneralRegistration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Contact demography data provider
    /// </summary>
    public class RegistrationDataProvider : RegistrationBaseDataProvider<ContactDemographicsModel>, IRegistrationDataProvider
    {
        #region initializations
        private IUnitOfWork unitOfWork = null;
        private IContactPresentingProblemProvider contactPresentingProblemProvider;

        public RegistrationDataProvider(IUnitOfWork unitOfWork,
        IContactAddressDataProvider _addressDataProvider,
        IContactPhoneDataProvider _phoneDataProvider,
        IContactEmailDataProvider _emailDataProvider,
        IContactClientIdentifierDataProvider _clientIdentifierDataProvider,
        IContactPresentingProblemProvider contactPresentingProblemProvider)
            : base(unitOfWork, _addressDataProvider, _phoneDataProvider, _emailDataProvider, _clientIdentifierDataProvider)
        {
            this.unitOfWork = unitOfWork;
            this.contactPresentingProblemProvider = contactPresentingProblemProvider;
        }
        #endregion initializations

        #region exposed functionality

        protected override string GetStoredProcedureName
        {
            get
            {
                return "usp_GetContactDemographics";
            }
        }

        protected override string AddStoredProcedureName
        {
            get
            {
                return "usp_AddContactDemographics";
            }

        }

        protected override string UpdStoredProcedureName
        {
            get
            {
                return "usp_UpdateContactDemographics";
            }

        }

        protected override string DeleteStoredProcedureName
        {
            get
            {
                return base.DeleteStoredProcedureName;
            }

        }


        public Response<ContactDemographicsModel> AddContactDemographics(ContactDemographicsModel contact)
        {
            return base.AddContact(contact);
        }


        public Response<ContactDemographicsModel> GetContactDemographics(long contactID)
        {
            var result = base.GetContact(contactID, null);

            if (result.DataItems != null && result.DataItems.Count() > 0)
            {
                var lstModel = result.DataItems.FirstOrDefault();
                lstModel.Addresses = lstModel.Addresses.Take(1).ToList();
                lstModel.Phones = lstModel.Phones.Take(1).ToList();
                lstModel.Emails = lstModel.Emails.Take(1).ToList();

                var presentingProblem = contactPresentingProblemProvider.GetContactPresentingProblem(contactID);
                if (presentingProblem.DataItems != null && presentingProblem.DataItems.Count > 0)
                    lstModel.ContactPresentingProblem = presentingProblem.DataItems.FirstOrDefault();

                result.DataItems = new List<ContactDemographicsModel> { lstModel };
            }

            return result;
        }

        public Response<ContactDemographicsModel> UpdateContactDemographics(ContactDemographicsModel contact)
        {
            return base.UpdateContact(contact);
        }

        public Response<ContactDemographicsModel> VerifyDuplicateContacts(ContactDemographicsModel contact)
        {
            var duplicateContactsRepository = unitOfWork.GetRepository<ContactDemographicsModel>(SchemaName.Registration);
            var duplicateContactsParameters = BuildDuplicateContactSpParams(contact);
            return duplicateContactsRepository.ExecuteStoredProc("usp_GetContactPossibleDuplicates", duplicateContactsParameters);
        }
        public override bool ContactOtherOperations(ContactDemographicsModel contact, Response<ContactDemographicsModel> spResults)
        {
            if (base.ContactOtherOperations(contact, spResults))
            {
                if (contact.ContactPresentingProblem != null)
                {
                    var presentingProblemResult = new Response<ContactPresentingProblemModel>();
                    contact.ContactPresentingProblem.ContactID = contact.ContactID;
                    contact.ContactPresentingProblem.TransactionID = contact.TransactionID;
                    contact.ContactPresentingProblem.ScreenID = contact.ScreenID;
                    if (contact.ContactPresentingProblem.ContactPresentingProblemID != 0)
                        presentingProblemResult = contactPresentingProblemProvider.UpdateContactPresentingProblem(contact.ContactPresentingProblem);
                    else
                        presentingProblemResult = contactPresentingProblemProvider.AddContactPresentingProblem(contact.ContactPresentingProblem);

                    if (presentingProblemResult.ResultCode != 0)
                    {
                        spResults.ResultCode = presentingProblemResult.ResultCode;
                        spResults.ResultMessage = presentingProblemResult.ResultMessage;
                        return false;
                    }
                }
            }
            return true;
        }
        protected override List<SqlParameter> BuildContactAddUpdSpParams(ContactDemographicsModel contact)
        {
            var spParameters = new List<SqlParameter>
            {
                new SqlParameter("TransactionLogID", (object)contact.TransactionID ?? DBNull.Value),
                new SqlParameter("ModuleComponentID", (object)contact.ScreenID ?? DBNull.Value),
                new SqlParameter("ContactID", (object)contact.ContactID ?? DBNull.Value),
                new SqlParameter("ContactTypeID", (object)contact.ContactTypeID ?? DBNull.Value),
                new SqlParameter("ClientTypeID", (object)contact.ClientTypeID ?? DBNull.Value),
                new SqlParameter("FirstName", (object)contact.FirstName ?? DBNull.Value),
                new SqlParameter("Middle", (object)contact.Middle ?? DBNull.Value),
                new SqlParameter("LastName", (object)contact.LastName ?? DBNull.Value),
                new SqlParameter("SuffixID", (object)contact.SuffixID ?? DBNull.Value),
                new SqlParameter("GenderID", (object)contact.GenderID ?? DBNull.Value),
                new SqlParameter("PreferredGenderID", (object)contact.PreferredGenderID ?? DBNull.Value),
                new SqlParameter("TitleID", (object)contact.TitleID ?? DBNull.Value),
                new SqlParameter("SequesteredByID", (object)contact.SequesteredByID ?? DBNull.Value),
                new SqlParameter("DOB", (object)contact.DOB ?? DBNull.Value),
                new SqlParameter("DOBStatusID", (object)contact.DOBStatusID ?? DBNull.Value),
                new SqlParameter("SSN", (object)contact.SSN ?? DBNull.Value),
                new SqlParameter("SSNStatusID", (object)contact.SSNStatusID ?? DBNull.Value),
                new SqlParameter("DriverLicense", (object)contact.DriverLicense ?? DBNull.Value),
                new SqlParameter("DriverLicenseStateID", (object)contact.DriverLicenseStateID ?? DBNull.Value),
                new SqlParameter("IsPregnant", (object)contact.IsPregnant ?? DBNull.Value),
                new SqlParameter("PreferredName", (object)contact.PreferredName ?? DBNull.Value),
                new SqlParameter("IsDeceased", (object)contact.IsDeceased ?? DBNull.Value),
                new SqlParameter("DeceasedDate", (object)contact.DeceasedDate ?? DBNull.Value),
                new SqlParameter("CauseOfDeath", (object)contact.CauseOfDeath ?? DBNull.Value),
                new SqlParameter("PreferredContactMethodID", (object)contact.ContactMethodID ?? DBNull.Value),
                new SqlParameter("ReferralSourceID", (object)contact.ReferralSourceID ?? DBNull.Value),
                new SqlParameter("GestationalAge", (object)contact.GestationalAge ?? DBNull.Value),
                new SqlParameter("ModifiedOn", contact.ModifiedOn ?? DateTime.Now) 
            };

            return spParameters;
        }

        protected List<SqlParameter> BuildDuplicateContactSpParams(ContactDemographicsModel contact)
        {
            var spParameters = new List<SqlParameter>
            {
                new SqlParameter("FirstName", (object)contact.FirstName ?? DBNull.Value),
                new SqlParameter("LastName", (object)contact.LastName ?? DBNull.Value),
                new SqlParameter("GenderID", (object)contact.GenderID ?? DBNull.Value),
                new SqlParameter("SSN", (object)contact.SSN ?? DBNull.Value),
                new SqlParameter("DOB", (object)contact.DOB ?? DBNull.Value),
            };

            return spParameters;
        }

        #endregion Helpers

    }
}
