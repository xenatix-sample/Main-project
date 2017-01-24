using System;
using System.Collections.Generic;

namespace Axis.Model.Clinical.Allergy
{
    public class ContactAllergyDetailsModel : ClinicalBaseModel
    {
        public ContactAllergyDetailsModel()
        {
            Symptoms = new List<int>();
        }

        public long ContactAllergyID { get; set; }
        public long ContactAllergyDetailID { get; set; }
        public int AllergyID { get; set; }
        public int SeverityID { get; set; }
        public List<int> Symptoms { get; set;}
    }
}