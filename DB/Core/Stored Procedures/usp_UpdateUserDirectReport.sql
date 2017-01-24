-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateUserDirectReport]
-- Author:		Scott Martin
-- Date:		02/29/2016
--
-- Purpose:		Update a direct report mapping  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/29/2016	Scott Martin		Initial Creation
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserDirectReport]
	@UserID INT,
	@ParentID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UsersHierarchyMapping', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Core].[UsersHierarchyMapping]
	SET UserID = @UserID,
		ParentID = @ParentID,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE();
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UsersHierarchyMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END