-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateUserPhoto]
-- Author:		Scott Martin
-- Date:		02/25/2016
--
-- Purpose:		Update User Photo  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/25/2015	Scott Martin		Initial Creation
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserPhoto]
	@UserPhotoID BIGINT,
	@UserID INT,
	@PhotoID BIGINT,
	@IsPrimary BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ExistingPrimaryID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	SELECT @ExistingPrimaryID = UserPhotoID FROM Core.UserPhoto WHERE UserID = @UserID AND IsPrimary = 1;

 	IF @ExistingPrimaryID <> @UserPhotoID AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserPhoto', @ExistingPrimaryID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.UserPhoto
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			UserPhotoID = @ExistingPrimaryID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserPhoto', @AuditDetailID, @ExistingPrimaryID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			
		END
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserPhoto', @UserPhotoID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Core].[UserPhoto]
	SET	UserID = @UserID,
		PhotoID = @PhotoID,
		IsPrimary = @IsPrimary,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		UserPhotoID = @UserPhotoID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserPhoto', @AuditDetailID, @UserPhotoID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END