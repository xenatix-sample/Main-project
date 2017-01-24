-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_AddBatchErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		08/16/2016
--
-- Purpose:		Add Batch Error Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/16/2016	Sumana Sangapu	Initial creation.
-- 09/03/2016	Rahul Vats		Review the proc
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_AddBatchErrorDetails]
	@BatchID INT,
	@BatchStatusID int,
	@PackageName nvarchar(100),
	@PackageTask nvarchar(50),
	@ErrorCode nvarchar(50),
	@ErrorDescription nvarchar(1000),
	@ModifiedOn datetime,
	@ModifiedBy int,
	@CreatedBy int,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully';

		INSERT INTO [Synch].[BatchErrorDetails] 
		( BatchID, BatchStatusID, PackageName, PackageTask, ErrorCode, ErrorDescription, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn )
		VALUES 
		(	@BatchID, 
			@BatchStatusID,
			@PackageName, 
			@PackageTask,
			@ErrorCode, 
			@ErrorDescription, 
			1,
			@ModifiedBy,
			@ModifiedOn,
			@CreatedBy,
			GETUTCDATE(),
			GETUTCDATE(),
			GETUTCDATE()
		)
		 
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO