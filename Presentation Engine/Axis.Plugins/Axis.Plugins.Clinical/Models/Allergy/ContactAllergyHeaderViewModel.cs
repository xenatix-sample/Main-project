using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical.Models.Allergy
{
    public class ContactAllergyHeaderViewModel : ClinicalBaseViewModel
    {
        public long ContactAllergyID { get; set; }
        public long ContactAllergyDetailID { get; set; }
        public int AllergyID { get; set; }
        public string AllergyName { get; set; }
        public int AllergySeverityID { get; set; }
        public string AllergySeverity { get; set; }
        public int SortOrder { get; set; }
    }
}
