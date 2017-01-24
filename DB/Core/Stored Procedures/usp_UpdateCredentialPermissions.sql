-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateCredentialPermissions]
-- Author:		John Crossen	
-- Date:		08/14/2015
--
-- Purpose:		Update modifiable Phone Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/14/2015 - John Crossen - TFS# 1182     Initial draft
-- 09/25/2015 - John Crossen    - Refactor Proc to use PK for update
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateCredentialPermissions]
	@CredentialPermissionID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@IsActive BIT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);	

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed Successfully';

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'CredentialPermission', @CredentialPermissionID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	
		
	UPDATE Core.CredentialPermission
	SET IsActive = @IsActive,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		CredentialPermissionID = @CredentialPermissionID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'CredentialPermission', @AuditDetailID, @CredentialPermissionID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
