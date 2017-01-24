-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetFinancialSummaryConfirmationStatement]
-- Author:		Scott Martin
-- Date:		04/09/2016
--
-- Purpose:		Get Confirmation Statements for Contact Financial Assessment
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016	Scott Martin	Initial Creation
-- 04/12/2016	Scott Martin	Added ConfirmationStatement field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetFinancialSummaryConfirmationStatement]
	@FinancialSummaryID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
	SELECT	@ResultCode = 0,
			@ResultMessage = 'executed successfully';

	SELECT
		FinancialSummaryConfirmationStatementID,
		FinancialSummaryID,
		FS.ConfirmationStatementID,
		CS.ConfirmationStatement,
		DateSigned,
		SignatureStatusID
	FROM
		Registration.FinancialSummaryConfirmationStatement FS
		INNER JOIN Reference.ConfirmationStatement CS
			ON FS.ConfirmationStatementID = CS.ConfirmationStatementID
	WHERE
		FS.IsActive = 1
		AND FinancialSummaryID = @FinancialSummaryID;

	END TRY

	BEGIN CATCH
		SELECT	@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO

