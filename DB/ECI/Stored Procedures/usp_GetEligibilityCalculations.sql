----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_GetEligibilityCalculations]
-- Author:		Sumana Sangapu
-- Date:		10/26/2015
--
-- Purpose:		Get the Eligibility Calculations
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/26/2015	-	Sumana Sangapu	- Task 3015	- Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetEligibilityCalculations]
	@EligibilityID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 
AS
BEGIN
		SELECT @ResultCode = 0,
			   @ResultMessage = 'Executed successfully'
		
		BEGIN TRY

				SELECT	[EligibilityCalculationID],
						[EligibilityID],
						[UseAdjustedAge],
						[TestingDate] AS EligibilityDate,
						[GestationAge] AS GestationalAge, 
						[SCRawScore] AS SCRS,
						[SCAEMonths] AS SCAE,
						[PRRawScore] AS PRRS,
						[PRAEMonths] AS PRAE,
						[AdpAE] AS AAE,
						[AdpMonthsDelay] AS AMD,
						[AdpPCTDelay] AS APD,
						[AIRawScore] AS AIRS,
						[AIAEMonths] AS AIAE ,
						[PIRawScore] AS PIRS,
						[PIAEMonths] AS PIAE,
						[SRRawScore] AS SRRS,
						[SRAEMonths] AS SRAE,
						[PSAE] AS PersonalSocialAE,
						[PSMonthsDelay] AS PersonalSocialMD,
						[PSPCTDelay] AS PersonalSocialPD,
						[RCRawScore] AS RCRS,
						[ECRawScore] AS ECRS,
						[RCAEMonths] AS RCAE,
						[ECAEMonths] AS ECAE,
						[ECMonths] AS ECMD,
						[ECPCTDelay] AS ECPD,
						[CommAE],
						[CommMonthsDelay] AS CommMD,
						[CommPCTDelay] AS CommPD,
						[GMRawScore] AS GMRS,
						[GMAE] AS GMAE,
						[GMMonthsDelay] AS GMD,
						[GMPCTDelay] GMPD,
						[FMRawScore] AS FMRS,
						[FMAEMonths] AS FMAE,
						[PMRawScore] AS PMRS,
						[PMAEMonths] AS PMAE,
						[FMPMAE] AS FPMAE,
						[FMPMMonthsDelay] AS FPMD,
						[FMPMPCTDelay] FPMPD,
						[AMRS] ,
						[AMAEMonths] AS AMAE,
						[RARawScore] AS RARS,
						[RAAEMonths] AS RAAE,
						[PCRawScore] AS PCRS,
						[PCAEMonths] AS PCAE,
						[CogAE] AS CognitiveAE,
						[CogMonthsDelay] AS CD,
						[CogPCTDelay] AS CPD
				FROM	ECI.EligibilityCalculations 
				WHERE	EligibilityID = @EligibilityID--EligibilityCalculationID = @EligibilityCalculationsID

		END TRY
		BEGIN CATCH
				SELECT  @ResultCode = ERROR_SEVERITY(),
						@ResultMessage = ERROR_MESSAGE()
		END CATCH

END