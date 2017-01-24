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
    public class ReferralPhoneDataProvider : IReferralPhoneDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhoneDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralPhoneDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality


        /// <summary>
        /// Gets the phones.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> GetPhones(long referralID, int? contactTypeID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ReferralID", referralID) };
            //if (contactTypeID.HasValue)
            //    spParameters.Add(new SqlParameter("ContactTypeID", contactTypeID));
            //else
            //    spParameters.Add(new SqlParameter("ContactTypeID", ContactType.Contact));

            var repository = _unitOfWork.GetRepository<ReferralPhoneModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetReferralPhones", spParameters);

            return results;
        }


        /// <summary>
        /// Adds the phones.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="phones">The phones.</param>
        /// <returns></returns>
        Response<ReferralPhoneModel> AddPhones(long referralID, List<ReferralPhoneModel> phones)
        {
            var repository = _unitOfWork.GetRepository<ReferralPhoneModel>(SchemaName.Registration);

            var ReferralID = new SqlParameter("ReferralID", referralID);
            var requestXMLValueParam = new SqlParameter("PhonesXML", PhonesToXML(phones));
            var spParameters = new List<SqlParameter>() { ReferralID, requestXMLValueParam };
            return _unitOfWork.EnsureInTransaction<Response<ReferralPhoneModel>>(repository.ExecuteNQStoredProc, "usp_AddReferralPhones", spParameters, forceRollback: phones.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }


        /// <summary>
        /// Phoneses to XML.
        /// </summary>
        /// <param name="phones">The phones.</param>
        /// <returns></returns>
        public string PhonesToXML(List<ReferralPhoneModel> phones)
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
                        new XElement("IsActive", phone.IsActive),
                        new XElement("ReferralPhoneID", phone.ReferralPhoneID),
                        new XElement("ModifiedOn", phone.ModifiedOn ?? DateTime.Now)
                )
            );

            return xEle.ToString();
        }


        /// <summary>
        /// Add and update phones.
        /// </summary>
        /// <param name="phones">The phones.</param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> AddUpdatePhones(List<ReferralPhoneModel> phones)
        {
            long referralID = 0;
            if (phones.Count > 0)
                referralID = phones[0].ReferralID;
            var phoneResult = new Response<ReferralPhoneModel>();
            phoneResult.ResultCode = 0;

            // Prepare phone collection to add/update
            var phonesToAdd = phones.Where(phone => phone.PhoneID == 0).ToList();
            var phonesToUpdate = phones.Where(phone => phone.PhoneID != 0).ToList();
            if (phonesToAdd.Count() > 0)
            {
                phoneResult = AddPhones(referralID, phonesToAdd);
                if (phoneResult.ResultCode != 0)
                {
                    return phoneResult;
                }
            }

            if (phonesToUpdate.Count > 0)
            {
                var requestXMLValueParam = new SqlParameter("PhoneXML", PhonesToXML(phones));
                var spParameters = new List<SqlParameter>() { requestXMLValueParam };
                var repository = _unitOfWork.GetRepository<ReferralPhoneModel>(SchemaName.Registration);
                phoneResult = _unitOfWork.EnsureInTransaction<Response<ReferralPhoneModel>>(repository.ExecuteNQStoredProc, "usp_UpdateReferralPhones", spParameters, forceRollback: phones.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            return phoneResult;
        }


        /// <summary>
        /// Deletes the referral phone.
        /// </summary>
        /// <param name="referralPhoneID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> DeleteReferralPhone(long referralPhoneID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ReferralPhoneID", referralPhoneID), new SqlParameter("ModifiedOn", modifiedOn) };
            var referralPhoneRepository = _unitOfWork.GetRepository<ReferralPhoneModel>(SchemaName.Registration);
            Response<ReferralPhoneModel> spResults = new Response<ReferralPhoneModel>();
            spResults = referralPhoneRepository.ExecuteNQStoredProc("usp_DeleteReferralPhone", procsParameters);
            return spResults;
        }


        #endregion exposed functionality
    }
}
