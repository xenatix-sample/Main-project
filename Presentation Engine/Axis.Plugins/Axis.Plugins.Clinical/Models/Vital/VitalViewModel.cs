using System;

namespace Axis.Plugins.Clinical.Models.Vital
{
    /// <summary>
    /// ViewModel for Vital
    /// </summary>
    public class VitalViewModel : ClinicalBaseViewModel
    {
        /// <summary>
        /// VitalID
        /// </summary>
        public long VitalID { get; set; }

        /// <summary>
        /// HeightFeet
        /// </summary>
        public Int16? HeightFeet { get; set; }

        /// <summary>
        /// HeightInches
        /// </summary>
        public Int16? HeightInches { get; set; }

        /// <summary>
        /// WeightLbs
        /// </summary>
        public Int16? WeightLbs { get; set; }

        /// <summary>
        /// WeightOz
        /// </summary>
        public Int16? WeightOz { get; set; }

        /// <summary>
        /// BMI
        /// </summary>
        public decimal? BMI { get; set; }

        /// <summary>
        /// LMP
        /// </summary>
        public DateTime? LMP { get; set; }

        /// <summary>
        /// BPMethodID
        /// </summary>
        public Int16? BPMethodID { get; set; }

        /// <summary>
        /// Systolic
        /// </summary>
        public Int16? Systolic { get; set; }

        /// <summary>
        /// Diastolic
        /// </summary>
        public Int16? Diastolic { get; set; }

        /// <summary>
        /// OxygenSaturation
        /// </summary>
        public Int16? OxygenSaturation { get; set; }

        /// <summary>
        /// RespiratoryRate
        /// </summary>
        public Int16? RespiratoryRate { get; set; }

        /// <summary>
        /// Pulse
        /// </summary>
        public Int16? Pulse { get; set; }

        /// <summary>
        /// Temperature
        /// </summary>
        public decimal? Temperature { get; set; }

        /// <summary>
        /// Glucose
        /// </summary>
        public Int16? Glucose { get; set; }

        /// <summary>
        /// Waist Circumference
        /// </summary>
        public Int16? WaistCircumference { get; set; }
    }
}
