-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Core.[usp_AssignCredentialPermission]
-- Author:		John Crossen
-- Date:		08/12/2015
--
-- Purpose:		Assign Credential Permissions
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	John Crossen TFS#1182 		Initial creation.
-- 09/22/2015   John Crossen        Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 02/17/2016	Scott Martin		Refactored for audit loggin
--------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE Core.usp_AssignCredentialPermission
	@CredentialID BIGINT,
	@PermissionID BIGINT,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
DECLARE @ID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF EXISTS(SELECT 1 FROM Core.CredentialPermission WHERE CredentialID = @CredentialID AND PermissionID=@PermissionID  AND [IsActive]=1)
		 BEGIN
			RAISERROR ('CredentialPermission Pairing can not be duplicate.', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		 END	

	INSERT INTO Core.CredentialPermission
	(
		CredentialID,
		PermissionID,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn	          
	)
	VALUES
	(
		@CredentialID , 
		@PermissionID ,
		@IsActive,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
	       
	SELECT @ID = SCOPE_IDENTITY();

	DECLARE @AuditDetailID BIGINT;

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'CredentialPermission', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'CredentialPermission', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END