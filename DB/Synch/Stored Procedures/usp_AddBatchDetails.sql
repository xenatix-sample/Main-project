 -----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_AddBatchDetails]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Add Batch Details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
--03/01/2016	Sumana Sangapu	Initial creation.
-- 09/03/2016	Rahul Vats		Review the proc
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_AddBatchDetails]
	@BatchID INT,
	@BatchStatusID int,
	@PackageName nvarchar(100),
	@PackageTask nvarchar(50),
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

		INSERT INTO [Synch].[BatchDetails]
		(
			[BatchID] ,
			[BatchStatusID],
			[PackageName],
			[PackageTask],
			[IsActive] ,
			[ModifiedBy] ,
			[ModifiedOn] ,
			[CreatedBy],
			[CreatedOn] ,
			[SystemCreatedOn],
			[SystemModifiedOn]
		)
		VALUES
		(
 			@BatchID ,
			@BatchStatusID,
			@PackageName,
			@PackageTask,
			1,
			@ModifiedBy,
			GETUTCDATE(),
			@CreatedBy,
			GETUTCDATE(),
			GETUTCDATE(),
			GETUTCDATE()
		);
	SELECT @ID = SCOPE_IDENTITY();
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO