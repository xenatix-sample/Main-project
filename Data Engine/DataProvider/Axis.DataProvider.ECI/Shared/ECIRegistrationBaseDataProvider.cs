using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Registration;
using Axis.DataProvider.Registration.Common;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.Registration;
using Axis.Logging;

namespace Axis.DataProvider.ECI.Shared
{
    public abstract class ECIRegistrationBaseDataProvider<Model> where Model : ECIContactBaseModel, new()
    {
        #region Class Variables/Properties

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = null;
        /// <summary>
        /// The address data provider
        /// </summary>
        private readonly IContactAddressDataProvider _addressDataProvider;
        /// <summary>
        /// The phone data provider
        /// </summary>
        private readonly IContactPhoneDataProvider _phoneDataProvider;
        /// <summary>
        /// The email data provider
        /// </summary>
        private readonly IContactEmailDataProvider _emailDataProvider;
        /// <summary>
        /// The client Identifier data provider
        /// </summary>
        private IContactClientIdentifierDataProvider _clientIdentifierDataProvider;

        /// <summary>
        /// Logging
        /// </summary>
        private readonly ILogger _logger = null;
        /// <summary>
        /// Gets the name of the get stored procedure.
        /// </summary>
        /// <value>The name of the get stored procedure.</value>
        protected virtual string GetStoredProcedureName
        {
            get
            {
                return "usp_GetContactRelationForContactType";
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ECIRegistrationBaseDataProvider{Model}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="addressDataProvider">The address data provider.</param>
        /// <param name="phoneDataProvider">The phone data provider.</param>
        /// <param name="emailDataProvider">The email data provider.</param>
        public ECIRegistrationBaseDataProvider(IUnitOfWork unitOfWork, IContactAddressDataProvider addressDataProvider,
                                               IContactPhoneDataProvider phoneDataProvider, IContactEmailDataProvider emailDataProvider,
                                               IContactClientIdentifierDataProvider clientIdentifierDataProvider)
        {
            this._unitOfWork = unitOfWork;
            this._addressDataProvider = addressDataProvider;
            this._phoneDataProvider = phoneDataProvider;
            this._emailDataProvider = emailDataProvider;
            this._clientIdentifierDataProvider = clientIdentifierDataProvider;
            this._logger = new Logger(true);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <param name="contactID">The contact identifier</param>
        /// <param name="contactTypeID">The contact type identifier</param>
        /// <returns>Response</returns>
        public virtual Response<Model> GetContact(long contactID, int? contactTypeID)
        {
            //var procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var procsParameters = BuildContactGetParams(contactID, contactTypeID);

            var contactRepository = _unitOfWork.GetRepository<Model>(SchemaName.Registration);
            var contactResults = contactRepository.ExecuteStoredProc(GetStoredProcedureName, procsParameters);

            if (contactResults == null || contactResults.ResultCode != 0) return contactResults;

            if (contactResults.DataItems.Count == 0)
            {
                return contactResults;
            }
            var addressResults = _addressDataProvider.GetAddresses(contactID, contactTypeID);

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
                        (addressResults.DataItems.Where(a => a.ContactID == m.ContactID).OrderByDescending(a => a.IsPrimary).ThenByDescending(a => a.ContactAddressID).Take(1).ToList());
                });
            }
            var phoneResults = _phoneDataProvider.GetPhones(contactID, contactTypeID);
            if (phoneResults.ResultCode != 0)
            {
                contactResults.ResultCode = phoneResults.ResultCode;
                contactResults.ResultMessage = phoneResults.ResultMessage;
            }
            else
            {
                contactResults.DataItems.ForEach(m =>
                {
                    m.Phones = (phoneResults.DataItems.Where(a => a.ContactID == m.ContactID).OrderByDescending(a => a.IsPrimary).ThenByDescending(a => a.ContactPhoneID).Take(1).ToList());
                });
            }

            var emailResults = _emailDataProvider.GetEmails(contactID, contactTypeID);
            if (emailResults.ResultCode != 0)
            {
                contactResults.ResultCode = emailResults.ResultCode;
                contactResults.ResultMessage = emailResults.ResultMessage;
            }
            else
            {
                contactResults.DataItems.ForEach(m =>
                {
                    m.Emails = (emailResults.DataItems.Where(a => a.ContactID == m.ContactID).OrderByDescending(a => a.IsPrimary).ThenByDescending(a => a.ContactEmailID).Take(1).ToList());
                });
            }

            var clientIdentifierResults = _clientIdentifierDataProvider.GetContactClientIdentifiers(contactID);
            if (clientIdentifierResults.ResultCode != 0)
            {
                contactResults.ResultCode = clientIdentifierResults.ResultCode;
                contactResults.ResultMessage = clientIdentifierResults.ResultMessage;
            }
            else
            {
                contactResults.DataItems.ForEach(m =>
                {
                    m.ClientAlternateIDs = (clientIdentifierResults.DataItems.Where(a => a.ContactID == m.ContactID).ToList());
                });
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
            var contactRepository = _unitOfWork.GetRepository<Model>(SchemaName.Registration);
            Response<Model> spResults = new Response<Model>();

            using (var transactionScope = _unitOfWork.BeginTransactionScope())
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
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }
            end:
            return spResults;
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
                _unitOfWork.GetRepository<Model>(SchemaName.Registration);
            Response<Model> spResults = new Response<Model>();
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    if (!contact.isContactNotDirty)
                    {

                        spResults = contactRepository.ExecuteNQStoredProc(UpdStoredProcedureName,
                        contactParameters);
                        if (spResults.ResultCode != 0)
                            goto end;
                    }
                    spResults.ID = contact.ContactID;

                    if (!ContactOtherOperations(contact, spResults))
                    {
                        goto end;
                    }

                    if (!contact.ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }
            end:
            return spResults;
        }

        #endregion

        #region Private Methods

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
                addressResults = _addressDataProvider.CopyContactAddresses(spResults.ID, contact.Addresses);
            }
            else
            {
                addressResults = _addressDataProvider.UpdateAddresses(spResults.ID, contact.Addresses);
            }
            if (addressResults.ResultCode != 0)
            {
                spResults.ResultCode = addressResults.ResultCode;
                spResults.ResultMessage = addressResults.ResultMessage;
                return false;
            }

            var emailResults = _emailDataProvider.UpdateEmails(spResults.ID, contact.Emails);
            if (emailResults.ResultCode != 0)
            {
                spResults.ResultCode = emailResults.ResultCode;
                spResults.ResultMessage = emailResults.ResultMessage;
                return false;
            }

            var phoneResults = _phoneDataProvider.UpdatePhones(spResults.ID, contact.Phones);

            if (phoneResults.ResultCode != 0)
            {
                spResults.ResultCode = phoneResults.ResultCode;
                spResults.ResultMessage = phoneResults.ResultMessage;
                return false;
            }

            var clientIdentifierResults = _clientIdentifierDataProvider.AddUpdateContactClientIdentifiers(spResults.ID, contact.ClientAlternateIDs);
            if (clientIdentifierResults.ResultCode != 0)
            {
                spResults.ResultCode = clientIdentifierResults.ResultCode;
                spResults.ResultMessage = clientIdentifierResults.ResultMessage;
                return false;
            }

            return true;
        }

        #endregion

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
            var procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            if (contactTypeID.HasValue)
                procsParameters.Add(new SqlParameter("ContactTypeID", (object)contactTypeID ?? DBNull.Value));

            return procsParameters;
        }

        #endregion Helpers
    }
}
