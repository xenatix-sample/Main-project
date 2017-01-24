using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.Clinical.Models
{
    public class ClinicalBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// ContactID
        /// </summary>
        public long ContactID { get; set; }
        /// <summary>
        /// EncounterID
        /// </summary>
        public long? EncounterID { get; set; }
        /// <summary>
        /// TakenBy
        /// </summary>
        public int TakenBy { get; set; }
        /// <summary>
        /// TakenTime
        /// </summary>
        public DateTime TakenTime { get; set; }
    }
}
