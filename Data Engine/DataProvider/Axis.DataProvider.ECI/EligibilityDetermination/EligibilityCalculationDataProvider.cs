using System;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace Axis.DataProvider.ECI.EligibilityDetermination
{
    public class EligibilityCalculationDataProvider : IEligibilityCalculationDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public EligibilityCalculationDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        public Response<EligibilityCalculationModel> GetEligibilityCalculations(long eligibilityID)
        {
            var eligibilityCalculationRepository = _unitOfWork.GetRepository<EligibilityCalculationModel>(SchemaName.ECI);

            SqlParameter eligibilityIDParam = new SqlParameter("EligibilityID", eligibilityID);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityIDParam };
            var eligibilityCalculationResult = eligibilityCalculationRepository.ExecuteStoredProc("usp_GetEligibilityCalculations", procParams);

            return eligibilityCalculationResult;
        }

        public Response<EligibilityCalculationModel> AddEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            var eligibilityCalculationRepository = _unitOfWork.GetRepository<EligibilityCalculationModel>(SchemaName.ECI);
            SqlParameter eligibilityCalculationsXMLParam = new SqlParameter("EligibilityCalculationsXML", GenerateCalculationXML(calculation));
            eligibilityCalculationsXMLParam.DbType = System.Data.DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", calculation.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityCalculationsXMLParam, modifiedOnParam };
            var eligibilityCalculationResult = _unitOfWork.EnsureInTransaction<Response<EligibilityCalculationModel>>(eligibilityCalculationRepository.ExecuteNQStoredProc, "usp_AddEligibilityCalculations", procParams, idResult: false, forceRollback: calculation.ForceRollback.GetValueOrDefault(false));

            return eligibilityCalculationResult;
        }

        public Response<EligibilityCalculationModel> UpdateEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            var eligibilityCalculationRepository = _unitOfWork.GetRepository<EligibilityCalculationModel>(SchemaName.ECI);
            SqlParameter eligibilityCalculationsXMLParam = new SqlParameter("EligibilityCalculationsXML", GenerateCalculationXML(calculation));
            eligibilityCalculationsXMLParam.DbType = System.Data.DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", calculation.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { eligibilityCalculationsXMLParam, modifiedOnParam };
            var eligibilityCalculationResult = _unitOfWork.EnsureInTransaction<Response<EligibilityCalculationModel>>(eligibilityCalculationRepository.ExecuteNQStoredProc, "usp_UpdateEligibilityCalculations", procParams, idResult: false, forceRollback: calculation.ForceRollback.GetValueOrDefault(false));

            return eligibilityCalculationResult;
        }

        #endregion

        #region Private Methods

        private string GenerateCalculationXML(EligibilityCalculationModel calculation)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (var XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("EligibilityCalculation");

                    if (calculation != null)
                    {
                        if (calculation.EligibilityCalculationID != null && calculation.EligibilityCalculationID > 0)
                            XWriter.WriteElementString("EligibilityCalculationID", calculation.EligibilityCalculationID.ToString());
                        ////The result of the expression is always 'true' since a value of type 'long' is never equal to 'null' of type 'long?'
                        //if (calculation.EligibilityID != null) //EligibilityID is primary key and hence can not be null - redundant calculation
                        XWriter.WriteElementString("EligibilityID", calculation.EligibilityID.ToString());
                        if (calculation.UseAdjustedAge != null)
                            XWriter.WriteElementString("UseAdjustedAge", calculation.UseAdjustedAge.ToString());
                        if (calculation.EligibilityDate != null)
                            XWriter.WriteElementString("EligibilityDate", calculation.EligibilityDate.ToString());
                        if (calculation.GestationalAge != null)
                            XWriter.WriteElementString("GestationalAge", calculation.GestationalAge.ToString());
                        if (calculation.SCRS != null)
                            XWriter.WriteElementString("SCRS", calculation.SCRS.ToString());
                        if (calculation.SCAE != null)
                            XWriter.WriteElementString("SCAE", calculation.SCAE.ToString());
                        if (calculation.PRRS != null)
                            XWriter.WriteElementString("PRRS", calculation.PRRS.ToString());
                        if (calculation.PRAE != null)
                            XWriter.WriteElementString("PRAE", calculation.PRAE.ToString());
                        if (calculation.AAE != null)
                            XWriter.WriteElementString("AAE", calculation.AAE.ToString());
                        if (calculation.AMD != null)
                            XWriter.WriteElementString("AMD", calculation.AMD.ToString());
                        if (calculation.APD != null)
                            XWriter.WriteElementString("APD", calculation.APD.ToString());
                        if (calculation.AIRS != null)
                            XWriter.WriteElementString("AIRS", calculation.AIRS.ToString());
                        if (calculation.AIAE != null)
                            XWriter.WriteElementString("AIAE", calculation.AIAE.ToString());
                        if (calculation.PIRS != null)
                            XWriter.WriteElementString("PIRS", calculation.PIRS.ToString());
                        if (calculation.PIAE != null)
                            XWriter.WriteElementString("PIAE", calculation.PIAE.ToString());
                        if (calculation.SRRS != null)
                            XWriter.WriteElementString("SRRS", calculation.SRRS.ToString());
                        if (calculation.SRAE != null)
                            XWriter.WriteElementString("SRAE", calculation.SRAE.ToString());
                        if (calculation.PersonalSocialAE != null)
                            XWriter.WriteElementString("PersonalSocialAE", calculation.PersonalSocialAE.ToString());
                        if (calculation.PersonalSocialMD != null)
                            XWriter.WriteElementString("PersonalSocialMD", calculation.PersonalSocialMD.ToString());
                        if (calculation.PersonalSocialPD != null)
                            XWriter.WriteElementString("PersonalSocialPD", calculation.PersonalSocialPD.ToString());
                        if (calculation.RCRS != null)
                            XWriter.WriteElementString("RCRS", calculation.RCRS.ToString());
                        if (calculation.ECRS != null)
                            XWriter.WriteElementString("ECRS", calculation.ECRS.ToString());
                        if (calculation.RCAE != null)
                            XWriter.WriteElementString("RCAE", calculation.RCAE.ToString());
                        if (calculation.ECAE != null)
                            XWriter.WriteElementString("ECAE", calculation.ECAE.ToString());
                        if (calculation.ECMD != null)
                            XWriter.WriteElementString("ECMD", calculation.ECMD.ToString());
                        if (calculation.ECPD != null)
                            XWriter.WriteElementString("ECPD", calculation.ECPD.ToString());
                        if (calculation.CommAE != null)
                            XWriter.WriteElementString("CommAE", calculation.CommAE.ToString());
                        if (calculation.CommMD != null)
                            XWriter.WriteElementString("CommMD", calculation.CommMD.ToString());
                        if (calculation.CommPD != null)
                            XWriter.WriteElementString("CommPD", calculation.CommPD.ToString());
                        if (calculation.GMRS != null)
                            XWriter.WriteElementString("GMRS", calculation.GMRS.ToString());
                        if (calculation.GMAE != null)
                            XWriter.WriteElementString("GMAE", calculation.GMAE.ToString());
                        if (calculation.GMD != null)
                            XWriter.WriteElementString("GMD", calculation.GMD.ToString());
                        if (calculation.GMPD != null)
                            XWriter.WriteElementString("GMPD", calculation.GMPD.ToString());
                        if (calculation.FMRS != null)
                            XWriter.WriteElementString("FMRS", calculation.FMRS.ToString());
                        if (calculation.FMAE != null)
                            XWriter.WriteElementString("FMAE", calculation.FMAE.ToString());
                        if (calculation.PMRS != null)
                            XWriter.WriteElementString("PMRS", calculation.PMRS.ToString());
                        if (calculation.PMAE != null)
                            XWriter.WriteElementString("PMAE", calculation.PMAE.ToString());
                        if (calculation.FPMAE != null)
                            XWriter.WriteElementString("FPMAE", calculation.FPMAE.ToString());
                        if (calculation.FPMD != null)
                            XWriter.WriteElementString("FPMD", calculation.FPMD.ToString());
                        if (calculation.FPMPD != null)
                            XWriter.WriteElementString("FPMPD", calculation.FPMPD.ToString());
                        if (calculation.AMRS != null)
                            XWriter.WriteElementString("AMRS", calculation.AMRS.ToString());
                        if (calculation.AMAE != null)
                            XWriter.WriteElementString("AMAE", calculation.AMAE.ToString());
                        if (calculation.RARS != null)
                            XWriter.WriteElementString("RARS", calculation.RARS.ToString());
                        if (calculation.RAAE != null)
                            XWriter.WriteElementString("RAAE", calculation.RAAE.ToString());
                        if (calculation.PCRS != null)
                            XWriter.WriteElementString("PCRS", calculation.PCRS.ToString());
                        if (calculation.PCAE != null)
                            XWriter.WriteElementString("PCAE", calculation.PCAE.ToString());
                        if (calculation.CognitiveAE != null)
                            XWriter.WriteElementString("CognitiveAE", calculation.CognitiveAE.ToString());
                        if (calculation.CD != null)
                            XWriter.WriteElementString("CD", calculation.CD.ToString());
                        if (calculation.CPD != null)
                            XWriter.WriteElementString("CPD", calculation.CPD.ToString());
                    }

                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        #endregion
    }
}
