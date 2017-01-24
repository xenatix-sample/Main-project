using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.Plugins.ECI.Models.EligibilityDetermination;

namespace Axis.Plugins.ECI.Translator.EligibilityDetermination
{
    public static class EligibilityCalculationTranslator
    {
        public static EligibilityCalculationViewModel ToModel(this EligibilityCalculationModel entity)
        {
            if (entity == null)
                return null;

            var model = new EligibilityCalculationViewModel
            {
                 EligibilityCalculationID = entity.EligibilityCalculationID,
                 EligibilityID = entity.EligibilityID,
                 UseAdjustedAge = entity.UseAdjustedAge,
                 EligibilityDate = entity.EligibilityDate,
                 GestationalAge = entity.GestationalAge,
                 SCRS = entity.SCRS,
                 SCAE = entity.SCAE,
                 PRRS = entity.PRRS,
                 PRAE = entity.PRAE,
                 AAE = entity.AAE,
                 AMD = entity.AMD,
                 APD = entity.APD,
                 AIRS = entity.AIRS,
                 AIAE = entity.AIAE,
                 PIRS = entity.PIRS,
                 PIAE = entity.PIAE,
                 SRRS = entity.SRRS,
                 SRAE = entity.SRAE,
                 PersonalSocialAE = entity.PersonalSocialAE,
                 PersonalSocialMD = entity.PersonalSocialMD,
                 PersonalSocialPD = entity.PersonalSocialPD,
                 RCRS = entity.RCRS,
                 ECRS = entity.ECRS,
                 RCAE = entity.RCAE,
                 ECAE = entity.ECAE,
                 ECMD = entity.ECMD,
                 ECPD = entity.ECPD,
                 CommAE = entity.CommAE,
                 CommMD = entity.CommMD,
                 CommPD = entity.CommPD,
                 GMRS = entity.GMRS,
                 GMAE = entity.GMAE,
                 GMD = entity.GMD,
                 GMPD = entity.GMPD,
                 FMRS = entity.FMRS,
                 FMAE = entity.FMAE,
                 PMRS = entity.PMRS,
                 PMAE = entity.PMAE,
                 FPMAE = entity.FPMAE,
                 FPMD = entity.FPMD,
                 FPMPD = entity.FPMPD,
                 AMRS = entity.AMRS,
                 AMAE = entity.AMAE,
                 RARS = entity.RARS,
                 RAAE = entity.RAAE,
                 PCRS = entity.PCRS,
                 PCAE = entity.PCAE,
                 CognitiveAE = entity.CognitiveAE,
                 CD = entity.CD,
                 CPD = entity.CPD,
                 ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static EligibilityCalculationModel ToModel(this EligibilityCalculationViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new EligibilityCalculationModel
            {
                EligibilityCalculationID = entity.EligibilityCalculationID,
                EligibilityID = entity.EligibilityID,
                UseAdjustedAge = entity.UseAdjustedAge,
                EligibilityDate = entity.EligibilityDate,
                GestationalAge = entity.GestationalAge,
                SCRS = entity.SCRS,
                SCAE = entity.SCAE,
                PRRS = entity.PRRS,
                PRAE = entity.PRAE,
                AAE = entity.AAE,
                AMD = entity.AMD,
                APD = entity.APD,
                AIRS = entity.AIRS,
                AIAE = entity.AIAE,
                PIRS = entity.PIRS,
                PIAE = entity.PIAE,
                SRRS = entity.SRRS,
                SRAE = entity.SRAE,
                PersonalSocialAE = entity.PersonalSocialAE,
                PersonalSocialMD = entity.PersonalSocialMD,
                PersonalSocialPD = entity.PersonalSocialPD,
                RCRS = entity.RCRS,
                ECRS = entity.ECRS,
                RCAE = entity.RCAE,
                ECAE = entity.ECAE,
                ECMD = entity.ECMD,
                ECPD = entity.ECPD,
                CommAE = entity.CommAE,
                CommMD = entity.CommMD,
                CommPD = entity.CommPD,
                GMRS = entity.GMRS,
                GMAE = entity.GMAE,
                GMD = entity.GMD,
                GMPD = entity.GMPD,
                FMRS = entity.FMRS,
                FMAE = entity.FMAE,
                PMRS = entity.PMRS,
                PMAE = entity.PMAE,
                FPMAE = entity.FPMAE,
                FPMD = entity.FPMD,
                FPMPD = entity.FPMPD,
                AMRS = entity.AMRS,
                AMAE = entity.AMAE,
                RARS = entity.RARS,
                RAAE = entity.RAAE,
                PCRS = entity.PCRS,
                PCAE = entity.PCAE,
                CognitiveAE = entity.CognitiveAE,
                CD = entity.CD,
                CPD = entity.CPD,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<EligibilityCalculationViewModel> ToModel(this Response<EligibilityCalculationModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<EligibilityCalculationViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(EligibilityCalculationModel eligibilityCalculationModel)
                {
                    var transformedModel = eligibilityCalculationModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<EligibilityCalculationViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }
    }
}
