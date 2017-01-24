using System;
using System.Collections.Generic;

namespace Axis.Model.Clinical.Allergy
{
    public class ContactAllergyModel : ClinicalBaseModel
    {
        public long ContactAllergyID { get; set; }
        public Int16 AllergyTypeID { get; set; }
        public bool NoKnownAllergy { get; set; }
        public bool ReviewedNoChanges { get; set; }
    }
}