-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_UpdateBatchDetails]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Update Batch Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/01/2016	Sumana Sangapu	Initial creation.
-- 09/03/2016	Rahul Vats		Review the proc
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_UpdateBatchDetails]
	@BatchID INT,
	@BatchStatusID int,
	@PackageName nvarchar(100),
	@PackageTask nvarchar(50),
	@ModifiedBy int,
	@CreatedBy int,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT -- This is not needed.
AS
BEGIN

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully';

		UPDATE a
		SET
			[BatchStatusID] = @BatchStatusID,
			[PackageName] = @PackageName,
			[PackageTask] = @PackageTask,
			[ModifiedOn] = GETUTCDATE(),
			[SystemModifiedOn] = GETUTCDATE()
		FROM [Synch].[BatchDetails] a
		WHERE  [BatchID] = @BatchID
		
		 
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO