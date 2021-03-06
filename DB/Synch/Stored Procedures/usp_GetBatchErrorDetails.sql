-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_GetBatchErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		08/31/2016
--
-- Purpose:		Get the error details of the Batch
--
-- Notes:		n/a
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/31/2016	Sumana Sangapu	Initial creation.
-- 09/03/2016	Rahul Vats		Review the proc
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_GetBatchErrorDetails]
	@BatchID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT @ResultCode = 0,
			@ResultMessage = 'executed successfully'
		
		SELECT BatchID, PackageName, PackageTask, ErrorCode, ErrorDescription, CreatedOn
		FROM Synch.BatchErrorDetails
		WHERE BatchID = @BatchID
			  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO