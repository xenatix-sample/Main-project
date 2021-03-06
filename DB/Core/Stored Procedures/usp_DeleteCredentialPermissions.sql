-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_DeleteCredentialPermissions]
-- Author:		John Crossen
-- Date:		08/12/2015
--
-- Purpose:		Delete Credential Permissions
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/14/2015	John Crossen TFS#1182 		Initial creation.
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteCredentialPermissions]
	@CredentialID BIGINT,
	@PermissionID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	
	DECLARE @CredentialPermissionID BIGINT;
	DECLARE @AuditDetailID BIGINT;

	SELECT @CredentialPermissionID = CredentialPermissionID FROM Core.CredentialPermission WHERE CredentialID = @CredentialID AND PermissionID = @PermissionID;

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'CredentialPermission', @CredentialPermissionID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.[CredentialPermission]
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE 
		CredentialID = @CredentialID
		AND PermissionID = @PermissionID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'CredentialPermission', @AuditDetailID, @CredentialPermissionID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO

