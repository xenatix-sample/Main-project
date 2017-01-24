-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	Reference.[usp_DeleteCredentials]
-- Author:		John Crossen
-- Date:		08/12/2015
--
-- Purpose:		Update Credential details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/12/2015	John Crossen TFS#1182 		Initial creation.
-- 9/23/2015    John Crossen                Add Audit
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteCredential]
	@CredentialID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY	
	DECLARE @AuditDetailID BIGINT;

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Reference', 'Credentials', @CredentialID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Reference.[Credentials]
	SET 
		IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE 
		CredentialID = @CredentialID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Reference', 'Credentials', @AuditDetailID, @CredentialID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


