
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	usp_UpdateUserIsInternal
-- Author:		Sumana SAngapu
-- Date:		04/08/2016
--
-- Purpose:		Update if the user is Internal or External
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/08/2016 Sumana Sangapu  - Initial Creation
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserIsInternal]
	@UserID INT,
	@IsInternal INT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'User updated successfully'
		   --<RolesXMLValue><RoleID>1</RoleID><RoleID>2</RoleID></RolesXMLValue>

	DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		    @AuditDetailID BIGINT,
			@EmailXML XML

	BEGIN TRY			
		--Update user data before handling the roles
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Users', @UserID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.[Users] 
		SET IsInternal = @IsInternal,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE UserID = @UserID

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Users', @AuditDetailID, @UserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END