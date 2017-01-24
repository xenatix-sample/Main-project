using System;
using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.Model.ECI
{
    public class EligibilityDeterminationModel : ECIBaseModel
    {
        public EligibilityDeterminationModel()
        {
            Members = new List<int>();
        }

        public long? EligibilityID { get; set; } 
        public DateTime EligibilityDate { get; set; }
        public int EligibilityTypeID { get; set; }
        public string EligibilityType { get; set; }
        public int EligibilityDurationID { get; set; }
        public int EligibilityCategoryID { get; set; }
        public string EligibilityCategory { get; set; }
        public int AdjustedAge { get; set; }
        public DateTime? DOB { get; set; }
        public string Notes { get; set; }
        public List<int> Members { get; set; }
    }
}
