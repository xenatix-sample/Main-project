-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddGroupSchedulingHeader]
-- Author:		Sumana Sangapu
-- Date:		02/10/2016
--
-- Purpose:		Add Group Scheduling Header
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/10/2016	 Sumana Sangapu  Initial creation.
-- 03/18/2016	 Sumana Sangapu  Added Audit Trail Code
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Scheduling].[usp_AddGroupSchedulingHeader]
	@GroupDetailID BIGINT,
	@Comments NVARCHAR(1000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS

BEGIN
	DECLARE @AuditPost XML,
			@AuditID BIGINT,
			@PrimaryKeyValue BIGINT;

	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);
		  
	SELECT
		@ResultCode = 0,
		@ResultMessage = 'Executed Successfully';

	BEGIN TRY
		INSERT INTO [Scheduling].[GroupSchedulingHeader]
		(GroupDetailID,  Comments, IsActive, ModifiedBy,  ModifiedOn, CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn )
		VALUES( @GroupDetailID,  @Comments,1, @ModifiedBy, 
		@ModifiedOn, @ModifiedBy, @ModifiedOn, GETUTCDATE(), GETUTCDATE()  );

		SELECT @ID = SCOPE_IDENTITY();

		EXECUTE Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Scheduling', 'GroupSchedulingHeader', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXECUTE Auditing.usp_AddPostAuditLog 'Insert', 'Scheduling', 'GroupSchedulingHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	 
	END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE();
  END CATCH;

END;