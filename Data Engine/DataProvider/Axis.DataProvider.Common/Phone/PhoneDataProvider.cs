using System;
using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Phone;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using Axis.Data.Repository.Schema;
using Axis.Model.Registration;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Phone data provider
    /// </summary>
    public class PhoneDataProvider : IPhoneDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PhoneDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the phones.
        /// </summary>
        /// <param name="phoneID">The phone identifier.</param>
        /// <returns></returns>
        public Response<PhoneModel> GetPhones(long phoneID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("PhoneID", phoneID) };
            var repository = _unitOfWork.GetRepository<PhoneModel>();
            var results = repository.ExecuteStoredProc("usp_GetPhone", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the phones.
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        public Response<PhoneModel> AddPhones(PhoneModel phone)
        {
            var repository = _unitOfWork.GetRepository<PhoneModel>();

            var requestXMLValueParam = new SqlParameter("PhonesXML", PhonesToXML(new List<PhoneModel> { phone }));
            var spParameters = new List<SqlParameter>() { requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<PhoneModel>>(repository.ExecuteNQStoredProc, "usp_AddPhone", spParameters, forceRollback: phone.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Phoneses to XML.
        /// </summary>
        /// <param name="phones">The phones.</param>
        /// <returns></returns>
        public string PhonesToXML(List<PhoneModel> phones)
        {
            var xEle = new XElement("RequestXMLValue",
                from phone in phones
                select new XElement("Phone",
                               new XElement("PhoneID", phone.PhoneID),
                               new XElement("PhoneTypeID", phone.PhoneTypeID),
                               new XElement("Number", phone.Number),
                               new XElement("Extension", phone.Extension),
                               new XElement("ModifiedOn", phone.ModifiedOn ?? DateTime.Now)
                           ));

            return xEle.ToString();
        }

        /// <summary>
        /// Phones to XML.
        /// </summary>
        /// <param name="phones">The phones.</param>
        /// <returns></returns>
        public string PhonesToXml(List<UserPhoneModel> phones)
        {
            //TODO: PhoneTypeID is not have text field in UI
            var xEle = new XElement("RequestXMLValue",
                from phone in phones
                select new XElement("Phone",
                        new XElement("PhoneID", phone.PhoneID),
                        new XElement("UserPhoneID", phone.UserPhoneID),
                        new XElement("PhoneTypeID", phone.PhoneTypeID),
                        new XElement("Number", phone.Number),
                        new XElement("Extension", phone.Extension),
                        new XElement("PhonePermissionID", phone.PhonePermissionID),
                        new XElement("IsPrimary", phone.IsPrimary),
                        new XElement("ModifiedOn", phone.ModifiedOn ?? DateTime.Now)
                )
            );

            return xEle.ToString();
        }

        /// <summary>
        /// Updates the phones.
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        public Response<PhoneModel> UpdatePhones(PhoneModel phone)
        {
            var repository = _unitOfWork.GetRepository<PhoneModel>(SchemaName.Core);
            var requestXMLValueParam = new SqlParameter("PhonesXML", PhonesToXML(new List<PhoneModel> { phone }));
            var spParameters = new List<SqlParameter>() { requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<PhoneModel>>(repository.ExecuteNQStoredProc, "usp_UpdatePhone", spParameters, forceRollback: phone.ForceRollback.GetValueOrDefault(false));
        }

        #region UserPhone

        public Response<UserPhoneModel> GetUserPhones(int userID)
        {
            var userPhoneRepository = _unitOfWork.GetRepository<UserPhoneModel>();
            SqlParameter userIdParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIdParam };
            var userPhoneResult = userPhoneRepository.ExecuteStoredProc("usp_GetUserPhones", procParams);

            return userPhoneResult;
        }

        public Response<UserPhoneModel> SaveUserPhones(int userID, List<UserPhoneModel> phones)
        {
            var userPhoneRepository = _unitOfWork.GetRepository<UserPhoneModel>();
            var userPhoneResult = new Response<UserPhoneModel>() { ResultCode = 0 };

            var phonesToAdd = phones.Where(phone => phone.PhoneID == 0).ToList();
            var phonesToUpdate = phones.Where(phone => phone.PhoneID != 0).ToList();

            if (phonesToAdd.Count > 0)
            {
                SqlParameter userIDParam = new SqlParameter("UserID", userID);
                var requestXmlValueParam = new SqlParameter("PhoneXml", PhonesToXml(phonesToAdd));
                var phoneParameters = new List<SqlParameter>() { userIDParam, requestXmlValueParam };
                userPhoneResult = _unitOfWork.EnsureInTransaction<Response<UserPhoneModel>>(userPhoneRepository.ExecuteNQStoredProc, "usp_AddUserPhones", phoneParameters, forceRollback: phonesToAdd.Any(x => x.ForceRollback.GetValueOrDefault(false)));
                
                if (userPhoneResult.ResultCode != 0)
                {
                    return userPhoneResult;
                }
            }

            if (phonesToUpdate.Count > 0)
            {
                var requestXmlValueParam = new SqlParameter("PhoneXml", PhonesToXml(phonesToUpdate));
                var phoneParameters = new List<SqlParameter>() { requestXmlValueParam };
                userPhoneResult = _unitOfWork.EnsureInTransaction<Response<UserPhoneModel>>(userPhoneRepository.ExecuteNQStoredProc, "usp_UpdateUserPhones", phoneParameters, forceRollback: phonesToUpdate.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            return userPhoneResult;
        }

        #endregion

        #endregion exposed functionality
    }
}
