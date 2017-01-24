using System;
using System.Collections.Generic;
using Axis.Model.Address;
using Axis.Model.Common;
using Axis.Model.Email;
using Axis.Model.Phone;

namespace Axis.Model.Registration
{
    public class QuickRegistrationModel : BaseEntity
    {
        public QuickRegistrationModel()
        {
            RegistrationType = new List<RegistrationTypeModel>();
            AddressType = new List<AddressTypeModel>();
            County = new List<CountyModel>();
            StateProvince = new List<StateProvinceModel>();
            Gender = new List<GenderModel>();
            Addresses = new List<ContactAddressModel>();
            Emails = new List<ContactEmailModel>();
            Phones = new List<ContactPhoneModel>();
        }

        public int RegistrationTypeID { get; set; }
        public long ContactID { get; set; }
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public int? GenderID { get; set; }
        public DateTime? DOB { get; set; }
        public string SSN { get; set; }
        public int Age { get; set; }
        public int AnnualIncome { get; set; }
        public int FamilySize { get; set; }
        public string MrnValue { get; set; }
        public string MpiValue { get; set; }

        //for Dropdown fill
        public List<RegistrationTypeModel> RegistrationType {get;set;}
        public List<AddressTypeModel> AddressType { get; set; }
        public List<CountyModel> County { get; set; }
        public List<StateProvinceModel> StateProvince { get; set; }
        public List<GenderModel> Gender { get; set; }

        //fop save collection data
        public List<ContactAddressModel> Addresses { get; set; }
        public List<ContactEmailModel> Emails { get; set; }
        public List<ContactPhoneModel> Phones { get; set; }
    }
}
