using Axis.Data.Repository;
using Axis.DataProvider.Common;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Registration.Common;

namespace Axis.DataProvider.Registration
{
    public class QuickRegistrationDataProvider : IQuickRegistrationDataProvider
    {
        #region initializations

        IUnitOfWork _unitOfWork;
        IRegistrationTypeDataProvider _registrationTypeDataProvider;
        IAddressTypeDataProvider _addressTypeDataProvider;
        ICountyDataProvider _countyDataProvider;
        IStateProvinceDataProvider _stateProvinceDataProvider;
        IGenderDataProvider _genderDataProvider;
        IContactAddressDataProvider _addressDataProvider;
        IContactEmailDataProvider _emailDataProvider;
        IContactPhoneDataProvider _phoneDataProvider;



        public QuickRegistrationDataProvider(IUnitOfWork unitOfWork, IRegistrationTypeDataProvider registrationTypeDataProvider, IAddressTypeDataProvider addressTypeDataProvider,
            ICountyDataProvider countyDataProvider, IStateProvinceDataProvider stateProvinceDataProvider, IGenderDataProvider genderDataProvider, IContactAddressDataProvider addressDataProvider,
            IContactEmailDataProvider emailDataProvider, IContactPhoneDataProvider phoneDataProvider)
        {
            _unitOfWork = unitOfWork;
            _registrationTypeDataProvider = registrationTypeDataProvider;
            _addressTypeDataProvider = addressTypeDataProvider;
            _countyDataProvider = countyDataProvider;
            _stateProvinceDataProvider = stateProvinceDataProvider;
            _addressDataProvider = addressDataProvider;
            _emailDataProvider = emailDataProvider;
            _phoneDataProvider = phoneDataProvider;            
            _genderDataProvider = genderDataProvider;
        }

        public QuickRegistrationDataProvider(string token)
        {
            
        }
        #endregion initializations

        public Response<QuickRegistrationModel> GetQuickRegistration()
        {
            var quickRegistrationRepository = _unitOfWork.GetRepository<QuickRegistrationModel>(SchemaName.Registration);
            var quickRegistrationResults = quickRegistrationRepository.ExecuteStoredProc("usp_CreateMrnMpi");

            if (quickRegistrationResults == null || quickRegistrationResults.ResultCode != 0) return quickRegistrationResults;

            //RegistrationType
            var registrationTypeResults = _registrationTypeDataProvider.GetRegistrationType();

            if (registrationTypeResults.ResultCode != 0)
            {
                quickRegistrationResults.ResultCode = registrationTypeResults.ResultCode;
                quickRegistrationResults.ResultMessage = registrationTypeResults.ResultMessage;
                return quickRegistrationResults;
            }
            quickRegistrationResults.DataItems.FirstOrDefault().RegistrationType = registrationTypeResults.DataItems;

            //AddressType
            var addressTypeResults = _addressTypeDataProvider.GetAddressTypes();

            if (addressTypeResults.ResultCode != 0)
            {
                quickRegistrationResults.ResultCode = addressTypeResults.ResultCode;
                quickRegistrationResults.ResultMessage = addressTypeResults.ResultMessage;
                return quickRegistrationResults;
            }
            quickRegistrationResults.DataItems.FirstOrDefault().AddressType = addressTypeResults.DataItems;

            //County
            var countyResults = _countyDataProvider.GetCounty();

            if (countyResults.ResultCode != 0)
            {
                quickRegistrationResults.ResultCode = countyResults.ResultCode;
                quickRegistrationResults.ResultMessage = countyResults.ResultMessage;
                return quickRegistrationResults;
            }
            quickRegistrationResults.DataItems.FirstOrDefault().County = countyResults.DataItems;

            //State
            var stateProvinceResults = _stateProvinceDataProvider.GetStateProvince();

            if (stateProvinceResults.ResultCode != 0)
            {
                quickRegistrationResults.ResultCode = stateProvinceResults.ResultCode;
                quickRegistrationResults.ResultMessage = stateProvinceResults.ResultMessage;
                return quickRegistrationResults;
            }
            quickRegistrationResults.DataItems.FirstOrDefault().StateProvince = stateProvinceResults.DataItems;

            //Gender
            var genderResults = _genderDataProvider.GetGenders();

            if (genderResults.ResultCode != 0)
            {
                quickRegistrationResults.ResultCode = genderResults.ResultCode;
                quickRegistrationResults.ResultMessage = genderResults.ResultMessage;
                return quickRegistrationResults;
            }
            quickRegistrationResults.DataItems.FirstOrDefault().Gender = genderResults.DataItems;

            return quickRegistrationResults;
        }

        public Response<QuickRegistrationModel> Add(QuickRegistrationModel contact)
        {
            var contactDemographicsParameters = BuildQuickRegistrationSpParams(contact);
            var contactDemographicsRepository = _unitOfWork.GetRepository<QuickRegistrationModel>(SchemaName.Registration);
            var spResults = contactDemographicsRepository.ExecuteNQStoredProc("usp_AddQuickRegistration", contactDemographicsParameters);

            if (spResults.ResultCode != 0) return spResults;


            if (contact.Addresses.Count != 0)
            {
                foreach (ContactAddressModel ad in contact.Addresses)
                {
                    ad.ContactID = 1;//spResults.DataItems[0].ContactID;
                }
                var addressResults = _addressDataProvider.AddAddresses(1, contact.Addresses);

                if (addressResults.ResultCode != 0)
                {
                    spResults.ResultCode = addressResults.ResultCode;
                    spResults.ResultMessage = addressResults.ResultMessage;
                    return spResults;
                }
            }
            if (contact.Emails.Count != 0)
            {
                foreach (ContactEmailModel em in contact.Emails)
                {
                    em.ContactID = 1;//spResults.DataItems[0].ContactID;
                }
                var emailResults = _emailDataProvider.AddEmails(1, contact.Emails);

                if (emailResults.ResultCode != 0)
                {
                    spResults.ResultCode = emailResults.ResultCode;
                    spResults.ResultMessage = emailResults.ResultMessage;
                    return spResults;
                }
            }
            if (contact.Phones.Count != 0)
            {
                foreach (ContactPhoneModel ph in contact.Phones)
                {
                    ph.ContactID = 1; //spResults.DataItems[0].ContactID;
                }
                var phoneResults = _phoneDataProvider.AddPhones(1, contact.Phones);

                //if (phoneResults.ResultCode == 0) return spResults;
                if (phoneResults.ResultCode != 0)
                {
                    spResults.ResultCode = phoneResults.ResultCode;
                    spResults.ResultMessage = phoneResults.ResultMessage;
                    return spResults;
                }
            }
            return spResults;
        }

        public Response<QuickRegistrationModel> GetMrnMpi()
        {
            var contactDemographicsRepository = _unitOfWork.GetRepository<QuickRegistrationModel>(SchemaName.Registration);
            var spResults = contactDemographicsRepository.ExecuteStoredProc("usp_CreateMrnMpi");
            return spResults;
        }

        #region Helpers

        private static List<SqlParameter> BuildQuickRegistrationSpParams(QuickRegistrationModel contact)
        {
            var spParameters = new List<SqlParameter>
            {               
                new SqlParameter("RegistrationTypeID", (object)contact.RegistrationTypeID ?? DBNull.Value),
                new SqlParameter("First", (object)contact.First ?? DBNull.Value),
                new SqlParameter("Middle", (object)contact.Middle ?? DBNull.Value),
                new SqlParameter("Last", (object)contact.Last ?? DBNull.Value),
                new SqlParameter("DOB", (object)contact.DOB ?? DBNull.Value),
                new SqlParameter("Age", (object)contact.Age ?? DBNull.Value),
                new SqlParameter("AnnualIncome", (object)contact.AnnualIncome ?? DBNull.Value),
                new SqlParameter("FamilySize", (object)contact.FamilySize ?? DBNull.Value),
                new SqlParameter("GenderID", (object)contact.GenderID ?? DBNull.Value),
                new SqlParameter("SSN", (object)contact.SSN ?? DBNull.Value),
                new SqlParameter("MrnValue", (object)contact.MrnValue ?? DBNull.Value),
                new SqlParameter("MpiValue", (object)contact.MpiValue ?? DBNull.Value),
                new SqlParameter("ModifiedOn", contact.ModifiedOn ?? DateTime.Now)
            };

            return spParameters;
        }

        #endregion Helpers
    }
}
