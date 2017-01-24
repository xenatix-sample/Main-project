using System;

namespace Axis.Model.ECI.EligibilityDetermination
{
    public class EligibilityCalculationModel : ECIBaseModel
    {
        public long? EligibilityCalculationID { get; set; }
        public long EligibilityID { get; set; }
        public bool? UseAdjustedAge { get; set; }
        public DateTime? EligibilityDate { get; set; }
        public decimal? GestationalAge { get; set; }//
        public decimal? SCRS { get; set; }
        public decimal? SCAE { get; set; }
        public decimal? PRRS { get; set; }
        public decimal? PRAE { get; set; }
        public decimal? AAE { get; set; }
        public decimal? AMD { get; set; }
        public decimal? APD { get; set; }
        public decimal? AIRS { get; set; }
        public decimal? AIAE { get; set; }
        public decimal? PIRS { get; set; }
        public decimal? PIAE { get; set; }
        public decimal? SRRS { get; set; }
        public decimal? SRAE { get; set; }
        public decimal? PersonalSocialAE { get; set; }
        public decimal? PersonalSocialMD { get; set; }
        public decimal? PersonalSocialPD { get; set; }
        public decimal? RCRS { get; set; }
        public decimal? ECRS { get; set; }
        public decimal? RCAE { get; set; }
        public decimal? ECAE { get; set; }
        public decimal? ECMD { get; set; }
        public decimal? ECPD { get; set; }
        public decimal? CommAE { get; set; }
        public decimal? CommMD { get; set; }
        public decimal? CommPD { get; set; }
        public decimal? GMRS { get; set; }
        public decimal? GMAE { get; set; }
        public decimal? GMD { get; set; }
        public decimal? GMPD { get; set; }
        public decimal? FMRS { get; set; }
        public decimal? FMAE { get; set; }
        public decimal? PMRS { get; set; }
        public decimal? PMAE { get; set; }
        public decimal? FPMAE { get; set; }
        public decimal? FPMD { get; set; }
        public decimal? FPMPD { get; set; }
        public decimal? AMRS { get; set; }
        public decimal? AMAE { get; set; }
        public decimal? RARS { get; set; }
        public decimal? RAAE { get; set; }
        public decimal? PCRS { get; set; }
        public decimal? PCAE { get; set; }
        public decimal? CognitiveAE { get; set; }
        public decimal? CD { get; set; }
        public decimal? CPD { get; set; }
    }
}
