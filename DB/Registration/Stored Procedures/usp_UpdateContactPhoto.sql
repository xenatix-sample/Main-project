-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactPhoto]
-- Author:		Scott Martin
-- Date:		12/29/2015
--
-- Purpose:		Update Contact Photo  
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/29/2015	Scott Martin		Initial Creation
-- 01/16/2016	Rajiv Ranjan		Removed @ID parameter, not required.
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateContactPhoto]
	@ContactPhotoID BIGINT,
	@ContactID BIGINT,
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
	SELECT @ExistingPrimaryID = ContactPhotoID FROM Registration.ContactPhoto WHERE ContactID = @ContactID AND IsPrimary = 1;

 	IF @ExistingPrimaryID <> @ContactPhotoID AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPhoto', @ExistingPrimaryID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactPhoto
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE
			ContactPhotoID = @ExistingPrimaryID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPhoto', @AuditDetailID, @ExistingPrimaryID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			
		END
		
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPhoto', @ContactPhotoID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[ContactPhoto]
	SET	ContactID = @ContactID,
		PhotoID = @PhotoID,
		IsPrimary = @IsPrimary,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactPhotoID = @ContactPhotoID;
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPhoto', @AuditDetailID, @ContactPhotoID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;			

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END