using System;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Contact address data provider
    /// </summary>
    public class ContactAddressDataProvider : IContactAddressDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactAddressDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="contactID">The contact identifier</param>
        /// <param name="contactTypeID">Type of Contact</param>
        /// <returns></returns>
        public Response<ContactAddressModel> GetAddresses(long contactID, int? contactTypeID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            if(contactTypeID.HasValue)
                spParameters.Add(new SqlParameter("ContactTypeID", contactTypeID));
            else
                spParameters.Add(new SqlParameter("ContactTypeID", ContactType.Contact));

            var repository = _unitOfWork.GetRepository<ContactAddressModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetContactAddresses", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the addresses.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public Response<ContactAddressModel> AddAddresses(long contactID, List<ContactAddressModel> addresses)
        {
            var repository = _unitOfWork.GetRepository<ContactAddressModel>(SchemaName.Registration);
            var contactIDParameter = new SqlParameter("ContactID", contactID);
            var requestXMLValueParam = new SqlParameter("AddressesXML", AddressesToXML(addresses));
            var spParameters = new List<SqlParameter>() { contactIDParameter, requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<ContactAddressModel>>(repository.ExecuteNQStoredProc, "usp_AddContactAddresses", spParameters, forceRollback: addresses.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }

        /// <summary>
        /// Updates the addresses.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public Response<ContactAddressModel> UpdateAddresses(long contactID, List<ContactAddressModel> addresses)
        {
            var addressResults = new Response<ContactAddressModel>();
            addressResults.ResultCode = 0;
            // Prepare address collection to add/update
            var addressesToAdd = addresses.Where(address => (address.AddressID <= 0 || address.AddressID == null) && (!address.IsDeleted)).ToList();
            var addressesToUpdate = addresses.Where(address => (address.AddressID != 0 && address.AddressID != null) && (!address.IsDeleted)).ToList();
            var addressesToDelete = addresses.Where(address => address.IsDeleted).ToList();

            if (addressesToAdd.Count() > 0)
            {
                addressResults = AddAddresses(contactID, addressesToAdd);
                if (addressResults.ResultCode != 0)
                {
                    return addressResults;
                }
            }
            if (addressesToUpdate.Count() > 0)
            {
                var repository = _unitOfWork.GetRepository<ContactAddressModel>(SchemaName.Registration);
                //var xmlParams = addresses.ToXml2();
                var requestXMLValueParam = new SqlParameter("AddressesXML", AddressesToXML(addressesToUpdate));
                //var spParameters = new List<SqlParameter>() { requestXMLValueParam };
                var spParameters = new List<SqlParameter>() { requestXMLValueParam };
                addressResults= _unitOfWork.EnsureInTransaction<Response<ContactAddressModel>>(repository.ExecuteNQStoredProc,
                    "usp_UpdateContactAddresses", spParameters, forceRollback: addresses.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            if (addressesToDelete.Count() > 0)
            {
                foreach (var addressToDelete in addressesToDelete)
                {
                    DeleteAddress(addressToDelete.ContactAddressID, addressToDelete.ModifiedOn ?? DateTime.Now);
                }
            }
            return addressResults;
        }

        /// <summary>
        /// Copy Contact Addressess.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public Response<ContactAddressModel> CopyContactAddresses(long contactID, List<ContactAddressModel> addresses)
        {
            var addressResults = new Response<ContactAddressModel>();
            addressResults.ResultCode = 0;
            // Prepare address collection to add/update
            var addressesToAdd = addresses.Where(address => address.AddressID == 0 || address.AddressID == null).ToList();
            var addressesToUpdate = addresses.Where(address => address.AddressID != 0 && address.AddressID != null).ToList();

            SqlParameter Action = null;
            SqlParameter requestXMLValueParam = null;
            if (addressesToAdd.Count() > 0)
            {
                Action = new SqlParameter("Action ", "Add");
                requestXMLValueParam = new SqlParameter("AddressesXML", AddressesToXML(addressesToAdd));
            }
            if (addressesToUpdate.Count() > 0)
            {
                Action = new SqlParameter("Action ", "Update");
                requestXMLValueParam = new SqlParameter("AddressesXML", AddressesToXML(addressesToUpdate));
            }
            var repository = _unitOfWork.GetRepository<ContactAddressModel>(SchemaName.Registration);
            var ContactID = new SqlParameter("ContactID", contactID);
            var spParameters = new List<SqlParameter>() { ContactID, requestXMLValueParam, Action };
            addressResults = _unitOfWork.EnsureInTransaction<Response<ContactAddressModel>>(repository.ExecuteNQStoredProc, "usp_CopyContactAddress", spParameters, forceRollback: addresses.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            return addressResults;

        }

        /// <summary>
        /// Addresseses to XML.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public string AddressesToXML(List<ContactAddressModel> addresses)
        {
            var xEle = new XElement("RequestXMLValue",
                from address in addresses
                where address != null
                select new XElement("Address",
                               new XElement("AddressID", address.AddressID),
                               new XElement("ContactAddressID",address.ContactAddressID),
                               new XElement("AddressTypeID", address.AddressTypeID),
                               new XElement("Line1", address.Line1),
                               new XElement("Line2", address.Line2),
                               new XElement("City", address.City),
                               new XElement("StateProvince", address.StateProvince),
                               new XElement("County", address.County),
                               new XElement("Zip", address.Zip),
                               new XElement("ComplexName", address.ComplexName),
                               new XElement("GateCode", address.GateCode),
                               new XElement("MailPermissionID", address.MailPermissionID),
                               new XElement("IsPrimary", address.IsPrimary),
                               new XElement("EffectiveDate", address.EffectiveDate ?? null),
                               new XElement("ExpirationDate", address.ExpirationDate ?? null),
                               new XElement("ModifiedOn", address.ModifiedOn ?? DateTime.Now)
                           ));

            return xEle.ToString();
        }

        /// <summary>
        /// Delete Contact Address
        /// </summary>
        /// <param name="contactAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ContactAddressModel> DeleteAddress(long contactAddressID, DateTime modifiedOn)
        {
            var procsParameters = new List<SqlParameter> { new SqlParameter("ContactAddressID", contactAddressID), new SqlParameter("ModifiedOn", modifiedOn) };
            var addressRepository = _unitOfWork.GetRepository<ContactAddressModel>(SchemaName.Registration);
            var addressResult = addressRepository.ExecuteNQStoredProc("usp_DeleteContactAddress", procsParameters);
            return addressResult;

        }

        #endregion exposed functionality
    }
}
