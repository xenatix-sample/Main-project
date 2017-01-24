using System;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Contact phone data provider
    /// </summary>
    public class ContactPhoneDataProvider : IContactPhoneDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPhoneDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactPhoneDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the phones.
        /// </summary>
        /// <param name="contactID">The contact identifier</param>
        /// <param name="contactTypeID">Type of contact type</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> GetPhones(long contactID, int? contactTypeID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            if (contactTypeID.HasValue)
                spParameters.Add(new SqlParameter("ContactTypeID", contactTypeID));
            else
                spParameters.Add(new SqlParameter("ContactTypeID", ContactType.Contact));

            var repository = _unitOfWork.GetRepository<ContactPhoneModel>();
            var results = repository.ExecuteStoredProc("usp_GetContactPhones", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="phones">The phones.</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> AddPhones(long contactID, List<ContactPhoneModel> phones)
        {
            var repository = _unitOfWork.GetRepository<ContactPhoneModel>(SchemaName.Registration);

            var ContactID = new SqlParameter("ContactID", contactID);
            var requestXMLValueParam = new SqlParameter("PhonesXML", PhonesToXML(phones));
            var spParameters = new List<SqlParameter>() { ContactID, requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<ContactPhoneModel>>(repository.ExecuteNQStoredProc, "usp_AddContactPhones", spParameters, forceRollback: phones.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }

        /// <summary>
        /// Phoneses to XML.
        /// </summary>
        /// <param name="phones">The phones.</param>
        /// <returns></returns>
        public string PhonesToXML(List<ContactPhoneModel> phones)
        {
            //TODO: PhoneTypeID is not have text field in UI
            var xEle = new XElement("RequestXMLValue",
                from phone in phones
                select new XElement("Phone",
                        new XElement("PhoneID", phone.PhoneID),
                        new XElement("PhoneTypeID", phone.PhoneTypeID),
                        new XElement("Number", phone.Number),
                        new XElement("Extension", phone.Extension),
                        new XElement("PhonePermissionID", phone.PhonePermissionID),
                        new XElement("IsPrimary", phone.IsPrimary),
                        new XElement("EffectiveDate", phone.EffectiveDate),
                        new XElement("ExpirationDate", phone.ExpirationDate),
                        new XElement("IsActive", phone.IsActive),
                        new XElement("ContactPhoneID", phone.ContactPhoneID),
                        new XElement("ModifiedOn", phone.ModifiedOn ?? DateTime.Now)
                )
            );

            return xEle.ToString();
        }

        /// <summary>
        /// Updates the phones.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="phones">The phones.</param>
        /// <returns></returns>
        public Response<ContactPhoneModel> UpdatePhones(long contactID, List<ContactPhoneModel> phones)
        {
            var phoneResult = new Response<ContactPhoneModel>();
            phoneResult.ResultCode = 0;

            // Prepare phone collection to add/update
            var phonesToAdd = phones.Where(phone => phone.PhoneID == 0).ToList();
            var phonesToUpdate = phones.Where(phone => phone.PhoneID != 0).ToList();
            if (phonesToAdd.Count() > 0)
            {
                phoneResult = AddPhones(contactID, phonesToAdd);
                if (phoneResult.ResultCode != 0)
                {
                    return phoneResult;
                }
            }

            if (phonesToUpdate.Count > 0)
            {
                var requestXMLValueParam = new SqlParameter("PhoneXML", PhonesToXML(phones));
                var spParameters = new List<SqlParameter>() { requestXMLValueParam };
                var repository = _unitOfWork.GetRepository<ContactPhoneModel>(SchemaName.Registration);
                phoneResult = _unitOfWork.EnsureInTransaction<Response<ContactPhoneModel>>(repository.ExecuteNQStoredProc, "usp_UpdateContactPhones", spParameters, forceRollback: phones.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            return phoneResult;
        }

        /// <summary>
        /// Deletes the contact phones.
        /// </summary>
        /// <param name="contactPhoneID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactPhoneModel> DeleteContactPhone(long contactPhoneID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ContactPhoneID", contactPhoneID), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactPhoneRepository = _unitOfWork.GetRepository<ContactPhoneModel>(SchemaName.Registration);
            Response<ContactPhoneModel> spResults = new Response<ContactPhoneModel>();
            spResults = contactPhoneRepository.ExecuteNQStoredProc("usp_DeleteContactPhone", procsParameters);
            return spResults;
        }

        #endregion exposed functionality
    }
}
