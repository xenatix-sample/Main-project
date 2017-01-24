-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteUserPhoto]
-- Author:		Scott Martin
-- Date:		02/25/2016
--
-- Purpose:		Delete a User Photo  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/25/2016	Scott Martin		Initial Creation
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteUserPhoto]
	@UserPhotoID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@UserID INT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT @UserID = UserID FROM Core.UserPhoto WHERE UserPhotoID = @UserPhotoID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'UserPhoto', @UserPhotoID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Core].[UserPhoto]
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		UserPhotoID = @UserPhotoID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'UserPhoto', @AuditDetailID, @UserPhotoID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
	  
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END