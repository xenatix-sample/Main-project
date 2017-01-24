using System;
using Axis.Data.Repository;
using Axis.Model.Address;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Model.Common;
using System.Xml.Linq;
using System.Linq;
using Axis.Data.Repository.Schema;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AddressDataProvider : IAddressDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public AddressDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<AddressModel> GetAddress(int addressID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("AddressID", addressID) };
            var repository = _unitOfWork.GetRepository<AddressModel>();
            var results = repository.ExecuteStoredProc("usp_GetAddress", spParameters);

            return results;
        }

        public Response<AddressModel> AddAddress(AddressModel address)
        {
            var repository = _unitOfWork.GetRepository<AddressModel>();

            var requestXMLValueParam = new SqlParameter("AddressXML", AddressesToXML(new List<AddressModel> { address }));
            var spParameters = new List<SqlParameter>() { requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<AddressModel>>(repository.ExecuteNQStoredProc, "usp_AddAddress", spParameters, forceRollback: address.ForceRollback.GetValueOrDefault(false));
        }

        public string AddressesToXML(List<AddressModel> addresses)
        {
            var xEle = new XElement("RequestXMLValue",
                from address in addresses
                where address!=null
                select new XElement("Address",
                               new XElement("AddressID", address.AddressID),
                               new XElement("AddressTypeID", address.AddressTypeID),
                               new XElement("Line1", address.Line1),
                               new XElement("Line2", address.Line2),
                               new XElement("City", address.City),
                               new XElement("StateProvince", address.StateProvince),
                               new XElement("County", address.County),
                               new XElement("Zip", address.Zip),
                               new XElement("ComplexName", address.ComplexName),
                               new XElement("GateCode", address.GateCode),
                               new XElement("ModifiedOn", address.ModifiedOn ?? DateTime.Now)
                           ));

            return xEle.ToString();
        }

        public string AddressesToXml(List<UserAddressModel> addresses)
        {
            var xEle = new XElement("RequestXMLValue",
                from address in addresses
                where address != null
                select new XElement("Address",
                               new XElement("AddressID", address.AddressID),
                               new XElement("UserAddressID", address.UserAddressID),
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

        public Response<AddressModel> UpdateAddress(AddressModel address)
        {
            var repository = _unitOfWork.GetRepository<AddressModel>(SchemaName.Core);
            var requestXMLValueParam = new SqlParameter("AddressXML", AddressesToXML(new List<AddressModel> { address }));
            var spParameters = new List<SqlParameter>() { requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<AddressModel>>(repository.ExecuteNQStoredProc, "usp_UpdateAddress", spParameters, forceRollback: address.ForceRollback.GetValueOrDefault(false));
        }

        #region UserAddress

        public Response<UserAddressModel> GetUserAddresses(int userID)
        {
            var userAddressRepository = _unitOfWork.GetRepository<UserAddressModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var userAddressResult = userAddressRepository.ExecuteStoredProc("usp_GetUserAddresses", procParams);

            return userAddressResult;
        }

        public Response<UserAddressModel> SaveUserAddresses(int userID, List<UserAddressModel> addresses)
        {
            var userAddressRepository = _unitOfWork.GetRepository<UserAddressModel>();
            var userAddressResult = new Response<UserAddressModel>() { ResultCode = 0 };

            var addressesToAdd = addresses.Where(address => address.AddressID == 0 || address.AddressID == null).ToList();
            var addressesToUpdate = addresses.Where(address => address.AddressID != 0).ToList();

            if (addressesToAdd.Count > 0)
            {
                SqlParameter userIDParam = new SqlParameter("UserID", userID);
                var requestXmlValueParam = new SqlParameter("AddressXML", AddressesToXml(addressesToAdd));
                var addressParameters = new List<SqlParameter>() { userIDParam, requestXmlValueParam };
                userAddressResult = _unitOfWork.EnsureInTransaction<Response<UserAddressModel>>(userAddressRepository.ExecuteNQStoredProc, "usp_AddUserAddresses", addressParameters, forceRollback: addressesToAdd.Any(x => x.ForceRollback.GetValueOrDefault(false)));

                if (userAddressResult.ResultCode != 0)
                {
                    return userAddressResult;
                }
            }

            if (addressesToUpdate.Count > 0)
            {
                var requestXmlValueParam = new SqlParameter("AddressXML", AddressesToXml(addressesToUpdate));
                var addressParameters = new List<SqlParameter>() { requestXmlValueParam };
                userAddressResult = _unitOfWork.EnsureInTransaction<Response<UserAddressModel>>(userAddressRepository.ExecuteNQStoredProc, "usp_UpdateUserAddresses", addressParameters, forceRollback: addressesToUpdate.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            return userAddressResult;
        }

        #endregion

        #endregion exposed functionality
    }

}
