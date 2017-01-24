using System.Collections.Generic;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Clinical.Models.PresentIllness
{
    public class PresentIllnessDetailViewModel : ClinicalBaseViewModel
    {
        public long HPIID { get; set; }
        public long HPIDetailID { get; set; }
        public string Comment { get; set; }
        public string Location { get; set; }
        public string Quality { get; set; }
        public short? HPISeverityID { get; set; } 
        public string Duration { get; set; }
        public string Timing { get; set; }
        public string Context { get; set; }
        public string Modifyingfactors { get; set; }
        public string Symptoms { get; set; }
        public string Conditions { get; set; }
        
       }
}