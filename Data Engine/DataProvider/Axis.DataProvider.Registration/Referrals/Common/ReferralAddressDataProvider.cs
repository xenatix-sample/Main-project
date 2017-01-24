using System;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.Registration.Referrals.Common
{
    public class ReferralAddressDataProvider : IReferralAddressDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralAddressDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralAddressModel> GetAddresses(long referralID, int? contactTypeID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ReferralID", referralID) };
            if(contactTypeID.HasValue)
                spParameters.Add(new SqlParameter("ContactTypeID", contactTypeID));
            else
                spParameters.Add(new SqlParameter("ContactTypeID", ContactType.Contact));

            var repository = _unitOfWork.GetRepository<ReferralAddressModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetReferralAddresses", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        Response<ReferralAddressModel> AddAddresses(long referralID, List<ReferralAddressModel> addresses)
        {
            var repository = _unitOfWork.GetRepository<ReferralAddressModel>(SchemaName.Registration);

            var referralIDParameter = new SqlParameter("ReferralID", referralID);
            var requestXMLValueParam = new SqlParameter("AddressesXML", AddressesToXML(addresses));
            var spParameters = new List<SqlParameter>() { referralIDParameter, requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<ReferralAddressModel>>(repository.ExecuteNQStoredProc, "usp_AddReferralAddresses", spParameters, forceRollback: addresses.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }

        /// <summary>
        /// Add and update addresses.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public Response<ReferralAddressModel> AddUpdateAddresses(List<ReferralAddressModel> addresses)
        {
            long referralID = 0;
            if (addresses.Count > 0)
                referralID = addresses.Single().ReferralID;

            var addressResults = new Response<ReferralAddressModel>();
            addressResults.ResultCode = 0;
            // Prepare address collection to add/update
            var addressesToAdd = addresses.Where(address => (address.AddressID <= 0 || address.AddressID == null) && (!address.IsDeleted)).ToList();
            var addressesToUpdate = addresses.Where(address => (address.AddressID != 0 && address.AddressID != null) && (!address.IsDeleted)).ToList();
            var addressesToDelete = addresses.Where(address => address.IsDeleted).ToList();

            if (addressesToAdd.Count() > 0)
            {
                 addressResults = AddAddresses(referralID, addressesToAdd);
                if (addressResults.ResultCode != 0)
                {
                    return addressResults;
                }
            }
            if (addressesToUpdate.Count() > 0)
            {
                var repository = _unitOfWork.GetRepository<ReferralAddressModel>(SchemaName.Registration);
                //var xmlParams = addresses.ToXml2();
                var requestXMLValueParam = new SqlParameter("AddressesXML", AddressesToXML(addressesToUpdate));
                //var spParameters = new List<SqlParameter>() { requestXMLValueParam };
                var spParameters = new List<SqlParameter>() { requestXMLValueParam };
                addressResults= _unitOfWork.EnsureInTransaction<Response<ReferralAddressModel>>(repository.ExecuteNQStoredProc,
                    "usp_UpdateReferralAddresses", spParameters, forceRollback: addresses.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            if (addressesToDelete.Count() > 0)
            {
                foreach (var addressToDelete in addressesToDelete)
                {
                    DeleteAddress(addressToDelete.ReferralAddressID, addressToDelete.ModifiedOn ?? DateTime.Now);
                }
            }
            return addressResults;
        }

        /// <summary>
        /// Addresseses to XML.
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public string AddressesToXML(List<ReferralAddressModel> addresses)
        {
            var xEle = new XElement("RequestXMLValue",
                from address in addresses
                where address != null
                select new XElement("Address",
                               new XElement("AddressID", address.AddressID),
                               new XElement("ReferralAddressID",address.ReferralAddressID),
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
                               new XElement("ModifiedOn", address.ModifiedOn ?? DateTime.Now)
                           ));

            return xEle.ToString();
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="referralAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralAddressModel> DeleteAddress(long referralAddressID, DateTime modifiedOn)
        {
            var procsParameters = new List<SqlParameter> { new SqlParameter("ReferralAddressID", referralAddressID), new SqlParameter("ModifiedOn", modifiedOn) };
            var addressRepository = _unitOfWork.GetRepository<ReferralAddressModel>(SchemaName.Registration);
            var addressResult = addressRepository.ExecuteNQStoredProc("usp_DeleteReferralAddress", procsParameters);
            return addressResult;

        }

        #endregion exposed functionality
    }
}
