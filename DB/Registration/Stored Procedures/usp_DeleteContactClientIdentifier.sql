-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_DeleteContactClientIdentifier]
-- Author:		Scott Martin
-- Date:		12/23/2015
--
-- Purpose:		Delete contact alternate id
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/23/2015	Scott Martin	Initial creation.
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-- 09/16/2016	Scott Martin		Added TransactionLogID and ModuleComponentID and modified Audit proc call
-- 09/18/2016	Kyle Campbell	TFS #14753	Added proc call for change log
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteContactClientIdentifier]
	@TransactionLogID BIGINT,
	@ModuleComponentID BIGINT,
	@ContactClientIdentifierID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID FROM Registration.ContactClientIdentifier WHERE ContactClientIdentifierID = @ContactClientIdentifierID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'ContactClientIdentifier', @ContactClientIdentifierID, NULL, @TransactionLogID, @ModuleComponentID, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Registration.ContactClientIdentifier
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ContactClientIdentifierID = @ContactClientIdentifierID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'ContactClientIdentifier', @AuditDetailID, @ContactClientIdentifierID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
	
	EXEC Auditing.usp_AddContactClientIdentifierChangeLog @TransactionLogID, @ContactClientIdentifierID, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT
 	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


