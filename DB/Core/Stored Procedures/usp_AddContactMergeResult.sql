-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_AddContactMergeResult
-- Author:		Scott Martin
-- Date:		08/15/2016
--
-- Purpose:		Store the outcome of merged records
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/15/2016 - Initial procedure creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddContactMergeResult](
	@MergedContactsMappingID BIGINT,
	@ModuleComponentID BIGINT,
	@TotalRecords INT,
	@TotalRecordsMerged INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
	DECLARE @IsSuccessful BIT;
		
	IF @ResultCode <> 0
		BEGIN
		SET @IsSuccessful = 0;
		END
	ELSE
		BEGIN
		SET @IsSuccessful = 1;
		END

	INSERT INTO Core.MergedContactResult
	(
		MergedContactsMappingID,
		ModuleComponentID,
		IsSuccessful,
		TotalRecords,
		TotalRecordsMerged,
		ResultMessage
	)
	VALUES
	(
		@MergedContactsMappingID,
		@ModuleComponentID,
		@IsSuccessful,
		@TotalRecords,
		@TotalRecordsMerged,
		@ResultMessage
	);

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END