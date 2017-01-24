-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddFinancialSummaryConfirmationStatement]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Add Financial Summary Confirmation Statement record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddFinancialSummaryConfirmationStatement]
	@FinancialSummaryID BIGINT,
	@ConfirmationStatementID INT,
	@DateSigned DATE,
	@SignatureStatusID INT,
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
	DECLARE @ContactID BIGINT;

	SELECT @ContactID = ContactID FROM Registration.FinancialSummary WHERE FinancialSummaryID = @FinancialSummaryID;

	INSERT INTO Registration.FinancialSummaryConfirmationStatement
	(
		FinancialSummaryID,
		ConfirmationStatementID,
		DateSigned,
		SignatureStatusID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@FinancialSummaryID,
		@ConfirmationStatementID,
		@DateSigned,
		@SignatureStatusID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);

	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'FinancialSummaryConfirmationStatement', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'FinancialSummaryConfirmationStatement', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
