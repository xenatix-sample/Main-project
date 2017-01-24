using System;
using System.Collections.Generic;

namespace Axis.Plugins.Clinical.Models.Allergy
{
    public class ContactAllergyViewModel : ClinicalBaseViewModel
    {
        public long ContactAllergyID { get; set; }
        public Int16 AllergyTypeID { get; set; } 
        public bool NoKnownAllergy { get; set; }
        public bool ReviewedNoChanges { get; set; }
    }
}
