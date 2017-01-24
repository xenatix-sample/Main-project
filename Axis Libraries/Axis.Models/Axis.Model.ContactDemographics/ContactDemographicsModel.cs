using System.Collections.Generic;
using Axis.Model.Address;
using Axis.Model.Email;
using Axis.Model.Phone;

namespace Axis.Model.ContactDemographics
{
    public class ContactDemographicsModel
    {
        public int ContactID { get; set; }
        public string ContactType { get; set; }
        public string ClientType { get; set; }
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }
        public string Prefix { get; set; }
        public string DOB { get; set; }
        public string DOBStatus { get; set; }
        public string SSN { get; set; }
        public string SSNStatus { get; set; }
        public string ReferralSource { get; set; }
        public string Alias { get; set; }
        public string FullCodeDNR { get; set; }
        public string SmokingStatus { get; set; }
        public string DeceasedDate { get; set; }
        public string ContactMethod { get; set; }
        public string PreferredContactMethod { get; set; }
        public List<AddressModel> Addresses { get; set; }
        public List<EmailModel> Emails { get; set; }
        public List<PhoneModel> Phones { get; set; }

    }
}
