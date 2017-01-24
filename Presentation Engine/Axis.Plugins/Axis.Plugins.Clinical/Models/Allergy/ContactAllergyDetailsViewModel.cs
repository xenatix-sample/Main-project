using System.Collections.Generic;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Clinical.Models.Allergy
{
    public class ContactAllergyDetailsViewModel : ClinicalBaseViewModel
    {
        public ContactAllergyDetailsViewModel()
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
