using Axis.Model.Common;
using System;

namespace Axis.Model.Clinical
{
    public class ClinicalBaseModel : BaseEntity
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
