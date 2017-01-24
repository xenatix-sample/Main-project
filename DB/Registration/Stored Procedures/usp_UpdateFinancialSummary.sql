-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateFinancialSummary]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Update Financial Summary record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateFinancialSummary]
	@FinancialSummaryID BIGINT,
	@ContactID BIGINT,
	@OrganizationID BIGINT,
	@FinancialAssessmentXML XML,
	@DateSigned DATE,
	@EffectiveDate DATE,
	@AssessmentEndDate DATE,
	@ExpirationDate DATE,
	@SignatureStatusID INT,
	@UserID INT,
	@UserPhoneID BIGINT,
	@CredentialID BIGINT,	
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'FinancialSummary', @FinancialSummaryID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.FinancialSummary
	SET	ContactID = @ContactID,
		OrganizationID = @OrganizationID,
		FinancialAssessmentXML = @FinancialAssessmentXML,
		DateSigned = @DateSigned,
		EffectiveDate = @EffectiveDate,
		AssessmentEndDate = @AssessmentEndDate,
		ExpirationDate = @ExpirationDate,
		SignatureStatusID = @SignatureStatusID,
		UserID = @UserID,
		UserPhoneID = @UserPhoneID,
		CredentialID = @CredentialID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		FinancialSummaryID = @FinancialSummaryID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'FinancialSummary', @AuditDetailID, @FinancialSummaryID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
