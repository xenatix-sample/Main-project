-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdatePhoto]
-- Author:		Scott Martin
-- Date:		12/29/2015
--
-- Purpose:		Update a Photo  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/29/2015	Scott Martin		Initial Creation
-- 01/11/2016	Rajiv Ranjan		Added where condition & check handled empty photoBLOB
-- 02/24/2016	Scott Martin		Moved from Registration to Core Schema
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdatePhoto]
	@PhotoID BIGINT,
	@PhotoBLOB VARBINARY(MAX),
	@ThumbnailBLOB VARBINARY(MAX),
	@TakenBy INT,
	@TakenTime DATETIME,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'Photo', @PhotoID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
	UPDATE [Core].[Photo]
	SET	PhotoBLOB = ISNULL(@PhotoBLOB, PhotoBLOB),
		ThumbnailBLOB = @ThumbnailBLOB,
		TakenBy = @TakenBy,
		TakenTime = @TakenTime,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		PhotoID = @PhotoID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'Photo', @AuditDetailID, @PhotoID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END