-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddFinancialSummary]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Add Financial Summary record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddFinancialSummary]
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
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	INSERT INTO Registration.FinancialSummary
	(
		ContactID,
		OrganizationID,
		FinancialAssessmentXML,
		DateSigned,
		EffectiveDate,
		AssessmentEndDate,
		ExpirationDate,
		SignatureStatusID,
		UserID,
		UserPhoneID,
		CredentialID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@OrganizationID,
		@FinancialAssessmentXML,
		@DateSigned,
		@EffectiveDate,
		@AssessmentEndDate,
		@ExpirationDate,
		@SignatureStatusID,
		@UserID,
		@UserPhoneID,
		@CredentialID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'FinancialSummary', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'FinancialSummary', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
