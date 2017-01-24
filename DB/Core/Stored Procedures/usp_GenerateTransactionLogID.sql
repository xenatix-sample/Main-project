-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GenerateTransactionLogID]
-- Author:		Scott Martin
-- Date:		09/16/2016
--
-- Purpose:		Generates a unique ID for grouping items together 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/16/2016	Scott Martin		Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_GenerateTransactionLogID]
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @TransactionID INT,
		@TransactionLogID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	
	SELECT @TransactionID = ISNULL(MAX(TransactionID), 0) + 1 FROM Core.TransactionLog WHERE DATEDIFF(DAY, TransactionDate, GETUTCDATE()) = 0;

	SET @TransactionLogID = CAST(CONVERT(VARCHAR(8), GETUTCDATE(), 112) + REPLACE(STR(@TransactionID, 8), SPACE(1), '0') AS BIGINT);

	INSERT INTO Core.TransactionLog
	(
		TransactionID,
		TransactionDate,
		TransactionLogID,
		CreatedBy
	)
	VALUES
	(
		@TransactionID,
		GETUTCDATE(),
		@TransactionLogID,
		@ModifiedBy
	);

	SELECT CONVERT(NVARCHAR(16), @TransactionLogID);

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END