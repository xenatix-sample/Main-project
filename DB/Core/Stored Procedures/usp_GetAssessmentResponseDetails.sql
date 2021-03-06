----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_GetAssessmentResponseDetails]
-- Author:		Rajiv Ranjan
-- Date:		09/17/2015
--
-- Purpose:		Get the response details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/07/2015	-	Rajiv Ranjan - Initial Creation
-- 10/08/2015	-	Rajiv Ranjan - Removed AssessmentSection from inner join as AssessmentSectionID is added into AssessmentResponses
-- 10/11/2015	-	Demetrios C. Christopher - Ensured that the correct response's details are obtained (ResponseID vs AssessmentID + ContactID)
-- 03/21/2016	-	Kyle Campbell	TFS #6596	Added QuestionDataKey and OptionDataKey
-- 04/18/2016		Scott Martin	Added SignatureBLOB
-- 05/02/2016		Atul Chauhan	Code commented
-- 03/05/2016		Rajiv Ranjan	Query optimization - corrected joins
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GetAssessmentResponseDetails]
	@ResponseID BIGINT,
	@AssessmentSectionID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT 
AS
BEGIN

	SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'
		
	BEGIN TRY
	-- Code commented till data snapshot fully functional
	--IF EXISTS (SELECT TOP 1 * FROM Core.AssessmentResponseDetailsAudit WHERE ResponseID = @ResponseID)
	--	BEGIN
	--	SELECT
	--		@ResponseID AS ResponseID,
	--		0 AS ResponseDetailsID,
	--		0 AS QuestionID,
	--		0 AS AssessmentSectionID,
	--		NULL AS OptionsID,
	--		'' AS ResponseText,
	--		0 AS Rating,
	--		NULL AS SignatureBLOB,
	--		AuditXML
	--	FROM
	--		Core.AssessmentResponseDetailsAudit ARDA
	--	WHERE
	--		ARDA.ResponseID = @ResponseID
	--		AND ARDA.IsActive = 1;
	--	END
	--ELSE
		BEGIN
		SELECT	
			ar.ResponseID,
			ard.ResponseDetailsID,					
			ard.QuestionID AS QuestionID, 			
			ard.AssessmentSectionID,
			ard.OptionsID,
			ard.ResponseText,
			ard.Rating,
			AO.Options,
			aq.DataKey As QuestionDataKey,
			ao.DataKey As OptionDataKey,
			s.SignatureBLOB
		FROM	
			[Core].[AssessmentResponses] ar					
			INNER JOIN [Core].[AssessmentResponseDetails] ard ON ar.ResponseID = ard.ResponseID 
			INNER JOIN [Core].[AssessmentQuestions] AQ ON AQ.QuestionID = ard.QuestionID			
			LEFT JOIN [Core].[AssessmentOptions] AO ON AO.OptionsID = ard.OptionsID
			INNER JOIN Core.DocumentMapping DM ON DM.AssessmentID = ar.AssessmentID
			LEFT JOIN ESignature.DocumentEntitySignatures DESS ON DESS.DocumentID = ard.ResponseDetailsID AND DESS.DocumentTypeID = DM.DocumentTypeID
			LEFT JOIN ESignature.EntitySignatures ES ON ES.EntitySignatureID = DESS.EntitySignatureID
			LEFT JOIN ESignature.Signatures S ON S.SignatureID = ES.SignatureID
		WHERE	
			ar.ResponseID = @ResponseID
			AND ard.AssessmentSectionID = @AssessmentSectionID
			AND ard.IsActive = 1
		END				
	END TRY
	BEGIN CATCH
			SELECT  @ResultCode = ERROR_SEVERITY(),
					@ResultMessage = ERROR_MESSAGE()
	END CATCH

END
