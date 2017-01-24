-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_DeleteUserDirectReport
-- Author:		Scott Martin
-- Date:		02/26/2016
--
-- Purpose:		Deactivates a user direct report
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/26/2016	Scott Martin		Initial Creation
-- 03/03/2016	Gurpreet Singh		Updated table name in which record is set to inactive.
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteUserDirectReport]
	@MappingID INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@UserID INT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	DECLARE @AuditDetailID BIGINT;

	SELECT @UserID = UserID FROM Core.UsersHierarchyMapping WHERE MappingID = @MappingID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'UsersHierarchyMapping', @MappingID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.[UsersHierarchyMapping] 
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		MappingID = @MappingID

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'UsersHierarchyMapping', @AuditDetailID, @MappingID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END