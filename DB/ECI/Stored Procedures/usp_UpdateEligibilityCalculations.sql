----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_AddEligibilityCalculations]
-- Author:		Sumana Sangapu
-- Date:		10/26/2015
--
-- Purpose:		Add the Eligibility Calculations
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/26/2015	Sumana Sangapu	- Task 3015	- Initial Creation
-- 11/24/2015	Scott Martin		TFS:3636	Added Audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_UpdateEligibilityCalculations]
	@EligibilityCalculationsXML XML,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 
AS
BEGIN
DECLARE @ID BIGINT,
		@AuditCursor CURSOR,
		@AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed successfully'
		
	BEGIN TRY
	SELECT
		T.C.value('EligibilityCalculationID[1]', '[BIGINT]') as EligibilityCalculationID,
		T.C.value('EligibilityID[1]','[bigint]') as EligibilityID,
		T.C.value('UseAdjustedAge[1]', '[bit]') as UseAdjustedAge,
		T.C.value('TestingDate[1]', '[date]') as TestingDate,
		T.C.value('GestationalAge[1]', '[decimal]') as GestationAge,
		T.C.value('SCRS[1]', '[decimal]') as SCRawScore,
		T.C.value('SCAE[1]', '[decimal]') as SCAEMonths,
		T.C.value('PRRS[1]', '[decimal]') as PRRawScore,
		T.C.value('PRAE[1]', '[decimal]') as PRAEMonths,
		T.C.value('AAE[1]', '[decimal]') as AdpAE,
		T.C.value('AMD[1]', '[decimal]') as AdpMonthsDelay,
		T.C.value('APD[1]', '[decimal]')as AdpPCTDelay,
		T.C.value('AIRS[1]', '[decimal]') as AIRawScore,
		T.C.value('AIAE[1]', '[decimal]') as AIAEMonths,
		T.C.value('PIRS[1]', '[decimal]') as PIRawScore,
		T.C.value('PIAE[1]', '[decimal]') as PIAEMonths,
		T.C.value('SRRS[1]', '[decimal]') as SRRawScore,
		T.C.value('SRAE[1]', '[decimal]') as SRAEMonths,
		T.C.value('PersonalSocialAE[1]', '[decimal]') as PSAE,
		T.C.value('PersonalSocialMD[1]', '[decimal]') as PSMonthsDelay,
		T.C.value('PersonalSocialPD[1]', '[decimal]' )as PSPCTDelay,
		T.C.value('RCRS[1]', '[decimal]') as RCRawScore,
		T.C.value('ECRS[1]', '[decimal]' )as ECRawScore,
		T.C.value('RCAE[1]', '[decimal]' )as RCAEMonths,
		T.C.value('ECAE[1]', '[decimal]') as ECAEMonths,
		T.C.value('ECMD[1]', '[decimal]') as ECMonths,
		T.C.value('ECPD[1]', '[decimal]') as ECPCTDelay,
		T.C.value('CommAE[1]', '[decimal]') as CommAE,
		T.C.value('CommMD[1]', '[decimal]') as CommMonthsDelay,
		T.C.value('CommPD[1]', '[decimal]') as CommPCTDelay,
		T.C.value('GMRS[1]', '[decimal]') as GMRawScore,
		T.C.value('GMAE[1]', '[decimal]') as GMAE,
		T.C.value('GMD[1]', '[decimal]') as GMMonthsDelay,--Was GMMD
		T.C.value('GMPD[1]', '[decimal]') as GMPCTDelay,
		T.C.value('FMRS[1]', '[decimal]') as FMRawScore,
		T.C.value('FMAE[1]', '[decimal]') as FMAEMonths,
		T.C.value('PMRS[1]', '[decimal]') as PMRawScore,
		T.C.value('PMAE[1]', '[decimal]') as PMAEMonths,
		T.C.value('FPMAE[1]', '[decimal]') as FMPMAE,
		T.C.value('FPMD[1]', '[decimal]') as FMPMMonthsDelay,
		T.C.value('FPMPD[1]', '[decimal]') as FMPMPCTDelay,
		T.C.value('AMRS[1]', '[decimal]') as AMRS,
		T.C.value('AMAE[1]', '[decimal]') as AMAEMonths,
		T.C.value('RARS[1]', '[decimal]') as RARawScore,
		T.C.value('RAAE[1]', '[decimal]') as RAAEMonths,
		T.C.value('PCRS[1]', '[decimal]') as PCRawScore,
		T.C.value('PCAE[1]', '[decimal]') as PCAEMonths,
		T.C.value('CognitiveAE[1]', '[decimal]') as CogAE,
		T.C.value('CD[1]', '[decimal]') as CogMonthsDelay,
		T.C.value('CPD[1]', '[decimal]') as CogPCTDelay,
		NULL AS AuditDetailID
	INTO #TmpXMLData
	FROM
		@EligibilityCalculationsXML.nodes('EligibilityCalculation') AS T(C)
		INNER JOIN [ECI].[EligibilityCalculations] ec
			ON ec.EligibilityCalculationID = T.C.value('EligibilityCalculationID[1]','Bigint');		

	IF EXISTS (SELECT TOP 1 * FROM #TmpXMLData WHERE EligibilityCalculationID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT EligibilityCalculationID FROM #TmpXMLData WHERE EligibilityCalculationID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'EligibilityCalculations', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE #TmpXMLData
		SET AuditDetailID = @AuditDetailID
		WHERE
			EligibilityCalculationID = @ID;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

	UPDATE  ec
	SET ec.[EligibilityID] = tx.[EligibilityID], 
		ec.[UseAdjustedAge] = tx.[UseAdjustedAge],
		ec.[TestingDate] = tx.[TestingDate],	
		ec.[GestationAge] = tx.[GestationAge], 
		ec.[SCRawScore]= tx.[SCRawScore] ,
		ec.[SCAEMonths] = tx.[SCAEMonths] ,
		ec.[PRRawScore] = tx.[PRRawScore] ,
		ec.[PRAEMonths] = tx.[PRAEMonths] ,
		ec.[AdpAE] = tx.[AdpAE],
		ec.[AdpMonthsDelay] = tx.[AdpMonthsDelay] ,
		ec.[AdpPCTDelay] =  tx.[AdpPCTDelay],
		ec.[AIRawScore] = tx.[AIRawScore],
		ec.[AIAEMonths] = tx.[AIAEMonths] ,
		ec.[PIRawScore] = tx.[PIRawScore],
		ec.[PIAEMonths] = tx.[PIAEMonths],
		ec.[SRRawScore] = tx.[SRRawScore],
		ec.[SRAEMonths] = tx.[SRAEMonths],
		ec.[PSAE] = tx.[PSAE],
		ec.[PSMonthsDelay] = tx.[PSMonthsDelay] ,
		ec.[PSPCTDelay] = tx.[PSPCTDelay] ,
		ec.[RCRawScore] = tx.[RCRawScore] ,
		ec.[ECRawScore]= tx.[ECRawScore],
		ec.[RCAEMonths] = tx.[RCAEMonths],	
		ec.[ECAEMonths] = tx.[ECAEMonths] ,
		ec.[ECMonths] = tx.[ECMonths],
		ec.[ECPCTDelay]=tx.[ECPCTDelay],
		ec.[CommAE] = tx.[CommAE], 
		ec.[CommMonthsDelay] =tx.[CommMonthsDelay],
		ec.[CommPCTDelay] = tx.[CommPCTDelay],
		ec.[GMRawScore]= tx.[GMRawScore],
		ec.[GMAE] = tx.[GMAE] ,
		ec.[GMMonthsDelay] =tx.[GMMonthsDelay],
		ec.[GMPCTDelay] = tx.[GMPCTDelay],
		ec.[FMRawScore] = tx.[FMRawScore],
		ec.[FMAEMonths] = tx.[FMAEMonths],	
		ec.[PMRawScore] = tx.[PMRawScore],
		ec.[PMAEMonths]=tx.[PMAEMonths],
		ec.[FMPMAE] = tx.[FMPMAE] ,
		ec.[FMPMMonthsDelay] = tx.[FMPMMonthsDelay] ,
		ec.[FMPMPCTDelay] =tx.[FMPMPCTDelay] ,
		ec.[AMRS] = tx.[AMRS] ,
		ec.[AMAEMonths] = tx.[AMAEMonths] ,
		ec.[RARawScore] =tx.[RARawScore], 
		ec.[RAAEMonths] = tx.[RAAEMonths],
		ec.[PCRawScore] =tx.[PCRawScore],
		ec.[PCAEMonths] = tx.[PCAEMonths],
		ec.[CogAE] = tx.[CogAE],
		ec.[CogMonthsDelay] = tx.[CogMonthsDelay],
		ec.[CogPCTDelay] = tx.[CogPCTDelay],
		ec.IsActive = 1,
		ec.ModifiedBy = @ModifiedBy,
		ec.ModifiedOn = @ModifiedOn,
		ec.SystemModifiedOn = GETUTCDATE()
	FROM
		[ECI].[EligibilityCalculations] ec
		INNER JOIN #TmpXMLData tx
			ON ec.EligibilityCalculationID = tx.EligibilityCalculationID
	WHERE
		ec.EligibilityCalculationID = tx.EligibilityCalculationID;

	IF EXISTS (SELECT TOP 1 * FROM #TmpXMLData WHERE EligibilityCalculationID IS NOT NULL)
		BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT EligibilityCalculationID, AuditDetailID FROM #TmpXMLData WHERE EligibilityCalculationID IS NOT NULL;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'EligibilityCalculations', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
		END;

	END TRY

	BEGIN CATCH
		SELECT  @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH

END