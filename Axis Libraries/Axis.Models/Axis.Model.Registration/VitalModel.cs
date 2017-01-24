using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.Model.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class VitalModel : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long VitalID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? ContactID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? HeightFeet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? HeightInches { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? WeightLbs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? WeightOz { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? BMI { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LMP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Vitals { get; set; }
  
        /// <summary>
        /// 
        /// </summary>
        public Int16? BPMethodID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? Systolic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? Diastolic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? OxygenSaturation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? RespiratoryRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? Pulse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Temperature { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int16? Glucose { get; set; }

    }
}
