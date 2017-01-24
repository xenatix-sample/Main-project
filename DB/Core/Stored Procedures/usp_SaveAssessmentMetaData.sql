-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_SaveAssessmentMetaData]
-- Author:		Scott Martin
-- Date:		04/15/2016
--
-- Purpose:		Proc used to store data outside of assessment engine
--
-- Notes:		
--				
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/15/2016	Scott Martin	Initial creation
-- 04/26/2016	Scott Martin	Added in a parameter to the signature count query
-- 05/06/2016	Scott Martin	Query wasn't calculating required signatures correctly
-- 05/11/2016	Scott Martin	Changed signed on to be from either LAR or Contact
-- 05/12/2016	Scott Martin	Query wasn't calculating required signatures correctly; Changed MAX to MIN for signedOn date as new records are created each time assessment is saved
-- 06/02/2016	Scott Martin	Added parameter inputs to Add/Update procs
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_SaveAssessmentMetaData
	@AssessmentID BIGINT,
	@ResponseID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
		
		SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully';

		BEGIN TRY
DECLARE @DocumentTypeID INT

SELECT
	@DocumentTypeID = DocumentTypeID
FROM
	Core.DocumentMapping DM
	INNER JOIN Core.Assessments A
		ON DM.AssessmentID = A.AssessmentID
WHERE
	DM.AssessmentID = @AssessmentID

IF @DocumentTypeID = 3
	BEGIN
	DECLARE @SigCount INT,
			@SigDate DATE,
			@SignatureStatusID INT = 1,
			@ContactID BIGINT,
			@ContactConsentID BIGINT,
			@EffectiveDate DATE,
			@AssessmentSectionID BIGINT

	;WITH SigCTE (ResponseID, EntityTypeID, SignedOn)
	AS
	(
		SELECT
			ResponseID,
			ES.EntityTypeID,
			MIN(ES.CreatedOn) OVER(PARTITION BY ResponseID)
		FROM
			Core.AssessmentResponseDetails ARD
			INNER JOIN ESignature.DocumentEntitySignatures DES
				ON ARD.ResponseDetailsID = DES.DocumentID
			INNER JOIN ESignature.EntitySignatures ES
				ON DES.EntitySignatureID = ES.EntitySignatureID
		WHERE
			ARD.IsActive = 1
			AND ARD.ResponseID = @ResponseID
			AND DES.DocumentTypeID = @DocumentTypeID
	)
	SELECT
		@SigCount = SUM(CASE WHEN CTE1.ResponseID IS NOT NULL THEN 1 ELSE 0 END) + SUM(CASE WHEN CTE2.ResponseID IS NOT NULL AND CTE3.ResponseID IS NULL THEN 1 ELSE 0 END) + SUM(CASE WHEN CTE3.ResponseID IS NOT NULL AND CTE2.ResponseID IS NULL THEN 1 ELSE 0 END) + SUM(CASE WHEN CTE3.ResponseID IS NOT NULL AND CTE2.ResponseID IS NOT NULL THEN 1 ELSE 0 END),
		@SigDate = COALESCE(MIN(CTE3.SignedOn), MIN(CTE2.SignedOn)),
		@ContactID = MAX(ContactID)
	FROM
		Core.AssessmentResponses AR
		INNER JOIN Core.Assessments A
			ON AR.AssessmentID = A.AssessmentID
		LEFT OUTER JOIN SigCTE CTE1
			ON AR.ResponseID = CTE1.ResponseID
			AND CTE1.EntityTypeID = 1
		LEFT OUTER JOIN SigCTE CTE2
			ON AR.ResponseID = CTE2.ResponseID
			AND CTE2.EntityTypeID = 2
		LEFT OUTER JOIN SigCTE CTE3
			ON AR.ResponseID = CTE3.ResponseID
			AND CTE3.EntityTypeID = 3
	WHERE
		AR.ResponseID = @ResponseID;

	SELECT TOP 1 @AssessmentSectionID = AssessmentSectionID FROM Core.AssessmentSections WHERE AssessmentID = @AssessmentID ORDER BY SortOrder;

	IF @SigCount > 1
		BEGIN
		SET @SignatureStatusID = 2;
		END

	IF NOT EXISTS (SELECT TOP 1 * FROM Registration.ContactConsent WHERE AssessmentID = @AssessmentID AND ResponseID = @ResponseID)
		BEGIN
		EXEC Registration.usp_AddContactConsent @ContactID, @AssessmentID, @AssessmentSectionID, @ResponseID, @SigDate, @ModifiedOn, NULL, NULL, NULL, NULL, @SignatureStatusID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, NULL;
		END
	ELSE
		BEGIN
		SELECT
			@ContactConsentID = ContactConsentID,
			@EffectiveDate = EffectiveDate
		FROM
			Registration.ContactConsent
		WHERE
			AssessmentID = @AssessmentID
			AND ResponseID = @ResponseID;

		EXEC Registration.usp_UpdateContactConsent @ContactConsentID, @ContactID, @AssessmentID, @AssessmentSectionID, @ResponseID, @SigDate, @EffectiveDate, NULL, NULL, NULL, NULL, @SignatureStatusID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END
	END

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END