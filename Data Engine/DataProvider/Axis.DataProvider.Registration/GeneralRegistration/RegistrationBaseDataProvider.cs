using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Registration.Common;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Registration.GeneralRegistration
{
    /// <summary>
    /// Class RegistrationBaseDataProvider.
    /// </summary>
    /// <typeparam name="Model">The type of the model.</typeparam>
    public abstract class RegistrationBaseDataProvider<Model> where Model : ContactBaseModel, new()
    {
        #region initializations
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;
        /// <summary>
        /// The address data provider
        /// </summary>
        private IContactAddressDataProvider addressDataProvider;
        /// <summary>
        /// The phone data provider
        /// </summary>
        private IContactPhoneDataProvider phoneDataProvider;
        /// <summary>
        /// The email data provider
        /// </summary>
        private IContactEmailDataProvider emailDataProvider;
        /// <summary>
        /// The client Identifier data provider
        /// </summary>
        private IContactClientIdentifierDataProvider clientIdentifierDataProvider;

        private ILogger logger = null;

        private string _getStoredProcedureName = "usp_GetContactRelationForContactType";

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationBaseDataProvider{Model}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="addressDataProvider">The address data provider.</param>
        /// <param name="phoneDataProvider">The phone data provider.</param>
        /// <param name="emailDataProvider">The email data provider.</param>
        public RegistrationBaseDataProvider(IUnitOfWork unitOfWork,
         IContactAddressDataProvider addressDataProvider,
         IContactPhoneDataProvider phoneDataProvider,
         IContactEmailDataProvider emailDataProvider,
         IContactClientIdentifierDataProvider clientIdentifierDataProvider)
        {
            this.unitOfWork = unitOfWork;
            this.addressDataProvider = addressDataProvider;
            this.phoneDataProvider = phoneDataProvider;
            this.emailDataProvider = emailDataProvider;
            this.clientIdentifierDataProvider = clientIdentifierDataProvider;
            this.logger = new Logger(true);
        }
        #endregion initializations


        #region protected properties

        /// <summary>
        /// Gets the name of the get stored procedure.
        /// </summary>
        /// <value>The name of the get stored procedure.</value>
        protected virtual string GetStoredProcedureName
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
        /// Gets the name of the add stored procedure.
        /// </summary>
        /// <value>The name of the add stored procedure.</value>
        protected virtual string AddStoredProcedureName
        {
            get
            {
                return "usp_AddRelationshipContact";
            }

        }
        /// <summary>
        /// Gets the name of the upd stored procedure.
        /// </summary>
        /// <value>The name of the upd stored procedure.</value>
        protected virtual string UpdStoredProcedureName
        {
            get
            {
                return "usp_UpdateRelationshipContact";
            }

        }
        /// <summary>
        /// Gets the name of the delete stored procedure.
        /// </summary>
        /// <value>The name of the delete stored procedure.</value>
        protected virtual string DeleteStoredProcedureName
        {
            get
            {
                return "usp_DeleteContactRelationship";
            }

        }
        #endregion protected properties

        #region Helpers
        /// <summary>
        /// Builds the contact add upd sp parameters.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>List&lt;SqlParameter&gt;.</returns>
        protected abstract List<SqlParameter> BuildContactAddUpdSpParams(Model contact);
        /// <summary>
        /// Builds the contact get parameters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns>List&lt;SqlParameter&gt;.</returns>
        protected virtual List<SqlParameter> BuildContactGetParams(long contactID, int? contactTypeID)
        {
            var procsParameters = new List<SqlParameter> { 
                                                            new SqlParameter("ContactID", contactID)
                                                        };
            if (contactTypeID.HasValue)
                procsParameters.Add(new SqlParameter("ContactTypeID", (object)contactTypeID ?? DBNull.Value));

            return procsParameters;
        }
        #endregion Helpers

        #region exposed functionality

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns>Response&lt;Model&gt;.</returns>
        public virtual Response<Model> GetContact(long contactID, int? contactTypeID)
        {
            //var procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var procsParameters = BuildContactGetParams(contactID, contactTypeID);

            var contactRepository = unitOfWork.GetRepository<Model>(SchemaName.Registration);
            var contactResults = contactRepository.ExecuteStoredProc(GetStoredProcedureName, procsParameters);

            if (contactResults == null || contactResults.ResultCode != 0) return contactResults;

            if (contactResults.DataItems.Count == 0)
            {
                return contactResults;
            }
            contactTypeID = contactResults.DataItems[0].ContactTypeID;
            var addressResults = addressDataProvider.GetAddresses(contactID, contactTypeID);

            if (addressResults.ResultCode != 0)
            {
                contactResults.ResultCode = addressResults.ResultCode;
                contactResults.ResultMessage = addressResults.ResultMessage;
            }
            else
            {
                contactResults.DataItems.ForEach(m =>
                {
                    m.Addresses =
                        (addressResults.DataItems.Where(a => ((a.ContactID == m.ContactID) && (a.EffectiveDate == null || a.EffectiveDate <= DateTime.Now) && (a.ExpirationDate == null || a.ExpirationDate > DateTime.Now))).OrderByDescending(a => a.IsPrimary).ThenByDescending(a => a.ContactAddressID).Take(1).ToList());
                });
            }
            var phoneResults = phoneDataProvider.GetPhones(contactID, contactTypeID);
            if (phoneResults.ResultCode != 0)
            {
                contactResults.ResultCode = phoneResults.ResultCode;
                contactResults.ResultMessage = phoneResults.ResultMessage;
            }
            else
            {
                contactResults.DataItems.ForEach(m =>
                {
                    //m.Phones = phoneResults.DataItems.Where(a => a.ContactID == m.ContactID && a.IsPrimary == true).ToList();
                    m.Phones = (phoneResults.DataItems.Where(a => ((a.ContactID == m.ContactID) && (a.EffectiveDate == null || a.EffectiveDate <= DateTime.Now) && (a.ExpirationDate == null || a.ExpirationDate > DateTime.Now))).OrderByDescending(a => a.IsPrimary).ThenByDescending(a => a.ContactPhoneID).Take(1).ToList());
                });
            }

            var emailResults = emailDataProvider.GetEmails(contactID, contactTypeID);
            if (emailResults.ResultCode != 0)
            {
                contactResults.ResultCode = emailResults.ResultCode;
                contactResults.ResultMessage = emailResults.ResultMessage;
            }
            else
            {
                contactResults.DataItems.ForEach(m =>
                {
                    //m.Emails = emailResults.DataItems.Where(a => a.ContactID == m.ContactID && a.IsPrimary == true).ToList();
                    m.Emails = (emailResults.DataItems.Where(a => ((a.ContactID == m.ContactID) && (a.EffectiveDate == null || a.EffectiveDate <= DateTime.Now) && (a.ExpirationDate == null || a.ExpirationDate > DateTime.Now))).OrderByDescending(a => a.IsPrimary).ThenByDescending(a => a.ContactEmailID).Take(1).ToList());
                });
            }

            foreach (var contact in contactResults.DataItems)
            {
                var clientIdentifierResults = clientIdentifierDataProvider.GetContactClientIdentifiers(contact.ContactID);
                if (clientIdentifierResults.ResultCode != 0)
                {
                    contactResults.ResultCode = clientIdentifierResults.ResultCode;
                    contactResults.ResultMessage = clientIdentifierResults.ResultMessage;
                }
                else
                {
                    contact.ClientAlternateIDs = clientIdentifierResults.DataItems;
                }
            }



            return contactResults;
        }

        /// <summary>
        /// Adds the contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>Response&lt;Model&gt;.</returns>
        public virtual Response<Model> AddContact(Model contact)
        {
            var contactParameters = BuildContactAddUpdSpParams(contact);
            var contactRepository = unitOfWork.GetRepository<Model>(SchemaName.Registration);
            Response<Model> spResults = new Response<Model>();

            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                try
                {
                    spResults = contactRepository.ExecuteNQStoredProc(AddStoredProcedureName, contactParameters,
                        idResult: true);
                    contact.ContactID = spResults.ID;

                    if (spResults.ResultCode != 0)
                    {
                        spResults.ResultCode = spResults.ResultCode;
                        spResults.ResultMessage = spResults.ResultMessage;
                        goto end;
                    }
                    if (!ContactOtherOperations(contact, spResults))
                    {
                        goto end;
                    }

                    if (!contact.ForceRollback.GetValueOrDefault(false))
                        unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }
        end:
            return spResults;
        }

        /// <summary>
        /// Perform Address,Phone,Email Add update operations
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="spResults"></param>
        /// <returns></returns>
       public virtual bool ContactOtherOperations(Model contact, Response<Model> spResults)
        {
            Response<ContactAddressModel> addressResults = null;

            if (contact.CopyContactAddress)
            {
                var address = contact.Addresses.Select(a => { a.AddressID = null; return a; });
                addressResults = addressDataProvider.CopyContactAddresses(spResults.ID, address.ToList());
            }
            else
            {
                addressResults = addressDataProvider.UpdateAddresses(spResults.ID, contact.Addresses);
            }
            if (addressResults.ResultCode != 0)
            {
                spResults.ResultCode = addressResults.ResultCode;
                spResults.ResultMessage = addressResults.ResultMessage;
                return false;
            }

            var emailResults = emailDataProvider.UpdateEmails(spResults.ID, contact.Emails);
            if (emailResults.ResultCode != 0)
            {
                spResults.ResultCode = emailResults.ResultCode;
                spResults.ResultMessage = emailResults.ResultMessage;
                return false;
            }

            var phoneResults = phoneDataProvider.UpdatePhones(spResults.ID, contact.Phones);

            if (phoneResults.ResultCode != 0)
            {
                spResults.ResultCode = phoneResults.ResultCode;
                spResults.ResultMessage = phoneResults.ResultMessage;
                return false;
            }

            var clientIdentifierResults = clientIdentifierDataProvider.AddUpdateContactClientIdentifiers(spResults.ID, contact.ClientAlternateIDs);
            if (clientIdentifierResults.ResultCode != 0)
            {
                spResults.ResultCode = clientIdentifierResults.ResultCode;
                spResults.ResultMessage = clientIdentifierResults.ResultMessage;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns>Response&lt;Model&gt;.</returns>
        public virtual Response<Model> UpdateContact(Model contact)
        {
            var contactParameters = BuildContactAddUpdSpParams(contact);
            var contactRepository =
                unitOfWork.GetRepository<Model>(SchemaName.Registration);
            Response<Model> spResults = new Response<Model>();
            using (var transactionScope = unitOfWork.BeginTransactionScope())
            {
                try
                {
                    if (!contact.isContactNotDirty)
                    {
                        spResults = contactRepository.ExecuteNQStoredProc(UpdStoredProcedureName, contactParameters);
                       
                        if (spResults.ResultCode != 0)
                            goto end;
                    }
                    spResults.ID = contact.ContactID;
                    if (!ContactOtherOperations(contact, spResults))
                    {
                        goto end;
                    }

                    if (!contact.ForceRollback.GetValueOrDefault(false))
                        unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }
        end:
            return spResults;
        }


        /// <summary>
        /// Gets the  Contact Address
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public virtual Response<ContactAddressModel> GetContactAddress(long contactID)
        {
            return addressDataProvider.GetAddresses(contactID, null);
        }

        /// <summary>
        /// Deletes the contact.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Response&lt;Model&gt;.</returns>
        public virtual Response<Model> DeleteContact(long Id, DateTime modifiedOn)
        {
            var procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", Id), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactRepository = unitOfWork.GetRepository<Model>(SchemaName.Registration);
            var contactResults = contactRepository.ExecuteNQStoredProc(DeleteStoredProcedureName, procsParameters);
            return contactResults;
        }


        /// <summary>
        /// Get client records based on the search criteria
        /// </summary>
        /// <param name="SearchCriteria">Search Criteria entered by user</param>
        /// <param name="contactType">contact type of contact</param>
        /// <returns>
        /// ContactDemographicsModel
        /// </returns>
        public Response<ContactDemographicsModel> GetClientSummary(string SearchCriteria, string ContactType)
        {
            SqlParameter searchParam = new SqlParameter("SearchCriteria", SearchCriteria);
            SqlParameter contactType = new SqlParameter("ContactType", ContactType ?? string.Empty);
            SqlParameter userID = new SqlParameter("UserID", AuthContext.Auth.User.UserID);
            List<SqlParameter> procsParameters = new List<SqlParameter>() { searchParam, contactType, userID };
            var contactRepository = unitOfWork.GetRepository<ContactDemographicsModel>(SchemaName.Registration);
            var contactResults = contactRepository.ExecuteStoredProc("usp_GetClientSearchResults", procsParameters);
            return contactResults;
        }

        /// <summary>
        /// Deletes the collateral.
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public virtual Response<Model> DeleteCollateral(long parentContactID, long contactID, DateTime modifiedOn)
        {
            var procsParameters = new List<SqlParameter> { new SqlParameter("ParentContactID", parentContactID), new SqlParameter("ContactID", contactID), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactRepository = unitOfWork.GetRepository<Model>(SchemaName.Registration);
            var contactResults = contactRepository.ExecuteNQStoredProc(DeleteStoredProcedureName, procsParameters);
            return contactResults;
        }

        /// <summary>
        /// Get the SSN.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<String> GetSSN(long contactID)
        {
            var procsParameters = new List<SqlParameter> {new SqlParameter("ContactID", contactID)};
            var contactRepository = unitOfWork.GetRepository<String>(SchemaName.Registration);
            var contactResults = contactRepository.ExecuteStoredProc("usp_GetContactSSN", procsParameters);
            return contactResults;
        }

        #endregion exposed functionality
    }
}
