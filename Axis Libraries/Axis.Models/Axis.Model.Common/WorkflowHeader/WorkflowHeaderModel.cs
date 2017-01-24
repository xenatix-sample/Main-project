using System;
namespace Axis.Model.Common.WorkflowHeader
{
    /// <summary>
    /// Represents Client Audit
    /// </summary>
    public class WorkflowHeaderModel : BaseEntity
    {
        
        public long? RecordHeaderID { get; set; }
        public string WorkflowDataKey { get; set; }
        
        public long? ContactID { get; set; }

        public long? MRN { get; set; }
        public string FirstName { get; set; }
        public string Middle { get; set; }
        public string LastName { get; set; }
        public int? SuffixID { get; set; }
        public DateTime? DOB { get; set; }
        public string MedicaidID { get; set; }
        public string SSN { get; set; }
        public long? OrganizationMappingID { get; set; }
        public long? SourceHeaderID { get; set; }
        public int? Age { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public int? PhoneTypeID { get; set; }
        public string Number { get; set; }
        public string Extension { get; set; }
        public int? AddressTypeID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public int? County { get; set; }
        public string StateProvince { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string ComplexName { get; set; }
        public string GateCode { get; set; }
      
    }
    
}
