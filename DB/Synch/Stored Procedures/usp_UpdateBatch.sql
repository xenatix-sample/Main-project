-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Synch].[usp_UpdateBatch]
-- Author:		Chad Roberts
-- Date:		1/26/2016
--
-- Purpose:		Update a Batch
--
-- Notes:		n/a
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/26/2016	Chad Roberts		Initial creation.
-- 03/03/2016	Sumana Sangapu		Added BatchTypeID column	
-- 09/03/2016	Rahul Vats			Review the proc
-- 09/16/2016	Sumana Sangapu		Update the SysmtemModifiedOn Date
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Synch].[usp_UpdateBatch]
	@BatchID INT,
	@BatchStatusID INT,
	@BatchTypeID INT,
	@ConfigID INT,
	@USN BIGINT,
	@IsActive bit,
	@ModifiedOn DateTime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT -- This is not needed.
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	if (@ModifiedBy = 0)
	Begin
		select @ModifiedBy = 1
	End

	UPDATE [Synch].[Batch]
	SET BatchStatusID = @BatchStatusID,
		BatchTypeID = @BatchTypeID,
		ConfigID = @ConfigID,
		USN = @USN,
		IsActive = @IsActive,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = GETUTCDATE(),
		SystemModifiedOn = GETUTCDATE()
	WHERE
		BatchID = @BatchID

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO