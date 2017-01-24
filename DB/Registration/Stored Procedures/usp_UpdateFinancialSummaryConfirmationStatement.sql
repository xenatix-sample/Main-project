-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateFinancialSummaryConfirmationStatement]
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

CREATE PROCEDURE [Registration].[usp_UpdateFinancialSummaryConfirmationStatement]
	@FinancialSummaryConfirmationStatementID BIGINT,
	@FinancialSummaryID BIGINT,
	@ConfirmationStatementID INT,
	@DateSigned DATE,
	@SignatureStatusID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ContactID = ContactID FROM Registration.FinancialSummary FS INNER JOIN Registration.FinancialSummaryConfirmationStatement FSCS ON FS.FinancialSummaryID = FSCS.FinancialSummaryID WHERE FSCS.FinancialSummaryConfirmationStatementID = @FinancialSummaryConfirmationStatementID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'FinancialSummaryConfirmationStatement', @FinancialSummaryConfirmationStatementID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.FinancialSummaryConfirmationStatement
	SET	FinancialSummaryID = @FinancialSummaryID,
		ConfirmationStatementID = @ConfirmationStatementID,
		DateSigned = @DateSigned,
		SignatureStatusID = @SignatureStatusID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		FinancialSummaryConfirmationStatementID = @FinancialSummaryConfirmationStatementID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'FinancialSummaryConfirmationStatement', @AuditDetailID, @FinancialSummaryConfirmationStatementID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();					
	END CATCH
END
