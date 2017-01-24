using System;

namespace Axis.Plugins.Clinical.Models.ChiefComplaint
{
    public class ChiefComplaintViewModel :  ClinicalBaseViewModel
    {
        public long ChiefComplaintID { get; set; }
        public new long ContactID { get; set; }
        public DateTime TakenDate { get; set; }
        public string Complaint { get; set; }
    }
}
